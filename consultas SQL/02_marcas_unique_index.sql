-- Índice único filtrado: evita duplicados entre marcas ACTIVAS
CREATE UNIQUE INDEX UX_MARCAS_Descripcion_Activas
ON dbo.MARCAS(Descripcion)
WHERE Activo = 1;