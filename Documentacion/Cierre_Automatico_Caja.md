# Cierre automático de caja

La caja se organiza por fecha calendario del equipo que ejecuta SQL Server LocalDB. A medianoche los nuevos cobros y movimientos pertenecen al nuevo día. Un temporizador nativo de Windows Forms comprueba el cambio de fecha durante toda la ejecución, incluso cuando se está en otra sección o en el login. El cierre se procesa en el primer ciclo disponible después de medianoche; no depende de mantener abierta la pantalla de caja.

Los saldos iniciales en pesos y dólares, la hora de inicio y las observaciones se guardan al modificar sus controles en `Caja_Abierta`. Al volver a esa pantalla se recuperan. Al cerrar se vuelven a consultar los movimientos reales y se conserva el importe base de los pagos, sin recargo. Pesos y dólares se mantienen separados.

Al cambiar de fecha, `CD_Caja.CerrarDiasAnteriores` busca días anteriores con movimientos o datos de caja abierta que todavía no tengan cierre. Guarda cada cierre con el texto `CIERRE AUTOMÁTICO`, sin atribuirlo a un empleado, y con el límite de medianoche como hora final. Utiliza el mismo bloqueo por fecha que los pagos para evitar cierres duplicados y escrituras simultáneas. Un cierre manual ya guardado se conserva.

Si el programa o la computadora estaban apagados, el proceso se ejecuta al iniciar, después de preparar la estructura de la base y antes del login. No se ejecuta un proceso mientras la computadora está apagada. Los días sin movimientos ni datos guardados no generan cierres vacíos. Al aplicar esta versión también se completan días históricos con actividad que todavía no tenían cierre; si no tenían datos iniciales guardados, sus saldos iniciales son cero.

Cuando la pantalla de caja estaba mostrando el día que termina, pasa al nuevo día. Una consulta de otra fecha se mantiene. El saldo inicial del nuevo día se ingresa de forma independiente: no se traslada automáticamente el balance del día anterior. Los datos de días cerrados quedan disponibles para consulta e impresión.

Los movimientos manuales de fechas anteriores no se pueden modificar después del cambio de día. Si falla el cierre automático durante la ejecución, se muestra un aviso y se reintenta cada treinta segundos. Si falla la recuperación al iniciar, se informa el problema y no se habilita el acceso al sistema hasta poder completarla.

## Verificación

La prueba aislada se ejecuta desde la carpeta del repositorio:

```powershell
dotnet run --project Pruebas/CajaComprobantes/Pruebas.csproj -- --cierre-automatico
```

La prueba utiliza exclusivamente la base temporal configurada en ese proyecto y comprueba recuperación de saldos, días pendientes, cierre concurrente sin duplicados, caja actual abierta y totales sin recargo. Requiere una instancia de LocalDB disponible y no cambia la fecha de Windows.
