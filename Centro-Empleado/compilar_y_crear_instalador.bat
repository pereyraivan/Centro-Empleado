@echo off
echo ========================================
echo    COMPILAR Y CREAR INSTALADOR
echo    Sistema Centro de Empleados
echo ========================================
echo.

REM Verificar que Visual Studio esté instalado
if not exist "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" (
    echo ERROR: Visual Studio 2019 no encontrado
    echo Por favor, compile el proyecto manualmente desde Visual Studio
    pause
    exit /b 1
)

echo Compilando proyecto...
echo.

REM Compilar el proyecto principal
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" "Centro-Empleado\Centro-Empleado.csproj" /p:Configuration=Debug /p:Platform=x86 /verbosity:minimal

if %ERRORLEVEL% neq 0 (
    echo ERROR: Fallo en la compilacion del proyecto principal
    echo Por favor, revise los errores en Visual Studio
    pause
    exit /b 1
)

echo.
echo Verificando archivo ejecutable...
if not exist "Centro-Empleado\bin\Debug\Centro-Empleado.exe" (
    echo ERROR: No se genero el archivo ejecutable
    echo Por favor, compile el proyecto desde Visual Studio
    pause
    exit /b 1
)

echo Archivo ejecutable encontrado: Centro-Empleado\bin\Debug\Centro-Empleado.exe
echo.

echo Compilando instalador...
echo.

REM Compilar el instalador
"C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" /p:Configuration=Release /verbosity:minimal

if %ERRORLEVEL% neq 0 (
    echo ERROR: Fallo en la compilacion del instalador
    echo Por favor, revise los errores en Visual Studio
    pause
    exit /b 1
)

echo.
echo ========================================
echo    COMPILACION COMPLETADA EXITOSAMENTE
echo ========================================
echo.

REM Verificar archivos generados
if exist "Centro-Empleados-Setup\Release\Centro-Empleados-Setup.msi" (
    echo Instalador generado: Centro-Empleados-Setup\Release\Centro-Empleados-Setup.msi
    echo.
    echo El instalador esta listo para distribuir
) else (
    echo ADVERTENCIA: No se encontro el archivo MSI generado
    echo Verifique la configuracion del proyecto de instalador
)

echo.
echo Presione cualquier tecla para continuar...
pause >nul
