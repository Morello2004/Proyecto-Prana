# Cierre automático por apertura

El sistema permite varios cierres por fecha, con una sola caja abierta a la vez. Program comprueba el cambio de día cada segundo y recupera pendientes al iniciar. CerrarDiasAnteriores cierra únicamente aperturas que continúan abiertas de fechas anteriores, identificadas por Caja.id. Conserva CIERRE AUTOMÁTICO, el límite de medianoche y los movimientos de ese turno, sin tocar los cierres manuales de la misma fecha. Después debe abrirse explícitamente una nueva caja para operar.

La descripción completa está en [Cajas por turno](Cajas_Por_Turno.md). La prueba aislada es: dotnet run --project Pruebas/CajaComprobantes/Pruebas.csproj -- --turnos-caja.
