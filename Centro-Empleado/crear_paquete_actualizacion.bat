@echo off
echo ========================================
echo    CREAR PAQUETE DE ACTUALIZACION
echo ========================================
echo.

REM Crear carpeta del paquete
set "version=2.0"
set "paquete=Paquete_Actualizacion_v%version%"
if exist "%paquete%" rmdir /s /q "%paquete%"
mkdir "%paquete%"

echo Creando paquete de actualizacion v%version%...
echo.

REM Copiar archivos de la aplicacion (sin base de datos)
echo Copiando archivos de la aplicacion...
copy "Centro-Empleado.exe" "%paquete%\" >nul
copy "*.dll" "%paquete%\" >nul
copy "*.bat" "%paquete%\" >nul
copy "Manual_Usuario.html" "%paquete%\" >nul
copy "GUIA_ACTUALIZACION.md" "%paquete%\" >nul
copy "INSTRUCCIONES_INSTALACION.txt" "%paquete%\" >nul

REM Copiar carpeta Resources
if exist "Resources" (
    echo Copiando carpeta Resources...
    xcopy "Resources" "%paquete%\Resources" /E /I >nul
)

REM Copiar archivos de configuracion de ejemplo
if exist "App.config" copy "App.config" "%paquete%\" >nul

REM Crear archivo de version
echo Creando archivo de version...
echo Version: %version% > "%paquete%\version.txt"
echo Fecha: %date% >> "%paquete%\version.txt"
echo Hora: %time% >> "%paquete%\version.txt"

REM Crear archivo README
echo Creando archivo README...
(
echo ========================================
echo    PAQUETE DE ACTUALIZACION v%version%
echo ========================================
echo.
echo Este paquete contiene una actualizacion del sistema
echo Centro de Empleados. Siga las instrucciones en
echo INSTRUCCIONES_INSTALACION.txt
echo.
echo ARCHIVOS INCLUIDOS:
echo - Centro-Empleado.exe: Aplicacion principal
echo - *.dll: Bibliotecas del sistema
echo - Resources/: Recursos de la aplicacion
echo - Scripts .bat: Herramientas de instalacion
echo - Manual_Usuario.html: Manual actualizado
echo.
echo IMPORTANTE:
echo - NO incluye la base de datos (se preserva la actual)
echo - NO incluye config.txt (se preserva la actual)
echo - Siempre crear respaldo antes de actualizar
echo.
echo Para instalar: Ejecutar instalar_actualizacion.bat
echo.
) > "%paquete%\README.txt"

echo.
echo ========================================
echo    PAQUETE CREADO EXITOSAMENTE
echo ========================================
echo.
echo Ubicacion: %paquete%\
echo.
echo Archivos incluidos:
dir /b "%paquete%"
echo.
echo El paquete esta listo para enviar al cliente
echo.
echo Presione cualquier tecla para continuar...
pause >nul
