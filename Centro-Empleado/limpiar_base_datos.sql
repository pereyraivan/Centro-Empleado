-- Script para limpiar todos los datos de la base de datos
-- Mantiene la estructura de las tablas pero elimina todos los registros
-- Ejecutar este script antes de crear el instalador

-- Desactivar verificación de claves foráneas temporalmente
PRAGMA foreign_keys = OFF;

-- Limpiar tabla de Recetarios Extraordinarios (eliminar todos los registros)
DELETE FROM RecetarioExtraordinario;

-- Limpiar tabla de Recetarios Mensuales (eliminar todos los registros)
DELETE FROM RecetarioMensual;

-- Limpiar tabla de Bonos (eliminar todos los registros)
DELETE FROM Bono;

-- Limpiar tabla de Recetarios (eliminar todos los registros)
DELETE FROM Recetario;

-- Limpiar tabla de Familiares (eliminar todos los registros)
DELETE FROM Familiar;

-- Limpiar tabla de Afiliados (eliminar todos los registros)
DELETE FROM Afiliado;

-- Reiniciar los contadores de auto-incremento
DELETE FROM sqlite_sequence WHERE name IN ('Afiliado', 'Familiar', 'Recetario', 'Bono', 'RecetarioMensual', 'RecetarioExtraordinario');

-- Reactivar verificación de claves foráneas
PRAGMA foreign_keys = ON;

-- Verificar que las tablas estén vacías
SELECT 'Afiliados restantes: ' || COUNT(*) as Resultado FROM Afiliado;
SELECT 'Familiares restantes: ' || COUNT(*) as Resultado FROM Familiar;
SELECT 'Recetarios restantes: ' || COUNT(*) as Resultado FROM Recetario;
SELECT 'Bonos restantes: ' || COUNT(*) as Resultado FROM Bono;
SELECT 'Recetarios Mensuales restantes: ' || COUNT(*) as Resultado FROM RecetarioMensual;
SELECT 'Recetarios Extraordinarios restantes: ' || COUNT(*) as Resultado FROM RecetarioExtraordinario;

-- Mensaje de confirmación
SELECT 'Base de datos limpiada exitosamente. Todas las tablas están vacías.' as Estado;
