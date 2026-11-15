-- ejecutar en la base que contiene ambas tablas solo consulta, no modifica datos
SET NOCOUNT ON;
-- consulta los datos o metadatos indicados; por si sola no guarda cambios en las tablas
SELECT DB_NAME() AS base_actual;


SELECT s.name AS esquema, t.name AS tabla, SUM(p.rows) AS filas
-- define el origen de los registros utilizados por la consulta
FROM sys.tables t
-- relaciona filas de tablas distintas mediante la condicion on
JOIN sys.schemas s ON s.schema_id=t.schema_id
-- conserva el origen izquierdo aunque no encuentre coincidencia; los datos faltantes pueden resultar null
LEFT JOIN sys.partitions p ON p.object_id=t.object_id AND p.index_id IN (0,1)
-- limita los registros afectados o consultados; es importante comprobarlo antes de ejecutar escrituras
WHERE t.name IN (N'Producto', N'Productos')
-- agrupa filas para calcular cantidades o totales por las claves indicadas
GROUP BY s.name,t.name;


SELECT fk.name AS restriccion,
    OBJECT_SCHEMA_NAME(fk.parent_object_id) AS esquema_origen,
    OBJECT_NAME(fk.parent_object_id) AS tabla_origen,
    OBJECT_SCHEMA_NAME(fk.referenced_object_id) AS esquema_destino,
    OBJECT_NAME(fk.referenced_object_id) AS tabla_destino

FROM sys.foreign_keys fk

WHERE OBJECT_NAME(fk.referenced_object_id) IN (N'Producto',N'Productos')
   OR OBJECT_NAME(fk.parent_object_id) IN (N'Producto',N'Productos');


SELECT OBJECT_SCHEMA_NAME(d.referencing_id) AS esquema,
    OBJECT_NAME(d.referencing_id) AS objeto_dependiente,
    d.referenced_schema_name AS esquema_referenciado,
    d.referenced_entity_name AS tabla_referenciada

FROM sys.sql_expression_dependencies d

WHERE d.referenced_entity_name IN (N'Producto',N'Productos');
