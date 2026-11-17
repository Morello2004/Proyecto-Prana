-- Ejecutar manualmente en SSMS, con Prana cerrado y un respaldo verificado.
-- No ejecutar con versiones anteriores de Prana: requieren la columna eliminada.
-- Solo modifica el DEFAULT y la columna dbo.Detalle_Venta.entregado.
USE [PranaElectronica];
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET LOCK_TIMEOUT 15000;

BEGIN TRY
    IF DB_NAME() <> N'PranaElectronica'
        THROW 50099, N'La base seleccionada no es PranaElectronica.', 1;
    BEGIN TRANSACTION;

    DECLARE @resultado int;
    EXEC @resultado = sys.sp_getapplock
        @Resource = N'Prana.ActualizarEsquema', @LockMode = N'Exclusive',
        @LockOwner = N'Transaction', @LockTimeout = 15000;
    IF @resultado < 0
        THROW 50100, N'No se pudo bloquear la migracion. Cierre Prana y reintente.', 1;

    DECLARE @tabla int = OBJECT_ID(N'dbo.Detalle_Venta', N'U');
    IF @tabla IS NULL
        THROW 50101, N'No existe dbo.Detalle_Venta en la base seleccionada.', 1;

    DECLARE @columna int = COLUMNPROPERTY(@tabla, N'entregado', 'ColumnId');
    IF @columna IS NULL
    BEGIN
        COMMIT;
        PRINT N'La columna ya no existe. No se realizaron cambios.';
        RETURN;
    END;

    IF EXISTS (
        SELECT 1 FROM (VALUES
            (N'Detalle_Venta', N'id_venta'), (N'Detalle_Venta', N'id_variante'),
            (N'Detalle_Venta', N'cantidad'), (N'Detalle_Venta', N'entregado'),
            (N'Venta', N'id_venta'),
            (N'Remito', N'id_remito'), (N'Remito', N'id_venta'),
            (N'Detalle_Remito', N'id_remito'), (N'Detalle_Remito', N'id_variante'),
            (N'Detalle_Remito', N'cantidad'), (N'Envio', N'id_remito'),
            (N'Envio', N'entregado'), (N'Envio', N'fecha_hora_entrega')
        ) requeridas(tabla, columna)
        WHERE OBJECT_ID(N'dbo.' + QUOTENAME(requeridas.tabla), N'U') IS NULL
           OR COL_LENGTH(N'dbo.' + QUOTENAME(requeridas.tabla), requeridas.columna) IS NULL
    )
        THROW 50102, N'Faltan tablas o columnas necesarias para verificar las entregas.', 1;

    DECLARE @default int = (
        SELECT default_object_id FROM sys.columns
        WHERE object_id = @tabla AND column_id = @columna
    );

    -- Bloquear dependencias distintas del DEFAULT. No se eliminan automaticamente.
    -- Una referencia de modulo a nivel de tabla tambien exige revision manual.
    IF EXISTS (
        SELECT 1 FROM sys.sql_expression_dependencies
        WHERE referenced_id = @tabla AND referenced_minor_id IN (0, @columna)
          AND referencing_id <> COALESCE(@default, 0)
    ) OR EXISTS (
        SELECT 1 FROM sys.index_columns WHERE object_id = @tabla AND column_id = @columna
    ) OR EXISTS (
        SELECT 1 FROM sys.stats_columns sc JOIN sys.stats s
          ON s.object_id = sc.object_id AND s.stats_id = sc.stats_id
        WHERE sc.object_id = @tabla AND sc.column_id = @columna AND s.user_created = 1
    ) OR EXISTS (
        SELECT 1 FROM sys.foreign_key_columns
        WHERE (parent_object_id = @tabla AND parent_column_id = @columna)
           OR (referenced_object_id = @tabla AND referenced_column_id = @columna)
    ) OR EXISTS (
        SELECT 1 FROM sys.check_constraints
        WHERE parent_object_id = @tabla AND parent_column_id = @columna
    ) OR EXISTS (
        SELECT 1 FROM sys.triggers WHERE parent_id = @tabla AND is_ms_shipped = 0
    )
    BEGIN
        SELECT OBJECT_SCHEMA_NAME(referencing_id) esquema,
               OBJECT_NAME(referencing_id) objeto, referenced_minor_id columna_referida
        FROM sys.sql_expression_dependencies
        WHERE referenced_id = @tabla AND referenced_minor_id IN (0, @columna)
          AND referencing_id <> COALESCE(@default, 0);
        SELECT i.name indice FROM sys.index_columns ic
        JOIN sys.indexes i ON i.object_id = ic.object_id AND i.index_id = ic.index_id
        WHERE ic.object_id = @tabla AND ic.column_id = @columna;
        SELECT name trigger_para_revisar FROM sys.triggers
        WHERE parent_id = @tabla AND is_ms_shipped = 0;
        THROW 50103, N'Hay dependencias adicionales. Reviselas antes de eliminar la columna.', 1;
    END;

    -- SQL dinamico evita errores de compilacion en una segunda ejecucion sin columna.
    EXEC sys.sp_executesql N'
        DECLARE @filas bigint;
        -- Mantener inmovil la informacion durante la comprobacion y el DROP.
        SELECT @filas = COUNT_BIG(*) FROM dbo.Detalle_Venta WITH (TABLOCKX, HOLDLOCK);
        SELECT @filas = COUNT_BIG(*) FROM dbo.Remito WITH (TABLOCKX, HOLDLOCK);
        SELECT @filas = COUNT_BIG(*) FROM dbo.Detalle_Remito WITH (TABLOCKX, HOLDLOCK);
        SELECT @filas = COUNT_BIG(*) FROM dbo.Envio WITH (TABLOCKX, HOLDLOCK);
        SELECT @filas = COUNT_BIG(*) FROM dbo.Venta WITH (TABLOCKX, HOLDLOCK);

        CREATE TABLE #Problemas (
            motivo nvarchar(200) NOT NULL, id_venta int NULL,
            id_variante int NULL, id_remito int NULL
        );

        INSERT #Problemas
        SELECT N''Detalle de venta sin venta'', d.id_venta, d.id_variante, NULL
        FROM dbo.Detalle_Venta d WHERE NOT EXISTS (
            SELECT 1 FROM dbo.Venta v WHERE v.id_venta = d.id_venta);

        INSERT #Problemas
        SELECT N''Remito sin venta'', r.id_venta, NULL, r.id_remito
        FROM dbo.Remito r WHERE NOT EXISTS (
            SELECT 1 FROM dbo.Venta v WHERE v.id_venta = r.id_venta);

        INSERT #Problemas
        SELECT N''El remito debe tener exactamente un envio'', r.id_venta, NULL, r.id_remito
        FROM dbo.Remito r LEFT JOIN dbo.Envio e ON e.id_remito = r.id_remito
        GROUP BY r.id_venta, r.id_remito HAVING COUNT(e.id_remito) <> 1;

        INSERT #Problemas
        SELECT N''Envio sin remito'', NULL, NULL, e.id_remito
        FROM dbo.Envio e WHERE NOT EXISTS (
            SELECT 1 FROM dbo.Remito r WHERE r.id_remito = e.id_remito);

        INSERT #Problemas
        SELECT N''Remito sin detalles'', r.id_venta, NULL, r.id_remito
        FROM dbo.Remito r WHERE NOT EXISTS (
            SELECT 1 FROM dbo.Detalle_Remito dr WHERE dr.id_remito = r.id_remito);

        INSERT #Problemas
        SELECT N''Detalle de remito sin remito'', NULL, dr.id_variante, dr.id_remito
        FROM dbo.Detalle_Remito dr WHERE NOT EXISTS (
            SELECT 1 FROM dbo.Remito r WHERE r.id_remito = dr.id_remito);

        INSERT #Problemas
        SELECT N''Cantidad vendida no positiva o estado historico nulo'', id_venta, id_variante, NULL
        FROM dbo.Detalle_Venta WHERE cantidad IS NULL OR cantidad <= 0 OR entregado IS NULL;

        INSERT #Problemas
        SELECT N''Cantidad preparada no positiva'', r.id_venta, dr.id_variante, dr.id_remito
        FROM dbo.Detalle_Remito dr LEFT JOIN dbo.Remito r ON r.id_remito = dr.id_remito
        WHERE dr.cantidad IS NULL OR dr.cantidad <= 0;

        INSERT #Problemas
        SELECT N''Estado de envio y fecha de entrega inconsistentes'', r.id_venta, NULL, e.id_remito
        FROM dbo.Envio e LEFT JOIN dbo.Remito r ON r.id_remito = e.id_remito
        WHERE e.entregado IS NULL
           OR (e.entregado = 1 AND e.fecha_hora_entrega IS NULL)
           OR (e.entregado = 0 AND e.fecha_hora_entrega IS NOT NULL);

        SELECT id_venta, id_variante, SUM(CONVERT(bigint, cantidad)) vendidas,
               MIN(CONVERT(int, entregado)) minimo_historico,
               MAX(CONVERT(int, entregado)) maximo_historico
        INTO #Vendidas FROM dbo.Detalle_Venta GROUP BY id_venta, id_variante;

        SELECT r.id_venta, dr.id_variante, SUM(CONVERT(bigint, dr.cantidad)) preparadas
        INTO #Preparadas FROM dbo.Remito r
        JOIN dbo.Detalle_Remito dr ON dr.id_remito = r.id_remito
        GROUP BY r.id_venta, dr.id_variante;

        SELECT r.id_venta, dr.id_variante, SUM(CONVERT(bigint, dr.cantidad)) entregadas
        INTO #Entregadas FROM dbo.Remito r
        JOIN dbo.Detalle_Remito dr ON dr.id_remito = r.id_remito
        WHERE EXISTS (SELECT 1 FROM dbo.Envio e WHERE e.id_remito = r.id_remito AND e.entregado = 1)
        GROUP BY r.id_venta, dr.id_variante;

        INSERT #Problemas
        SELECT N''Preparacion sin variante vendida o cantidad superior a la vendida'',
               p.id_venta, p.id_variante, NULL
        FROM #Preparadas p LEFT JOIN #Vendidas v
          ON v.id_venta = p.id_venta AND v.id_variante = p.id_variante
        WHERE v.id_venta IS NULL OR p.preparadas > v.vendidas;

        -- Revisar tambien ventas anuladas: su informacion historica no se descarta.
        INSERT #Problemas
        SELECT N''El BIT historico difiere de las entregas confirmadas'', v.id_venta, v.id_variante, NULL
        FROM #Vendidas v LEFT JOIN #Entregadas e
          ON e.id_venta = v.id_venta AND e.id_variante = v.id_variante
        WHERE v.minimo_historico <> CASE WHEN COALESCE(e.entregadas, 0) >= v.vendidas THEN 1 ELSE 0 END
           OR v.maximo_historico <> CASE WHEN COALESCE(e.entregadas, 0) >= v.vendidas THEN 1 ELSE 0 END;

        IF EXISTS (SELECT 1 FROM #Problemas)
        BEGIN
            SELECT motivo, id_venta, id_variante, id_remito
            FROM #Problemas ORDER BY motivo, id_venta, id_variante, id_remito;
            SELECT v.*, COALESCE(p.preparadas, 0) preparadas, COALESCE(e.entregadas, 0) entregadas
            FROM #Vendidas v LEFT JOIN #Preparadas p
              ON p.id_venta = v.id_venta AND p.id_variante = v.id_variante
            LEFT JOIN #Entregadas e ON e.id_venta = v.id_venta AND e.id_variante = v.id_variante
            WHERE EXISTS (SELECT 1 FROM #Problemas x WHERE x.id_venta = v.id_venta
                          AND (x.id_variante IS NULL OR x.id_variante = v.id_variante));
            THROW 50104, N''Hay inconsistencias. No se elimino la columna ni se corrigieron datos automaticamente.'', 1;
        END;
    ';

    DECLARE @nombreDefault sysname = (
        SELECT name FROM sys.default_constraints
        WHERE parent_object_id = @tabla AND parent_column_id = @columna
    );
    IF @default <> 0 AND @nombreDefault IS NULL
        THROW 50105, N'El DEFAULT es una dependencia no estandar; requiere revision manual.', 1;
    IF @nombreDefault IS NOT NULL
    BEGIN
        DECLARE @sql nvarchar(max) = N'ALTER TABLE dbo.Detalle_Venta DROP CONSTRAINT '
                                    + QUOTENAME(@nombreDefault) + N';';
        EXEC sys.sp_executesql @sql;
    END;

    EXEC sys.sp_executesql N'ALTER TABLE dbo.Detalle_Venta DROP COLUMN entregado;';
    COMMIT;
    PRINT N'Migracion completada. Solo se eliminaron el DEFAULT asociado y Detalle_Venta.entregado.';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK;
    THROW;
END CATCH;
