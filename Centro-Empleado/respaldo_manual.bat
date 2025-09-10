@echo off
echo ========================================
echo    RESPALDO MANUAL DE BASE DE DATOS
echo    Sistema Centro de Empleados
echo ========================================
echo.

REM Verificar si existe la base de datos
if not exist "CentroEmpleado.db" (
    echo ERROR: No se encontro la base de datos CentroEmpleado.db
    echo Asegurese de ejecutar este script desde la carpeta de la aplicacion
    echo.
    echo Presione cualquier tecla para continuar...
    pause >nul
    exit /b 1
)

echo Base de datos encontrada:
dir "CentroEmpleado.db" | findstr CentroEmpleado
echo.

REM Crear carpeta de respaldos si no existe
if not exist "Backups" mkdir Backups

REM Obtener fecha y hora actual
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
set "timestamp=%YYYY%-%MM%-%DD%_%HH%-%Min%-%Sec%"

REM Mostrar confirmacion
echo ¿Desea crear un respaldo de la base de datos?
echo.
echo El respaldo se guardara como: CentroEmpleado_%timestamp%.db
echo Ubicacion: Backups\
echo.
set /p confirm="¿Continuar? (S/N): "

if /i not "%confirm%"=="S" (
    echo Operacion cancelada
    echo.
    echo Presione cualquier tecla para continuar...
    pause >nul
    exit /b 0
)

echo.
echo Creando respaldo...

REM Crear respaldo
set "backup_file=Backups\CentroEmpleado_%timestamp%.db"
copy "CentroEmpleado.db" "%backup_file%"

if %errorlevel% equ 0 (
    echo.
    echo ✓ Respaldo creado exitosamente
    echo.
    echo Archivo: %backup_file%
    echo Tamaño:
    dir "%backup_file%" | findstr CentroEmpleado
    echo.
    echo Ubicacion: %cd%\Backups\
    echo.
    echo ✓ Base de datos respaldada correctamente
) else (
    echo.
    echo ✗ ERROR: No se pudo crear el respaldo
    echo Verifique los permisos de escritura en la carpeta
)

echo.
echo Presione cualquier tecla para continuar...
pause >nul
