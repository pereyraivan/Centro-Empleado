@echo off
echo ========================================
echo    RESPALDO DE BASE DE DATOS
echo ========================================
echo.

REM Crear carpeta de respaldos si no existe
if not exist "Backups" mkdir Backups

REM Obtener fecha y hora actual
for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
set "timestamp=%YYYY%-%MM%-%DD%_%HH%-%Min%-%Sec%"

REM Verificar si existe la base de datos
if not exist "CentroEmpleado.db" (
    echo ERROR: No se encontro la base de datos CentroEmpleado.db
    echo Asegurese de ejecutar este script desde la carpeta de la aplicacion
    pause
    exit /b 1
)

REM Crear respaldo
set "backup_file=Backups\CentroEmpleado_%timestamp%.db"
copy "CentroEmpleado.db" "%backup_file%"

if %errorlevel% equ 0 (
    echo.
    echo ✓ Respaldo creado exitosamente: %backup_file%
    echo.
    echo Tamaño del archivo:
    dir "%backup_file%" | findstr CentroEmpleado
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
