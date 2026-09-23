-- amplía el descuento solo cuando la columna todavía tiene una precisión menor
IF EXISTS (
    SELECT 1
    FROM sys.columns AS c
    INNER JOIN sys.types AS t ON t.user_type_id = c.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.Venta')
      AND c.name = N'descuento'
      AND t.name = N'decimal'
      AND c.precision < 10
)
BEGIN
    ALTER TABLE dbo.Venta ALTER COLUMN descuento DECIMAL(10,2) NULL;
END;
