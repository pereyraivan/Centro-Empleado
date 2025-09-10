@echo off
echo ========================================
echo    INSTALADOR DE ACTUALIZACION
echo    Sistema Centro de Empleados
echo ========================================
echo.

REM Verificar si se está ejecutando desde la carpeta correcta
if not exist "Centro-Empleado.exe" (
    echo ERROR: Este script debe ejecutarse desde la carpeta de la aplicacion
    echo Asegurese de estar en la carpeta donde esta instalado el sistema
    pause
    exit /b 1
)

echo Verificando instalacion actual...
echo.

REM Verificar si existe la base de datos
if not exist "CentroEmpleado.db" (
    echo ADVERTENCIA: No se encontro la base de datos actual
    echo Esto puede ser una instalacion nueva
    echo.
    set /p continue="¿Desea continuar con la instalacion? (S/N): "
    if /i not "%continue%"=="S" (
        echo Instalacion cancelada
        pause
        exit /b 0
    )
)

echo Paso 1: Creando respaldo de seguridad...
call backup_database.bat

if %errorlevel% neq 0 (
    echo ERROR: No se pudo crear el respaldo
    echo Abortando instalacion por seguridad
    pause
    exit /b 1
)

echo.
echo Paso 2: Preparando actualizacion...
echo.

REM Crear carpeta temporal
if not exist "ActualizacionTemp" mkdir ActualizacionTemp

echo INSTRUCCIONES:
echo.
echo 1. Copie todos los archivos de la nueva version en la carpeta 'ActualizacionTemp'
echo 2. Asegurese de incluir:
echo    - Centro-Empleado.exe
echo    - Archivos .dll
echo    - Carpeta Resources
echo    - Archivos .bat
echo    - Manual_Usuario.html
echo.
echo 3. NO incluya CentroEmpleado.db (se preservara la actual)
echo 4. NO incluya config.txt (se preservara la actual)
echo.
echo Presione cualquier tecla cuando haya copiado los archivos...
pause

REM Verificar que se copiaron los archivos necesarios
if not exist "ActualizacionTemp\Centro-Empleado.exe" (
    echo ERROR: No se encontro Centro-Empleado.exe en ActualizacionTemp
    echo Por favor, copie los archivos correctamente
    pause
    exit /b 1
)

echo.
echo Paso 3: Realizando actualizacion...

REM Hacer respaldo de archivos de configuracion
if exist "config.txt" (
    copy "config.txt" "Backups\config_respaldo_%date:~-4,4%%date:~-10,2%%date:~-7,2%.txt" >nul
    echo Archivo de configuracion respaldado
)

REM Reemplazar archivos de la aplicacion
echo Reemplazando archivos de la aplicacion...
for %%f in (ActualizacionTemp\*.*) do (
    if not "%%~nxf"=="CentroEmpleado.db" (
        if not "%%~nxf"=="config.txt" (
            echo Copiando %%~nxf...
            copy "%%f" "." >nul
        )
    )
)

REM Copiar carpetas
if exist "ActualizacionTemp\Resources" (
    echo Copiando carpeta Resources...
    xcopy "ActualizacionTemp\Resources" "Resources" /E /Y >nul
)

REM Limpiar carpeta temporal
rmdir /s /q "ActualizacionTemp"

echo.
echo ========================================
echo    ACTUALIZACION COMPLETADA
echo ========================================
echo.
echo ✓ Respaldo de seguridad creado
echo ✓ Archivos de aplicacion actualizados
echo ✓ Base de datos preservada
echo ✓ Configuracion mantenida
echo.
echo El sistema esta listo para usar con la nueva version
echo.
echo Presione cualquier tecla para continuar...
pause >nul
