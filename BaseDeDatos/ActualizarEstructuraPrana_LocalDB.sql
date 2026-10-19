/* Actualización acumulativa e idempotente.
   Se ejecuta sobre la conexión abierta por el inicializador, sin USE ni borrados.
   Las columnas existentes y los datos de trabajo se conservan.
*/
IF OBJECT_ID(N'dbo.Usuario',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Medio_Pago',N'U') IS NULL OR OBJECT_ID(N'dbo.Rol',N'U') IS NULL
    THROW 50001, N'La estructura base está incompleta. No se puede actualizar automáticamente.', 1;
GO

-- Recargos por cuotas y baja lógica de los medios de pago.
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

-- Agrega los campos necesarios para limitar los intentos de inicio de sesion.
IF COL_LENGTH(N'dbo.Usuario',N'intentos_login_fallidos') IS NULL
    ALTER TABLE dbo.Usuario ADD intentos_login_fallidos int NOT NULL CONSTRAINT DF_Usuario_IntentosLogin DEFAULT(0) WITH VALUES;
IF COL_LENGTH(N'dbo.Usuario',N'bloqueado_hasta') IS NULL
    ALTER TABLE dbo.Usuario ADD bloqueado_hasta datetime2(7) NULL;
GO

-- Guarda los lotes generados desde la herramienta de pruebas para poder retirarlos.
IF OBJECT_ID(N'dbo.LoteDatosPrueba',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LoteDatosPrueba (
        id_lote uniqueidentifier NOT NULL CONSTRAINT PK_LoteDatosPrueba PRIMARY KEY,
        token nvarchar(50) NOT NULL CONSTRAINT UQ_LoteDatosPrueba_Token UNIQUE,
        creado_por int NOT NULL,
        fecha_creacion datetime2(7) NOT NULL CONSTRAINT DF_LoteDatosPrueba_Fecha DEFAULT(SYSDATETIME()),
        activo bit NOT NULL CONSTRAINT DF_LoteDatosPrueba_Activo DEFAULT(1),
        CONSTRAINT FK_LoteDatosPrueba_Usuario FOREIGN KEY(creado_por) REFERENCES dbo.Usuario(id_usuario)
    );
END;
IF OBJECT_ID(N'dbo.RegistroDatosPrueba',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RegistroDatosPrueba (
        id_lote uniqueidentifier NOT NULL,
        tabla nvarchar(50) NOT NULL,
        id_registro int NOT NULL,
        fecha date NULL,
        CONSTRAINT PK_RegistroDatosPrueba PRIMARY KEY(id_lote, tabla, id_registro),
        CONSTRAINT FK_RegistroDatosPrueba_Lote FOREIGN KEY(id_lote) REFERENCES dbo.LoteDatosPrueba(id_lote)
    );
END;
GO

-- Amplía el descuento monetario antiguo sin reducir columnas de mayor capacidad.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta')
    AND name=N'descuento' AND precision<10 AND scale=2 AND TYPE_NAME(system_type_id) IN (N'decimal',N'numeric'))
BEGIN
    IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Venta') AND name=N'descuento' AND is_nullable=1)
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NULL;
    ELSE
        ALTER TABLE dbo.Venta ALTER COLUMN descuento decimal(10,2) NOT NULL;
END;
GO

-- Historial: la fecha se registra automáticamente en cada operación.
IF OBJECT_ID(N'dbo.LogActividad',N'U') IS NULL
BEGIN
    CREATE TABLE dbo.LogActividad (
        id_log int IDENTITY(1,1) NOT NULL CONSTRAINT PK_LogActividad PRIMARY KEY,
        id_usuario int NOT NULL,
        accion nvarchar(100) NOT NULL,
        entidad nvarchar(100) NOT NULL,
        id_entidad int NULL,
        detalle nvarchar(1000) NULL,
        fecha datetime2 NOT NULL CONSTRAINT DF_LogActividad_Fecha DEFAULT(SYSDATETIME()),
        CONSTRAINT FK_LogActividad_Usuario FOREIGN KEY(id_usuario) REFERENCES dbo.Usuario(id_usuario)
    );
END;
GO

-- Datos de logística que se guardan junto con una nueva venta.
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

-- Tipo_Envio pasa a ser una columna controlada de Envio.
IF OBJECT_ID(N'dbo.Envio',N'U') IS NOT NULL
    AND COL_LENGTH(N'dbo.Envio',N'tipo_envio') IS NULL
    ALTER TABLE dbo.Envio ADD tipo_envio nvarchar(30) NULL;
GO

IF OBJECT_ID(N'dbo.Envio',N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH(N'dbo.Envio',N'id_tipo_envio') IS NOT NULL
    BEGIN
        IF OBJECT_ID(N'dbo.Tipo_Envio',N'U') IS NOT NULL
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

        DECLARE @quitarFkTipoEnvio nvarchar(max) = N'';
        SELECT @quitarFkTipoEnvio = @quitarFkTipoEnvio +
            N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) + N'.' +
            QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
        FROM sys.foreign_keys fk
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
    -- También limpia referencias residuales si la columna ya se quitó en una ejecución parcial.
    DECLARE @quitarFkTipoEnvioRestantes nvarchar(max) = N'';
    SELECT @quitarFkTipoEnvioRestantes = @quitarFkTipoEnvioRestantes +
        N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(fk.parent_object_id)) + N'.' +
        QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';'
    FROM sys.foreign_keys fk
    WHERE fk.referenced_object_id=OBJECT_ID(N'dbo.Tipo_Envio');
    EXEC sys.sp_executesql @quitarFkTipoEnvioRestantes;

    DROP TABLE dbo.Tipo_Envio;
END;
GO

-- El remito se genera siempre; la columna solo almacenaba un indicador redundante.
IF COL_LENGTH(N'dbo.Venta_Entrega',N'generar_remito') IS NOT NULL
    ALTER TABLE dbo.Venta_Entrega DROP COLUMN generar_remito;
GO

-- Quita el catálogo de formatos, que no selecciona ni almacena plantillas reales.
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

-- El esquema de devoluciones no tiene uso operativo en la aplicación.
IF OBJECT_ID(N'dbo.Devolucion',N'U') IS NOT NULL
    DROP TABLE dbo.Devolucion;
IF OBJECT_ID(N'dbo.Tipo_Resolucion',N'U') IS NOT NULL
    DROP TABLE dbo.Tipo_Resolucion;
GO

-- Permite varios usuarios sin correo, conservando la unicidad de los correos informados.
-- Busca la restricción por columna: su nombre cambia entre instalaciones.
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
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Usuario') AND name=N'UX_Usuario_EmailInformado')
    CREATE UNIQUE INDEX UX_Usuario_EmailInformado ON dbo.Usuario(email) WHERE email IS NOT NULL;
GO

-- El código de permisos usa el identificador 2 para Vendedor; no altera roles existentes.
IF NOT EXISTS(SELECT 1 FROM dbo.Rol WHERE id_rol_usuario=2)
BEGIN
    SET IDENTITY_INSERT dbo.Rol ON;
    INSERT INTO dbo.Rol(id_rol_usuario,nombre_rol_usuario) VALUES(2,N'Vendedor');
    SET IDENTITY_INSERT dbo.Rol OFF;
END;
GO

-- Unifica la condición inicial de las ventas anteriores sin modificar sus importes.
IF OBJECT_ID(N'dbo.Venta_Entrega', N'U') IS NOT NULL
    EXEC(N'UPDATE dbo.Venta_Entrega SET condicion_pago=N''Pago parcial'' WHERE condicion_pago=N''Seña para reserva''');
GO