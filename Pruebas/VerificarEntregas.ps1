# Ejecutar desde cualquier carpeta. Solo crea tablas temporales en una conexion a master.
# Lee las consultas reales del codigo. Nunca ejecuta la migracion ni abre PranaElectronica.
$ErrorActionPreference = 'Stop'
$raizPrana = Split-Path $PSScriptRoot -Parent
$ventaFuente = [IO.File]::ReadAllText((Join-Path $raizPrana 'Prana.Datos/CD_Venta.cs'))
$inicioEstado = $ventaFuente.IndexOf('                OUTER APPLY (')
$finEstado = $ventaFuente.IndexOf('                WHERE 1 = 1', $inicioEstado)
if ($inicioEstado -lt 0 -or $finEstado -lt 0) { throw 'No se encontro el calculo del estado.' }
$estado = $ventaFuente.Substring($inicioEstado, $finEstado - $inicioEstado)
$clienteFuente = [IO.File]::ReadAllText((Join-Path $raizPrana 'Prana.Datos/CD_Cliente.cs'))
$baja = [regex]::Match($clienteFuente.Substring($clienteFuente.IndexOf('AS tiene_deuda,')),
    '(?s)SELECT 1\s+FROM Venta v.*?(?=\s+\) THEN 1 ELSE 0 END AS tiene_pedidos_pendientes)').Value
if (!$baja) { throw 'No se encontro la validacion de baja.' }
$baja = $baja.Replace('@cliente', 'x.id_venta')
foreach ($tabla in @('Detalle_Venta', 'Detalle_Remito', 'Estado_Venta', 'Remito', 'Envio', 'Venta')) {
    $estado = [regex]::Replace($estado, "\b$tabla\b", "#$tabla")
    $baja = [regex]::Replace($baja, "\b$tabla\b", "#$tabla")
}
$sql = @'
SET NOCOUNT ON;
SET XACT_ABORT ON;
CREATE TABLE #Estado_Venta(id_estado_venta int, nombre_estado_venta nvarchar(30));
INSERT #Estado_Venta VALUES (1,N'Pagada'),(2,N'Cancelada'),(3,N'Anulada');
CREATE TABLE #Venta(id_venta int, id_cliente int, id_estado_venta int, nombre_estado_venta nvarchar(30));
CREATE TABLE #Detalle_Venta(id_venta int, id_variante int, cantidad int);
CREATE TABLE #Remito(id_remito int, id_venta int);
CREATE TABLE #Detalle_Remito(id_remito int, id_variante int, cantidad int);
CREATE TABLE #Envio(id_envio int, id_remito int, entregado bit);
CREATE TABLE #Esperado(id_venta int, estado nvarchar(30), pendiente int);
INSERT #Esperado VALUES
 (1,N'Pendiente',1), (2,N'Entrega parcial',1), (3,N'Entrega parcial',1),
 (4,N'Entregada',0), (5,N'Entregada',0), (6,N'Entregada',0),
 (7,N'Entrega parcial',1), (8,N'Cancelada',0), (9,N'Cancelada',0),
 (10,N'Preparada',1), (11,N'Preparación parcial',1), (12,N'Entrega parcial',1);
INSERT #Venta SELECT x.id_venta,x.id_venta,e.id_estado_venta,e.nombre_estado_venta
 FROM #Esperado x JOIN #Estado_Venta e ON e.id_estado_venta = CASE x.id_venta WHEN 8 THEN 2 WHEN 9 THEN 3 ELSE 1 END;
INSERT #Detalle_Venta VALUES
 (1,1,3),(2,1,3),(3,1,3),(4,1,3),(5,1,2),(5,2,1),
 (6,1,2),(6,1,1),(7,1,3),(8,1,3),(9,1,3),(10,1,3),(11,1,3),(12,1,1),(12,2,2);
INSERT #Remito VALUES (21,2),(31,3),(32,3),(41,4),(42,4),(51,5),(61,6),(62,6),(71,7),(101,10),(111,11),(121,12);
INSERT #Detalle_Remito VALUES
 (21,1,1),(31,1,1),(32,1,2),(41,1,1),(42,1,2),(51,1,2),(51,2,1),
 (61,1,1),(62,1,2),(71,1,1),(101,1,3),(111,1,1),(121,1,3);
INSERT #Envio VALUES
 (21,21,1),(31,31,1),(32,32,0),(41,41,1),(42,42,1),(51,51,1),
 (61,61,1),(62,62,1),(71,71,1),(72,71,1),(73,71,0),(101,101,0),(111,111,0),(121,121,1);
SELECT x.id_venta, entrega.estado_entrega estado,
 CASE WHEN EXISTS (/*BAJA*/) THEN 1 ELSE 0 END pendiente
INTO #Resultado
FROM #Venta v JOIN #Esperado x ON x.id_venta=v.id_venta
/*ESTADO*/;
IF EXISTS (SELECT id_venta,estado,pendiente FROM #Esperado EXCEPT SELECT id_venta,estado,pendiente FROM #Resultado)
 OR EXISTS (SELECT id_venta,estado,pendiente FROM #Resultado EXCEPT SELECT id_venta,estado,pendiente FROM #Esperado)
BEGIN
 SELECT x.*,r.estado estado_real,r.pendiente pendiente_real FROM #Esperado x LEFT JOIN #Resultado r ON r.id_venta=x.id_venta;
 THROW 50200, N'Fallo en los calculos de entregas o baja de clientes.',1;
END;
-- La misma condicion de confirmacion debe afectar una fila una unica vez.
UPDATE #Envio SET entregado=1 WHERE id_envio=32 AND entregado=0;
IF @@ROWCOUNT <> 1 THROW 50201,N'No se confirmo el envio pendiente.',1;
UPDATE #Envio SET entregado=1 WHERE id_envio=32 AND entregado=0;
IF @@ROWCOUNT <> 0 THROW 50202,N'Se pudo confirmar dos veces el mismo envio.',1;
-- Verificar las consultas reales nuevamente luego de confirmar las dos unidades restantes.
DELETE #Resultado;
INSERT #Resultado
SELECT x.id_venta, entrega.estado_entrega,
 CASE WHEN EXISTS (/*BAJA*/) THEN 1 ELSE 0 END
FROM #Venta v JOIN #Esperado x ON x.id_venta=v.id_venta
/*ESTADO*/;
IF NOT EXISTS(SELECT 1 FROM #Resultado WHERE id_venta=3 AND estado=N'Entregada' AND pendiente=0)
 THROW 50203,N'La confirmacion final no completo las tres unidades.',1;
PRINT N'OK: 12 escenarios de estado/baja, confirmacion unica y entrega parcial 1 + 2.';
'@
$sql = $sql.Replace('/*BAJA*/', $baja).Replace('/*ESTADO*/', $estado)
$archivoTemporal = Join-Path ([IO.Path]::GetTempPath()) ('Prana-Pruebas-Entregas-' + [guid]::NewGuid().ToString('N') + '.sql')
try {
    [IO.File]::WriteAllText($archivoTemporal, $sql, [Text.UTF8Encoding]::new($true))
    & sqlcmd -S '(localdb)\MSSQLLocalDB' -d master -E -C -b -l 15 -i $archivoTemporal
    if ($LASTEXITCODE -ne 0) { throw 'Fallo la verificacion SQL de entregas.' }
}
finally { Remove-Item -LiteralPath $archivoTemporal -ErrorAction SilentlyContinue }
