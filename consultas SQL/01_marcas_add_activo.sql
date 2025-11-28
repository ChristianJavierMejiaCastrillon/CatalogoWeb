-- Agregar columna Activo a MARCAS + default
ALTER TABLE dbo.MARCAS
ADD Activo BIT NOT NULL CONSTRAINT DF_MARCAS_Activo DEFAULT(1);

-- Asegurar valores existentes
UPDATE dbo.MARCAS SET Activo = 1 WHERE Activo IS NULL;
