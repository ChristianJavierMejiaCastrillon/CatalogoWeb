-- �ndice �nico filtrado: evita duplicados entre categor�as ACTIVAS
CREATE UNIQUE INDEX UX_CATEGORIAS_Descripcion_Activas
ON dbo.CATEGORIAS(Descripcion)
WHERE Activo = 1;