@echo off
echo ========================================
echo CONFIGURACION AUTOMATICA DE BASE DE DATOS
echo ========================================
echo.

REM Verificar si existe la base de datos
if exist "CentroEmpleado.db" (
    echo Base de datos encontrada: CentroEmpleado.db
    echo Tamaño: 
    dir CentroEmpleado.db | find "CentroEmpleado.db"
    echo.
) else (
    echo Base de datos no encontrada.
    echo Se creará automáticamente al ejecutar la aplicación.
    echo.
)

REM Verificar archivos SQLite
echo Verificando archivos SQLite...
if exist "System.Data.SQLite.dll" (
    echo ✓ System.Data.SQLite.dll - OK
) else (
    echo ✗ System.Data.SQLite.dll - FALTANTE
)

if exist "System.Data.SQLite.EF6.dll" (
    echo ✓ System.Data.SQLite.EF6.dll - OK
) else (
    echo ✗ System.Data.SQLite.EF6.dll - FALTANTE
)

if exist "System.Data.SQLite.Core.dll" (
    echo ✓ System.Data.SQLite.Core.dll - OK
) else (
    echo ✗ System.Data.SQLite.Core.dll - FALTANTE
)

echo.
echo ========================================
echo CONFIGURACION COMPLETADA
echo ========================================
echo.
echo La base de datos se creará automáticamente
echo al ejecutar CentroEmpleado.exe por primera vez.
echo.
echo Presiona cualquier tecla para continuar...
pause >nul
