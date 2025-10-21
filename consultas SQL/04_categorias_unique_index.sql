-- Índice único filtrado: evita duplicados entre categorías ACTIVAS
CREATE UNIQUE INDEX UX_CATEGORIAS_Descripcion_Activas
ON dbo.CATEGORIAS(Descripcion)
WHERE Activo = 1;