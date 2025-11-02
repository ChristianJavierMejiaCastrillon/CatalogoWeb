-- Agregar columna Activo a CATEGORIAS + default
ALTER TABLE dbo.CATEGORIAS
ADD Activo BIT NOT NULL CONSTRAINT DF_CATEGORIAS_Activo DEFAULT(1);

-- Asegurar valores existentes
UPDATE dbo.CATEGORIAS SET Activo = 1 WHERE Activo IS NULL;
