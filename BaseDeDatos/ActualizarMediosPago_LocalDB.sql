-- Agrega los recargos por cuotas y la baja logica a Medio_Pago.
USE PranaElectronica;
GO

-- Agrega el recargo para tres cuotas.
IF COL_LENGTH('dbo.Medio_Pago', 'recargo_3') IS NULL ALTER TABLE dbo.Medio_Pago ADD recargo_3 DECIMAL(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo3 DEFAULT 0;
GO

-- Agrega el recargo para seis cuotas.
IF COL_LENGTH('dbo.Medio_Pago', 'recargo_6') IS NULL ALTER TABLE dbo.Medio_Pago ADD recargo_6 DECIMAL(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo6 DEFAULT 0;
GO

-- Agrega el recargo para nueve cuotas.
IF COL_LENGTH('dbo.Medio_Pago', 'recargo_9') IS NULL ALTER TABLE dbo.Medio_Pago ADD recargo_9 DECIMAL(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo9 DEFAULT 0;
GO

-- Agrega el recargo para doce cuotas.
IF COL_LENGTH('dbo.Medio_Pago', 'recargo_12') IS NULL ALTER TABLE dbo.Medio_Pago ADD recargo_12 DECIMAL(5,2) NOT NULL CONSTRAINT DF_MedioPago_Recargo12 DEFAULT 0;
GO

-- Agrega el estado activo para realizar bajas logicas.
IF COL_LENGTH('dbo.Medio_Pago', 'activo') IS NULL ALTER TABLE dbo.Medio_Pago ADD activo BIT NOT NULL CONSTRAINT DF_MedioPago_Activo DEFAULT 1;
GO
