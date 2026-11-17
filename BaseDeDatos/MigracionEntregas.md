# Eliminacion de Detalle_Venta.entregado — Prana 1.0.9

La aplicacion ya no lee ni escribe esta columna. La unica marca de confirmacion
es `Envio.entregado`; las cantidades salen de `Detalle_Remito` y se comparan con
la suma de `Detalle_Venta` por venta y variante. La deuda del cliente se sigue
comprobando por separado. Las ventas anuladas/canceladas no impiden su baja.

## Actualizacion manual de una instalacion existente

1. Cerrar Prana en todos los puestos y guardar un respaldo completo verificado
   de `PranaElectronica`. Conservar tambien el instalador anterior.
2. Instalar la version 1.0.9, **sin seleccionar Abrir Prana al finalizar**.
   No usar ninguna version del programa hasta terminar la migracion: la nueva
   version deja de mantener el BIT antiguo y podria generar diferencias si se
   usa antes de eliminarlo.
3. Abrir SQL Server Management Studio. Conectar a `(localdb)\MSSQLLocalDB`
   con autenticacion de Windows, usando el mismo usuario de Windows que utiliza
   Prana. Verificar que alli este la base `PranaElectronica` utilizada por el sistema.
4. Abrir **EliminarDetalleVentaEntregado.sql** mediante Archivo > Abrir > Archivo.
   El script selecciona explicitamente `PranaElectronica` mediante `USE`.
5. Ejecutar **todo el archivo**, sin seleccionar fragmentos, mediante F5.
6. Si aparecen inconsistencias o dependencias, guardar los resultados y mensajes
   para revisarlos. El script revierte la transaccion y no elimina la columna ni
   corrige datos. No omitir las verificaciones ni forzar la eliminacion.
7. Si aparece `Migracion completada`, abrir Prana 1.0.9 y comprobar el listado de
   ventas, entregas y la baja de un cliente de prueba sin deuda ni pendientes.
   Una segunda ejecucion del script informa que la columna ya no existe.

No ejecutar `InstalarPrana.sql` como sustituto de esta migracion sobre una base
con datos. La migracion no se ejecuta desde el programa ni desde el instalador.
No volver a usar ejecutables anteriores despues de eliminar la columna.

## Alcance y controles del script

- Usa transaccion, `XACT_ABORT`, manejo de errores, bloqueo de actualizacion y
  bloqueos de tablas durante la comprobacion. Un timeout tambien revierte la operacion.
- Comprueba tablas/columnas y dependencias SQL, indices, estadisticas creadas por
  el usuario, restricciones y claves foraneas. Las referencias de modulos a nivel
  de tabla requieren revision manual, aunque no mencionen explicitamente la columna.
- Detiene la migracion ante remitos sin envio o con multiples envios, registros
  huerfanos, remitos sin detalles, cantidades no positivas, incoherencias entre
  confirmacion y fecha, variantes preparadas no vendidas o cantidades excesivas.
- Compara el minimo y maximo del BIT historico de cada venta/variante con su
  entrega completa calculada. Revisa tambien el historial de ventas anuladas.
- Elimina exclusivamente el DEFAULT de la columna (sin asumir su nombre) y la
  columna. No modifica registros de venta, remitos, envios, pagos ni stock.
- Las referencias externas al repositorio, SQL dinamico de otros sistemas y
  dependencias no registradas por SQL Server deben revisarse antes de actualizar.

## Cambios del proyecto

| Archivo | Cambio |
| --- | --- |
| `Prana.Datos/CD_RegistroVenta.cs` | INSERT de detalles sin el BIT ni su parametro. |
| `Prana.Datos/CD_Entrega.cs` | Retiradas las actualizaciones derivadas del BIT al confirmar y al retirar. |
| `Prana.Datos/CD_Cliente.cs` | Pendientes por cantidades vendidas agrupadas frente a cantidades confirmadas, con EXISTS para no multiplicar envios. |
| `Prana.Datos/CD_Venta.cs` | Consolida los envios por remito antes de sumar, conservando las reglas de estado actuales. |
| `Prana.Datos/InicializadorBaseDatos.cs` | No exige la columna. No ejecuta la migracion. |
| `BaseDeDatos/InstalarPrana.sql` | Instalacion nueva sin columna ni DEFAULT; verificaciones actualizadas. |
| `BaseDeDatos/EliminarDetalleVentaEntregado.sql` | Migracion manual independiente. |
| `Pruebas/VerificarEntregas.ps1` | Pruebas de las consultas reales con tablas temporales, sin acceder a PranaElectronica. |
| `Instalador/PranaActual.iss` | Version del instalador 1.0.9. |

No se modificaron entidades, formularios, reglas de pagos ni sentencias de stock.

## Validacion realizada

- Compilacion de la solucion: correcta, cero errores; permanecen advertencias
  del proyecto (nulabilidad, campos sin usar y otras existentes).
- Analisis sintactico con Microsoft SQL ScriptDom: correctos el script de
  instalacion, la migracion y su bloque SQL dinamico de comprobaciones.
- `Pruebas/VerificarEntregas.ps1`: 12 escenarios correctos para estados y
  pendientes que impiden la baja; incluye entrega completa inmediata, entregas
  parciales, varias preparaciones, variantes vendidas en varios renglones,
  envios duplicados y ventas anuladas/canceladas. Tambien comprueba el rechazo
  de una segunda confirmacion y el paso a entrega completa al confirmar 1 + 2.
- Las pruebas usan exclusivamente tablas temporales en una conexion a `master`.
  Se ejecutan con:

  ```powershell
  powershell -NoProfile -ExecutionPolicy Bypass -File Pruebas/VerificarEntregas.ps1
  ```

La migracion no se ejecuto, ni siquiera como parte de las pruebas. Su DROP y su
idempotencia fueron revisados estaticamente; no se afirma una prueba de ejecucion
de la migracion. Tampoco se ejecuto la instalacion ni una venta real. Los flujos
de guardado y stock se revisaron en codigo y mediante compilacion, sin efectuar
operaciones comerciales sobre la base existente.
