@echo off
echo ========================================
echo    RESTAURAR RESPALDO DE BASE DE DATOS
echo ========================================
echo.

REM Verificar si existe la carpeta de respaldos
if not exist "Backups" (
    echo ERROR: No se encontro la carpeta de respaldos
    echo No hay respaldos disponibles para restaurar
    pause
    exit /b 1
)

echo Respaldo disponibles:
echo.
dir /b Backups\CentroEmpleado_*.db
echo.

set /p backup_file="Ingrese el nombre del archivo de respaldo a restaurar: "

REM Verificar que el archivo existe
if not exist "Backups\%backup_file%" (
    echo ERROR: El archivo de respaldo no existe
    echo Asegurese de escribir el nombre completo del archivo
    pause
    exit /b 1
)

echo.
echo ADVERTENCIA: Esta accion reemplazara la base de datos actual
echo con el respaldo seleccionado. Los datos actuales se perderan.
echo.
set /p confirm="¿Esta seguro que desea continuar? (S/N): "

if /i not "%confirm%"=="S" (
    echo Operacion cancelada
    pause
    exit /b 0
)

echo.
echo Restaurando respaldo...

REM Hacer respaldo de la base de datos actual antes de restaurar
if exist "CentroEmpleado.db" (
    for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
    set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
    set "HH=%dt:~8,2%" & set "Min=%dt:~10,2%" & set "Sec=%dt:~12,2%"
    set "timestamp=%YYYY%-%MM%-%DD%_%HH%-%Min%-%Sec%"
    copy "CentroEmpleado.db" "Backups\CentroEmpleado_ANTES_RESTAURAR_%timestamp%.db" >nul
    echo Respaldo de seguridad creado antes de restaurar
)

REM Restaurar el respaldo
copy "Backups\%backup_file%" "CentroEmpleado.db"

if %errorlevel% equ 0 (
    echo.
    echo ✓ Respaldo restaurado exitosamente
    echo La base de datos ha sido restaurada al estado del respaldo seleccionado
) else (
    echo.
    echo ✗ ERROR: No se pudo restaurar el respaldo
    echo Verifique los permisos de escritura
)

echo.
echo Presione cualquier tecla para continuar...
pause >nul
