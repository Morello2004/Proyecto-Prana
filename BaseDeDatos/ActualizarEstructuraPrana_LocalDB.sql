-- actualizacion acumulativa e idempotente se ejecuta sobre la conexion abierta por el inicializador, sin cambiar la base con use incluye altas, adaptaciones y eliminacion de columnas o tablas antiguas sin uso respaldar antes de actualizar comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Usuario',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Medio_Pago',N'U') IS NULL OR OBJECT_ID(N'dbo.Rol',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Cliente',N'U') IS NULL
    -- detiene el flujo con un error explicativo en lugar de continuar con una estructura incompatible
    THROW 50001, N'La estructura base está incompleta. No se puede actualizar automáticamente.', 1;
-- fin del lote el inicializador divide el archivo aqui; go no es una consulta enviada por sqlcommand
GO

-- estado de actividad y baja logica de los clientes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Cliente',N'activo') IS NULL
    -- modifica una tabla existente add incorpora estructura; alter cambia su tipo; drop elimina el objeto indicado
    ALTER TABLE dbo.Cliente ADD activo bit NOT NULL CONSTRAINT DF_Cliente_Activo DEFAULT(1) WITH VALUES;
-- comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Cliente',N'eliminado') IS NULL
    
    ALTER TABLE dbo.Cliente ADD eliminado bit NOT NULL CONSTRAINT DF_Cliente_Eliminado DEFAULT(0) WITH VALUES;

GO

-- recargos por cuotas y baja logica de los medios de pago comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_2') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_2 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo2 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_3') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_3 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo3 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_6') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_6 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo6 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_9') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_9 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo9 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'recargo_12') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD recargo_12 decimal(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo12 DEFAULT(0) WITH VALUES;

GO

IF COL_LENGTH(N'dbo.Medio_Pago',N'activo') IS NULL
    
    ALTER TABLE dbo.Medio_Pago ADD activo bit NOT NULL CONSTRAINT DF_MedioPago_Activo DEFAULT(1) WITH VALUES;

GO

-- agrega los campos necesarios para limitar los intentos de inicio de sesion comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Usuario',N'intentos_login_fallidos') IS NULL
    
    ALTER TABLE dbo.Usuario ADD intentos_login_fallidos int NOT NULL CONSTRAINT DF_Usuario_IntentosLogin DEFAULT(0) WITH VALUES;

IF COL_LENGTH(N'dbo.Usuario',N'bloqueado_hasta') IS NULL
    
    ALTER TABLE dbo.Usuario ADD bloqueado_hasta datetime2(7) NULL;

GO

-- amplia el descuento monetario antiguo sin reducir columnas de mayor capacidad comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta')
    AND name=N'descuento' AND precision<10 AND scale=2 AND TYPE_NAME(system_type_id) IN (N'decimal',N'numeric'))
-- inicio del grupo de instrucciones correspondiente a la condicion o al flujo anterior
BEGIN
    
    IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta') AND name=N'descuento' AND is_nullable=1)
        
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NULL;
    ELSE
        
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NOT NULL;
-- fin del bloque agrupado; las instrucciones siguientes quedan fuera de ese grupo
END;

GO

-- historial: la fecha se registra automaticamente en cada operacion comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.LogActividad',N'U') IS NULL

BEGIN
    -- crea la estructura de la tabla: cada columna define un dato y las restricciones protegen su integridad
    CREATE TABLE dbo.LogActividad (
        id_log int IDENTITY(1,1) NOT NULL CONSTRAINT PK_LogActividad PRIMARY KEY,
        id_usuario int NOT NULL,
        accion nvarchar(100) NOT NULL,
        entidad nvarchar(100) NOT NULL,
        id_entidad int NULL,
        detalle nvarchar(1000) NULL,
        fecha datetime2 NOT NULL CONSTRAINT DF_LogActividad_Fecha DEFAULT(SYSDATETIME()),
        -- da nombre o define una regla de integridad para poder comprobarla o administrarla despues
        CONSTRAINT FK_LogActividad_Usuario FOREIGN KEY(id_usuario) REFERENCES dbo.Usuario(id_usuario)
    );

END;

GO

-- datos de logistica que se guardan junto con una nueva venta comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Venta_Entrega',N'U') IS NULL

BEGIN
    
    CREATE TABLE dbo.Venta_Entrega (
        id_venta int NOT NULL CONSTRAINT PK_Venta_Entrega PRIMARY KEY,
        domicilio nvarchar(200) NULL,
        fecha_estimada datetime2 NULL,
        horario_sugerido nvarchar(50) NULL,
        observaciones nvarchar(500) NULL,
        tipo_logistica nvarchar(50) NULL,
        costo_envio decimal(10,2) NOT NULL,
        observacion_pago nvarchar(500) NULL,
        condicion_pago nvarchar(30) NOT NULL,
        
        CONSTRAINT FK_VentaEntrega_Venta FOREIGN KEY(id_venta) REFERENCES dbo.Venta(id_venta)
    );

END;

GO

-- tipo_envio pasa a ser una columna controlada de envio comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Envio',N'U') IS NOT NULL
    AND COL_LENGTH(N'dbo.Envio',N'tipo_envio') IS NULL
    
    ALTER TABLE dbo.Envio ADD tipo_envio nvarchar(30) NULL;

GO


IF OBJECT_ID(N'dbo.Envio',N'U') IS NOT NULL

BEGIN
    
    IF COL_LENGTH(N'dbo.Envio',N'id_tipo_envio') IS NOT NULL
    
    BEGIN
        
        IF OBJECT_ID(N'dbo.Tipo_Envio',N'U') IS NOT NULL
            -- ejecuta la instruccion o procedimiento indicado el sql dinamico permite adaptar nombres de objetos antiguos
            EXEC(N'UPDATE e SET tipo_envio=t.nombre_tipo_envio
                FROM dbo.Envio e JOIN dbo.Tipo_Envio t ON t.id_tipo_envio=e.id_tipo_envio
                WHERE e.tipo_envio IS NULL');

        
        EXEC(N'UPDATE e SET tipo_envio=CASE WHEN m.nombre_modalidad_entrega LIKE N''Retiro%''
                THEN N''Retiro en local'' ELSE N''Envío a domicilio'' END
            FROM dbo.Envio e JOIN dbo.Remito r ON r.id_remito=e.id_remito
            JOIN dbo.Venta v ON v.id_venta=r.id_venta
            JOIN dbo.Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
            WHERE e.tipo_envio IS NULL');

        
        IF EXISTS(SELECT 1 FROM dbo.Envio WHERE tipo_envio IS NULL)
        
        BEGIN
            ;THROW 50003, N'No se pudo determinar el tipo de algunos envíos. La actualización fue cancelada.', 1;
        
        END;

        -- declara una variable local del lote; no es una columna permanente de una tabla
        DECLARE @quitarFkTipoEnvio nvarchar(max) = N'';
        -- consulta los datos o metadatos indicados; por si sola no guarda cambios en las tablas
        SELECT @quitarFkTipoEnvio = @quitarFkTipoEnvio +
            N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) + N'.' +
            QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
        -- define el origen de los registros utilizados por la consulta
        FROM sys.foreign_keys fk
        -- limita los registros afectados o consultados; es importante comprobarlo antes de ejecutar escrituras
        WHERE fk.referenced_object_id=OBJECT_ID(N'dbo.Tipo_Envio');
        
        EXEC sys.sp_executesql @quitarFkTipoEnvio;

        
        ALTER TABLE dbo.Envio DROP COLUMN id_tipo_envio;
    
    END;

    
    IF EXISTS(SELECT 1 FROM dbo.Envio WHERE tipo_envio IS NULL)
    
    BEGIN
        ;THROW 50004, N'Hay envíos sin tipo asignado. La actualización fue cancelada.', 1;
    
    END;
    
    IF EXISTS(SELECT 1 FROM dbo.Envio
        
        WHERE tipo_envio NOT IN (N'Envío a domicilio',N'Retiro en local'))
    
    BEGIN
        ;THROW 50005, N'Hay tipos de envío distintos de los dos permitidos. Revise esos registros antes de actualizar.', 1;
    
    END;

    
    IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Envio')
        AND name=N'tipo_envio' AND is_nullable=1)
        
        ALTER TABLE dbo.Envio ALTER COLUMN tipo_envio nvarchar(30) NOT NULL;

    
    IF NOT EXISTS(SELECT 1 FROM sys.check_constraints
        
        WHERE parent_object_id=OBJECT_ID(N'dbo.Envio') AND name=N'CK_Envio_TipoEnvio')
        
        ALTER TABLE dbo.Envio ADD CONSTRAINT CK_Envio_TipoEnvio
            CHECK(tipo_envio IN (N'Envío a domicilio',N'Retiro en local'));

END;

GO


IF OBJECT_ID(N'dbo.Tipo_Envio',N'U') IS NOT NULL

BEGIN
    -- tambien limpia referencias residuales si la columna ya se quito en una ejecucion parcial declara una variable local del lote; no es una columna permanente de una tabla
    DECLARE @quitarFkTipoEnvioRestantes nvarchar(max) = N'';
    
    SELECT @quitarFkTipoEnvioRestantes = @quitarFkTipoEnvioRestantes +
        N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) + N'.' +
        QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
    
    FROM sys.foreign_keys fk
    
    WHERE fk.referenced_object_id=OBJECT_ID(N'dbo.Tipo_Envio');
    
    EXEC sys.sp_executesql @quitarFkTipoEnvioRestantes;

    -- elimina la tabla indicada y sus filas este paso requiere respaldo y revision de sus dependencias
    DROP TABLE dbo.Tipo_Envio;

END;

GO

-- el remito se genera siempre; la columna solo almacenaba un indicador redundante comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Venta_Entrega',N'generar_remito') IS NOT NULL
    
    ALTER TABLE dbo.Venta_Entrega DROP COLUMN generar_remito;

GO

-- quita el catalogo de formatos, que no selecciona ni almacena plantillas reales comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Formato_Documento',N'U') IS NOT NULL

BEGIN
    
    DECLARE @quitarFormatoDocumento nvarchar(max) = N'';
    
    SELECT @quitarFormatoDocumento = @quitarFormatoDocumento +
        N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) + N'.' +
        QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
    
    FROM sys.foreign_keys fk
    
    WHERE fk.referenced_object_id = OBJECT_ID(N'dbo.Formato_Documento');
    
    EXEC sys.sp_executesql @quitarFormatoDocumento;

    
    IF COL_LENGTH(N'dbo.Recibo',N'id_formato_documento') IS NOT NULL
        
        ALTER TABLE dbo.Recibo DROP COLUMN id_formato_documento;
    
    IF COL_LENGTH(N'dbo.Remito',N'id_formato_documento') IS NOT NULL
        
        ALTER TABLE dbo.Remito DROP COLUMN id_formato_documento;

    
    DROP TABLE dbo.Formato_Documento;

END;

GO

-- el esquema de devoluciones no tiene uso operativo en la aplicacion comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Devolucion',N'U') IS NOT NULL
    
    DROP TABLE dbo.Devolucion;

IF OBJECT_ID(N'dbo.Tipo_Resolucion',N'U') IS NOT NULL
    
    DROP TABLE dbo.Tipo_Resolucion;

GO

-- elimina el correo sin uso y sus restricciones en instalaciones anteriores busca la restriccion por columna: su nombre cambia entre instalaciones declara una variable local del lote; no es una columna permanente de una tabla
DECLARE @quitarUnicidadEmail nvarchar(max) = N'';

SELECT @quitarUnicidadEmail = @quitarUnicidadEmail +
    N'ALTER TABLE dbo.Usuario DROP CONSTRAINT ' + QUOTENAME(k.name) + N';'

FROM sys.key_constraints k

WHERE k.parent_object_id = OBJECT_ID(N'dbo.Usuario') AND k.type = 'UQ'
  AND (SELECT COUNT(*) FROM sys.index_columns ic
       
       WHERE ic.object_id=k.parent_object_id AND ic.index_id=k.unique_index_id AND ic.key_ordinal>0)=1
  AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
      ON c.object_id=ic.object_id AND c.column_id=ic.column_id
      
      WHERE ic.object_id=k.parent_object_id AND ic.index_id=k.unique_index_id
        AND ic.key_ordinal=1 AND c.name=N'email');

EXEC sys.sp_executesql @quitarUnicidadEmail;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Usuario') AND name=N'UX_Usuario_EmailInformado')
    -- retira el indice indicado, sin borrar por si mismo las filas de la tabla
    DROP INDEX UX_Usuario_EmailInformado ON dbo.Usuario;

IF COL_LENGTH(N'dbo.Usuario', N'email') IS NOT NULL
    
    ALTER TABLE dbo.Usuario DROP COLUMN email;

IF COL_LENGTH(N'dbo.Cliente', N'email') IS NOT NULL
    
    ALTER TABLE dbo.Cliente DROP COLUMN email;

IF COL_LENGTH(N'dbo.Proveedor', N'email') IS NOT NULL
    
    ALTER TABLE dbo.Proveedor DROP COLUMN email;

GO

-- el codigo de permisos usa el identificador 2 para vendedor; no altera roles existentes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF NOT EXISTS(SELECT 1 FROM dbo.Rol WHERE id_rol_usuario=2)

BEGIN
    -- configura una opcion de la sesion sql; su alcance y valor se indican en la instruccion
    SET IDENTITY_INSERT dbo.Rol ON;
    -- incorpora filas; revisar las columnas y el filtro de existencia para no duplicar configuracion o historial
    INSERT INTO dbo.Rol(id_rol_usuario,nombre_rol_usuario) VALUES(2,N'Vendedor');
    
    SET IDENTITY_INSERT dbo.Rol OFF;

END;

GO

-- unifica la condicion inicial de las ventas anteriores sin modificar sus importes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF OBJECT_ID(N'dbo.Venta_Entrega', N'U') IS NOT NULL
    
    EXEC(N'UPDATE dbo.Venta_Entrega SET condicion_pago=N''Pago parcial'' WHERE condicion_pago=N''Seña para reserva''');

GO

-- convierte el codigo postal numerico de localidad a texto sin cambiar sus valores comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF EXISTS (
    
    SELECT 1
    
    FROM sys.columns
    
    WHERE object_id = OBJECT_ID(N'dbo.Localidad')
      AND name = N'codigo_postal'
      AND TYPE_NAME(system_type_id) = N'int'
)

BEGIN
    
    ALTER TABLE dbo.Localidad ALTER COLUMN codigo_postal nvarchar(12) NULL;

END;

GO

-- unifica las condiciones reconocidas sin modificar otras condiciones existentes modifica las filas que cumplan el filtro; no agrega una nueva operacion por si sola
UPDATE dbo.Variante_Producto

SET condicion_producto = UPPER(LTRIM(RTRIM(condicion_producto)))

WHERE UPPER(LTRIM(RTRIM(condicion_producto))) IN (N'PRIMERA', N'OUTLET')
  AND condicion_producto COLLATE Latin1_General_100_BIN2
      <> UPPER(LTRIM(RTRIM(condicion_producto))) COLLATE Latin1_General_100_BIN2;

GO

-- iguala el costo de variante al precio sin recortar valores existentes comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF EXISTS (
    
    SELECT 1 FROM sys.columns
    
    WHERE object_id = OBJECT_ID(N'dbo.Variante_Producto')
      AND name = N'costo'
      AND TYPE_NAME(system_type_id) IN (N'decimal', N'numeric')
      AND precision = 18 AND scale = 2
)

BEGIN
    -- conserva el bloqueo hasta que termine la transaccion del inicializador comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
    IF EXISTS (
        
        SELECT 1 FROM dbo.Variante_Producto WITH (TABLOCKX, HOLDLOCK)
        
        WHERE costo > 99999999.99 OR costo < -99999999.99
    )
        
        THROW 50005, N'No se puede convertir el costo a decimal(10,2): existen valores fuera del rango permitido.', 1;

    
    ALTER TABLE dbo.Variante_Producto ALTER COLUMN costo decimal(10,2) NOT NULL;

END;

GO

-- conserva los documentos existentes y los identifica inicialmente como dni comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF COL_LENGTH(N'dbo.Cliente', N'tipo_documento') IS NULL

BEGIN
    
    ALTER TABLE dbo.Cliente ADD tipo_documento nvarchar(4) NOT NULL
        
        CONSTRAINT DF_Cliente_TipoDocumento DEFAULT (N'DNI') WITH VALUES;

END;

GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.Cliente') AND name = N'CK_Cliente_TipoDocumento')
    
    ALTER TABLE dbo.Cliente WITH CHECK ADD CONSTRAINT CK_Cliente_TipoDocumento CHECK (tipo_documento IN (N'DNI', N'CUIT'));

GO
-- reserva el nombre para siempre, incluso cuando la cuenta esta dada de baja comprueba el estado actual antes de aplicar el bloque; evita repetir cambios o valida un requisito
IF NOT EXISTS (SELECT 1 FROM sys.indexes
    
    WHERE object_id=OBJECT_ID(N'dbo.Usuario') AND name=N'UX_Usuario_Nombre')
AND NOT EXISTS (SELECT nombre_usuario FROM dbo.Usuario GROUP BY nombre_usuario HAVING COUNT(*) > 1)

BEGIN
    -- las bases antiguas con duplicados pueden iniciar sin cambiar cuentas existentes crearusuario y modificarusuario validan igualmente todos los nombres crea un indice que rechaza claves repetidas; no crea otra tabla de datos
    CREATE UNIQUE INDEX UX_Usuario_Nombre ON dbo.Usuario(nombre_usuario);

END;

GO
