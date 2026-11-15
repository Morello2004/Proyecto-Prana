-- agrega los horarios propios de cada apertura sin cambiar los identificadores de cajas anteriores
IF COL_LENGTH(N'dbo.Caja',N'inicio') IS NULL
    ALTER TABLE dbo.Caja ADD inicio datetime2 NULL;
IF COL_LENGTH(N'dbo.Caja',N'fin') IS NULL
    ALTER TABLE dbo.Caja ADD fin datetime2 NULL;
IF COL_LENGTH(N'dbo.Caja',N'id_usuario_apertura') IS NULL
    ALTER TABLE dbo.Caja ADD id_usuario_apertura int NULL REFERENCES dbo.Usuario(id_usuario);
IF COL_LENGTH(N'dbo.Pago',N'id_caja') IS NULL
    ALTER TABLE dbo.Pago ADD id_caja int NULL;
IF COL_LENGTH(N'dbo.Caja_Movimiento',N'id_caja') IS NULL
    ALTER TABLE dbo.Caja_Movimiento ADD id_caja int NULL;
GO

-- las operaciones anteriores se relacionan por fecha una sola vez antes de habilitar varios turnos diarios
-- si no habia fila de caja para un dia con operaciones la crea para conservar esas operaciones
INSERT dbo.Caja(fecha,estado,datos)
SELECT dias.fecha,N'Abierta',
    (SELECT dias.fecha AS Fecha,CONVERT(datetime2,dias.fecha) AS Inicio,0 AS SaldoInicial,0 AS SaldoInicialUsd FOR JSON PATH,WITHOUT_ARRAY_WRAPPER)
FROM (SELECT CONVERT(date,fecha_pago) fecha FROM dbo.Pago WHERE id_caja IS NULL
      UNION SELECT CONVERT(date,fecha) FROM dbo.Caja_Movimiento WHERE id_caja IS NULL) dias
WHERE NOT EXISTS(SELECT 1 FROM dbo.Caja c WHERE c.fecha=dias.fecha);

-- rechaza asignaciones ambiguas en lugar de repartir movimientos antiguos entre turnos arbitrarios
IF EXISTS(SELECT 1 FROM dbo.Pago p WHERE p.id_caja IS NULL AND
    (SELECT COUNT(*) FROM dbo.Caja c WHERE c.fecha=CONVERT(date,p.fecha_pago))<>1)
    THROW 50030,N'No se pudo identificar la caja de un pago anterior.',1;
IF EXISTS(SELECT 1 FROM dbo.Caja_Movimiento m WHERE m.id_caja IS NULL AND
    (SELECT COUNT(*) FROM dbo.Caja c WHERE c.fecha=CONVERT(date,m.fecha))<>1)
    THROW 50031,N'No se pudo identificar la caja de un movimiento anterior.',1;
UPDATE p SET id_caja=c.id FROM dbo.Pago p JOIN dbo.Caja c ON c.fecha=CONVERT(date,p.fecha_pago) WHERE p.id_caja IS NULL;
UPDATE m SET id_caja=c.id FROM dbo.Caja_Movimiento m JOIN dbo.Caja c ON c.fecha=CONVERT(date,m.fecha) WHERE m.id_caja IS NULL;

-- recupera horarios historicos desde el json y usa la fecha cuando una version anterior no los guardaba
UPDATE dbo.Caja SET inicio=COALESCE(TRY_CONVERT(datetime2,JSON_VALUE(datos,'$.Inicio'),126),CONVERT(datetime2,fecha)) WHERE inicio IS NULL;
UPDATE dbo.Caja SET fin=COALESCE(TRY_CONVERT(datetime2,JSON_VALUE(datos,'$.Fin'),126),fecha_cierre,DATEADD(day,1,CONVERT(datetime2,fecha)))
    WHERE estado=N'Cerrada' AND fin IS NULL;
UPDATE dbo.Caja SET fin=COALESCE(fecha_cierre,inicio) WHERE estado=N'Cerrada' AND fin<inicio;
UPDATE dbo.Caja SET fin=inicio WHERE estado=N'Cerrada' AND fin<inicio;
GO

-- completa cajas abiertas de dias anteriores para poder exigir una sola apertura vigente en toda la aplicacion
-- reconstruye sus movimientos con la base de pago sin recargo y excluye las anulaciones manuales
UPDATE c SET estado=N'Cerrada',id_usuario=NULL,fecha_cierre=SYSDATETIME(),
    fin=DATEADD(day,1,CONVERT(datetime2,c.fecha)),
    datos=JSON_MODIFY(JSON_MODIFY(JSON_MODIFY(JSON_MODIFY(c.datos,'$.Cerrada',CAST(1 AS bit)),
        '$.Fin',CONVERT(nvarchar(33),DATEADD(day,1,CONVERT(datetime2,c.fecha)),126)),
        '$.Usuario',N'CIERRE AUTOMÁTICO'),'$.Movimientos',JSON_QUERY((
            SELECT mov.* FROM (
                SELECT p.id_pago Id,p.id_venta Venta,p.fecha_pago Fecha,
                    CONCAT(cl.nombre_cliente,' ',cl.apellido_cliente,' - ',mp.nombre_medio_pago) Concepto,
                    N'Cobro venta' Tipo,COALESCE(p.tipo_caja,mp.tipo_caja) Medio,N'ARS' Moneda,
                    p.monto Importe,p.cuenta_destino Cuenta,CONVERT(nvarchar(20),p.id_usuario) Usuario
                FROM dbo.Pago p JOIN dbo.Venta v ON v.id_venta=p.id_venta JOIN dbo.Cliente cl ON cl.id_cliente=v.id_cliente
                JOIN dbo.Medio_Pago mp ON mp.id_medio_pago=p.id_medio_pago WHERE p.id_caja=c.id
                UNION ALL
                SELECT m.id,NULL,m.fecha,m.concepto,m.tipo,m.medio,m.moneda,m.importe,m.cuenta,CONVERT(nvarchar(20),m.id_usuario)
                FROM dbo.Caja_Movimiento m WHERE m.id_caja=c.id AND m.anulado=0
            ) mov ORDER BY mov.Fecha,mov.Id FOR JSON PATH)))
FROM dbo.Caja c WHERE c.estado=N'Abierta' AND c.fecha<CONVERT(date,SYSDATETIME());
GO

-- toda operacion queda asociada a una caja real incluso cuando hay varios cierres en la misma fecha
IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Caja') AND name=N'inicio' AND is_nullable=1)
    ALTER TABLE dbo.Caja ALTER COLUMN inicio datetime2 NOT NULL;
IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Pago') AND name=N'id_caja' AND is_nullable=1)
    ALTER TABLE dbo.Pago ALTER COLUMN id_caja int NOT NULL;
IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.Caja_Movimiento') AND name=N'id_caja' AND is_nullable=1)
    ALTER TABLE dbo.Caja_Movimiento ALTER COLUMN id_caja int NOT NULL;
IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_Pago_Caja')
    ALTER TABLE dbo.Pago ADD CONSTRAINT FK_Pago_Caja FOREIGN KEY(id_caja) REFERENCES dbo.Caja(id);
IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_CajaMovimiento_Caja')
    ALTER TABLE dbo.Caja_Movimiento ADD CONSTRAINT FK_CajaMovimiento_Caja FOREIGN KEY(id_caja) REFERENCES dbo.Caja(id);
IF NOT EXISTS(SELECT 1 FROM sys.check_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Caja') AND name=N'CK_Caja_FinTurno')
    ALTER TABLE dbo.Caja ADD CONSTRAINT CK_Caja_FinTurno CHECK(
        (estado=N'Abierta' AND fin IS NULL) OR (estado=N'Cerrada' AND fin IS NOT NULL));

-- permite repetir fechas pero impide dos cajas abiertas al mismo tiempo incluso desde dos computadoras
IF EXISTS(SELECT 1 FROM sys.key_constraints WHERE parent_object_id=OBJECT_ID(N'dbo.Caja') AND name=N'UQ_Caja_Fecha')
    ALTER TABLE dbo.Caja DROP CONSTRAINT UQ_Caja_Fecha;
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Caja') AND name=N'UX_Caja_UnicaAbierta')
    CREATE UNIQUE INDEX UX_Caja_UnicaAbierta ON dbo.Caja(estado) WHERE estado=N'Abierta';
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Pago') AND name=N'IX_Pago_Caja')
    CREATE INDEX IX_Pago_Caja ON dbo.Pago(id_caja,fecha_pago);
IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE object_id=OBJECT_ID(N'dbo.Caja_Movimiento') AND name=N'IX_CajaMovimiento_Caja')
    CREATE INDEX IX_CajaMovimiento_Caja ON dbo.Caja_Movimiento(id_caja,fecha);
GO
