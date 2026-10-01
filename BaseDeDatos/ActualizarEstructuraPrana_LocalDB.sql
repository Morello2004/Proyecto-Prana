/* Actualización acumulativa e idempotente.
   Se ejecuta sobre la conexión abierta por el inicializador, sin USE ni borrados.
   Las columnas existentes y los datos de trabajo se conservan.
*/
IF OBJECT_ID(N'dbo.Usuario',N'U') IS NULL OR OBJECT_ID(N'dbo.Venta',N'U') IS NULL
    OR OBJECT_ID(N'dbo.Medio_Pago',N'U') IS NULL OR OBJECT_ID(N'dbo.Rol',N'U') IS NULL
    THROW 50001, N'La estructura base está incompleta. No se puede actualizar automáticamente.', 1;
GO

-- Recargos por cuotas y baja lógica de los medios de pago.
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
        generar_remito bit NOT NULL,
        CONSTRAINT FK_VentaEntrega_Venta FOREIGN KEY(id_venta) REFERENCES dbo.Venta(id_venta)
    );
END;
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
