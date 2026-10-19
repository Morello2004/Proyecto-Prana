-- Migración idempotente, aplicada en la transacción del inicializador.
-- Nunca renumera ventas existentes ni retrocede una numeración superior.
IF COALESCE(IDENT_CURRENT(N'dbo.Venta'),0) < 2500 AND NOT EXISTS(SELECT 1 FROM dbo.Venta WHERE id_venta >= 2500)
BEGIN
    IF EXISTS(SELECT 1 FROM sys.identity_columns WHERE object_id=OBJECT_ID(N'dbo.Venta') AND last_value IS NULL)
        DBCC CHECKIDENT ('dbo.Venta', RESEED, 2500) WITH NO_INFOMSGS;
    ELSE
        DBCC CHECKIDENT ('dbo.Venta', RESEED, 2499) WITH NO_INFOMSGS;
END;
GO
IF COL_LENGTH(N'dbo.Medio_Pago',N'tipo_caja') IS NULL
BEGIN
    ALTER TABLE dbo.Medio_Pago ADD tipo_caja nvarchar(20) NOT NULL CONSTRAINT DF_Medio_TipoCaja DEFAULT(N'Otros');
    EXEC(N'UPDATE dbo.Medio_Pago SET tipo_caja=CASE
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%efectivo%'' THEN N''Efectivo''
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%transf%'' THEN N''Transferencia''
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%debito%'' THEN N''Débito''
        WHEN nombre_medio_pago COLLATE Latin1_General_CI_AI LIKE ''%credito%'' OR nombre_medio_pago LIKE ''%Tarjeta%'' THEN N''Crédito'' ELSE N''Otros'' END');
END;
GO
IF COL_LENGTH(N'dbo.Pago',N'tipo_caja') IS NULL
BEGIN
    ALTER TABLE dbo.Pago ADD tipo_caja nvarchar(20) NULL;
    EXEC(N'UPDATE p SET tipo_caja=m.tipo_caja FROM dbo.Pago p JOIN dbo.Medio_Pago m ON m.id_medio_pago=p.id_medio_pago');
END;
GO
IF OBJECT_ID(N'dbo.Caja_Movimiento',N'U') IS NULL
    CREATE TABLE dbo.Caja_Movimiento (
        id int IDENTITY PRIMARY KEY, fecha datetime2 NOT NULL DEFAULT(SYSDATETIME()),
        concepto nvarchar(200) NOT NULL, tipo nvarchar(20) NOT NULL CHECK(tipo IN(N'Gasto',N'Ingreso extra',N'Café')),
        medio nvarchar(20) NOT NULL, moneda char(3) NOT NULL CHECK(moneda IN('ARS','USD')),
        importe decimal(18,2) NOT NULL CHECK(importe>0), cuenta nvarchar(100) NOT NULL,
        id_usuario int NOT NULL REFERENCES dbo.Usuario(id_usuario), anulado bit NOT NULL DEFAULT(0),
        motivo_anulacion nvarchar(200) NULL);
GO
IF COL_LENGTH(N'dbo.Pago',N'cuenta_destino') IS NULL
    ALTER TABLE dbo.Pago ADD cuenta_destino nvarchar(100) NOT NULL CONSTRAINT DF_Pago_Cuenta DEFAULT(N'');
GO
IF OBJECT_ID(N'dbo.Caja_Cierre',N'U') IS NULL
    CREATE TABLE dbo.Caja_Cierre (
        id int IDENTITY PRIMARY KEY, fecha date NOT NULL UNIQUE,
        id_usuario int NOT NULL REFERENCES dbo.Usuario(id_usuario),
        fecha_cierre datetime2 NOT NULL DEFAULT(SYSDATETIME()), datos nvarchar(max) NOT NULL CHECK(ISJSON(datos)=1));
GO
-- Los comprobantes se construyen desde las tablas de la venta.
-- Retira la copia JSON de instalaciones anteriores; no elimina ventas ni pagos.
IF OBJECT_ID(N'dbo.Venta_Comprobante',N'U') IS NOT NULL
    DROP TABLE dbo.Venta_Comprobante;
GO

IF COL_LENGTH('dbo.Cliente','codigo_postal') IS NULL
BEGIN
    ALTER TABLE dbo.Cliente ADD codigo_postal nvarchar(12) NULL;
    EXEC(N'UPDATE c SET codigo_postal=CONVERT(nvarchar(12),l.codigo_postal) FROM Cliente c JOIN Localidad l ON l.id_localidad=c.id_localidad');
END;
GO

-- Recupera ventas anteriores con su fecha y vendedor originales, sin duplicarlas.
INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle,fecha)
SELECT v.id_usuario,N'REGISTRAR_VENTA',N'Venta',v.id_venta,
 CONCAT(N'Venta / RTO ',v.id_venta,N' | Total ARS: ',FORMAT(v.total,'N2','es-AR'),
 N' | Descuento ARS: ',FORMAT(COALESCE(v.descuento,0),'N2','es-AR'),N' | Recuperada del registro de ventas'),v.fecha_venta
FROM Venta v WHERE NOT EXISTS(SELECT 1 FROM LogActividad l WHERE l.entidad=N'Venta' AND l.id_entidad=v.id_venta AND l.accion=N'REGISTRAR_VENTA');
INSERT LogActividad(id_usuario,accion,entidad,id_entidad,detalle,fecha)
SELECT v.id_usuario,N'APLICAR_DESCUENTO',N'Venta',v.id_venta,
 CONCAT(N'Venta / RTO ',v.id_venta,N' | Descuento ARS: ',FORMAT(v.descuento,'N2','es-AR'),
 N' | Total final ARS: ',FORMAT(v.total,'N2','es-AR'),N' | Recuperado del registro de ventas'),v.fecha_venta
FROM Venta v WHERE v.descuento>0 AND NOT EXISTS(SELECT 1 FROM LogActividad l WHERE l.entidad=N'Venta' AND l.id_entidad=v.id_venta AND l.accion=N'APLICAR_DESCUENTO');
GO

-- Corrige los cuatro medios fijos sin alterar sus recargos generales ni los de credito
UPDATE mp
SET tipo_caja = medios.tipo_caja,
    activo = 1,
    recargo_2 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_2 ELSE 0 END,
    recargo_3 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_3 ELSE 0 END,
    recargo_6 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_6 ELSE 0 END,
    recargo_9 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_9 ELSE 0 END,
    recargo_12 = CASE WHEN medios.tipo_caja = N'Crédito' THEN mp.recargo_12 ELSE 0 END
FROM dbo.Medio_Pago mp
INNER JOIN (VALUES
    (N'Efectivo', N'Efectivo'),
    (N'Transferencia', N'Transferencia'),
    (N'Débito', N'Débito'),
    (N'Crédito', N'Crédito')
) AS medios(nombre, tipo_caja)
    ON mp.nombre_medio_pago COLLATE Latin1_General_100_BIN2 = medios.nombre COLLATE Latin1_General_100_BIN2;
GO

-- Crea los medios fijos que todavia no existan
INSERT INTO dbo.Medio_Pago
    (nombre_medio_pago, recargo_medio_pago, recargo_2, recargo_3,
     recargo_6, recargo_9, recargo_12, activo, tipo_caja)
SELECT medios.nombre, 0, 0, 0, 0, 0, 0, 1, medios.tipo_caja
FROM (VALUES
    (N'Efectivo', N'Efectivo'),
    (N'Transferencia', N'Transferencia'),
    (N'Débito', N'Débito'),
    (N'Crédito', N'Crédito')
) AS medios(nombre, tipo_caja)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.Medio_Pago mp
    WHERE mp.nombre_medio_pago COLLATE Latin1_General_100_BIN2 = medios.nombre COLLATE Latin1_General_100_BIN2
);
GO