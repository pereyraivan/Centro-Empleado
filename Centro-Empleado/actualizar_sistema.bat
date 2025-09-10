@echo off
echo ========================================
echo    ACTUALIZACION SEGURA DEL SISTEMA
echo ========================================
echo.

REM Verificar si existe la base de datos actual
if not exist "CentroEmpleado.db" (
    echo ERROR: No se encontro la base de datos actual
    echo Este script debe ejecutarse en la carpeta donde esta instalado el sistema
    pause
    exit /b 1
)

echo Paso 1: Creando respaldo de seguridad...
call backup_database.bat

if %errorlevel% neq 0 (
    echo ERROR: No se pudo crear el respaldo. Abortando actualizacion.
    pause
    exit /b 1
)

echo.
echo Paso 2: Preparando actualizacion...
echo.

REM Crear carpeta temporal para la nueva version
if not exist "NuevaVersion" mkdir NuevaVersion

echo INSTRUCCIONES PARA ACTUALIZAR:
echo.
echo 1. Copie los archivos de la nueva version en la carpeta 'NuevaVersion'
echo 2. Asegurese de que NO incluya el archivo CentroEmpleado.db de la nueva version
echo 3. Presione cualquier tecla cuando haya copiado los archivos
echo.
pause

REM Verificar que se copiaron los archivos de la nueva version
if not exist "NuevaVersion\Centro-Empleado.exe" (
    echo ERROR: No se encontro Centro-Empleado.exe en la carpeta NuevaVersion
    echo Por favor, copie los archivos de la nueva version correctamente
    pause
    exit /b 1
)

echo.
echo Paso 3: Realizando actualizacion...

REM Hacer respaldo de archivos de configuracion importantes
if exist "config.txt" copy "config.txt" "Backups\config_%timestamp%.txt" >nul

REM Reemplazar archivos de la aplicacion (excepto la base de datos)
echo Reemplazando archivos de la aplicacion...
for %%f in (NuevaVersion\*.*) do (
    if not "%%~nxf"=="CentroEmpleado.db" (
        echo Copiando %%~nxf...
        copy "%%f" "." >nul
    )
)

REM Limpiar carpeta temporal
rmdir /s /q "NuevaVersion"

echo.
echo ✓ Actualizacion completada exitosamente
echo.
echo La base de datos y configuraciones se mantuvieron intactas
echo Los respaldos estan disponibles en la carpeta 'Backups'
echo.
echo Presione cualquier tecla para continuar...
pause >nul
