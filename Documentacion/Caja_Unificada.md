# Caja unificada por apertura

Se conservan dos tablas propias: Caja y Caja_Movimiento. La misma fila de Caja permanece abierta y luego cerrada; Pago conserva los cobros de ventas. La implementación actual permite varias aperturas por fecha y una sola abierta a la vez. Cada operación se relaciona con id_caja, por lo que no se mezclan turnos ni se duplican importes.

Ver [Cajas por turno](Cajas_Por_Turno.md) para los métodos, columnas, migración y pruebas actuales.
