@echo off
echo ========================================
echo    ACTUALIZAR VERSION DEL INSTALADOR
echo    Sistema Centro de Empleados
echo ========================================
echo.

REM Obtener la version actual del proyecto principal
for /f "tokens=2 delims=:" %%a in ('findstr "AssemblyVersion" Centro-Empleado\Properties\AssemblyInfo.cs') do (
    set "current_version=%%a"
    set "current_version=!current_version: =!"
    set "current_version=!current_version:.*=!"
)

echo Version actual del proyecto: %current_version%
echo.

REM Solicitar nueva version
set /p new_version="Ingrese la nueva version (ej: 1.0.8): "

if "%new_version%"=="" (
    echo ERROR: Debe ingresar una version
    pause
    exit /b 1
)

echo.
echo Actualizando version a: %new_version%
echo.

REM Actualizar AssemblyInfo.cs
echo Actualizando AssemblyInfo.cs...
powershell -Command "(Get-Content 'Centro-Empleado\Properties\AssemblyInfo.cs') -replace 'AssemblyVersion\(\".*?\"\)', 'AssemblyVersion(\"%new_version%.0\")' | Set-Content 'Centro-Empleado\Properties\AssemblyInfo.cs'"
powershell -Command "(Get-Content 'Centro-Empleado\Properties\AssemblyInfo.cs') -replace 'AssemblyFileVersion\(\".*?\"\)', 'AssemblyFileVersion(\"%new_version%.0\")' | Set-Content 'Centro-Empleado\Properties\AssemblyInfo.cs'"

REM Actualizar el proyecto de instalador
echo Actualizando proyecto de instalador...
powershell -Command "(Get-Content 'Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj') -replace 'ProductVersion.*?\"8:[^\"]*\"', 'ProductVersion\" = \"8:%new_version%\"' | Set-Content 'Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj'"

REM Generar nuevo ProductCode (opcional, pero recomendado)
echo Generando nuevo ProductCode...
set "new_product_code={%random%-%random%-%random%-%random%-%random%}"
echo Nuevo ProductCode: %new_product_code%

REM Actualizar ProductCode en el instalador
powershell -Command "(Get-Content 'Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj') -replace 'ProductCode.*?\"8:{[^}]*}\"', 'ProductCode\" = \"8:{%new_product_code%}\"' | Set-Content 'Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj'"

echo.
echo ========================================
echo    VERSION ACTUALIZADA EXITOSAMENTE
echo ========================================
echo.
echo Version anterior: %current_version%
echo Version nueva: %new_version%
echo ProductCode: %new_product_code%
echo.
echo Ahora puede compilar el proyecto para generar el nuevo MSI
echo.
echo Presione cualquier tecla para continuar...
pause >nul
