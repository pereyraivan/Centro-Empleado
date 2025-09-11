@echo off
echo ========================================
echo    VERIFICAR ARCHIVOS DEL INSTALADOR
echo    Sistema Centro de Empleados
echo ========================================
echo.

echo Verificando archivos necesarios para el instalador...
echo.

REM Verificar archivo ejecutable principal
if exist "Centro-Empleado\bin\Debug\Centro-Empleado.exe" (
    echo ✅ Centro-Empleado.exe encontrado
    for %%F in ("Centro-Empleado\bin\Debug\Centro-Empleado.exe") do echo    Tamaño: %%~zF bytes
    for %%F in ("Centro-Empleado\bin\Debug\Centro-Empleado.exe") do echo    Fecha: %%~tF
) else (
    echo ❌ Centro-Empleado.exe NO encontrado
    echo    Ruta esperada: Centro-Empleado\bin\Debug\Centro-Empleado.exe
)

echo.

REM Verificar archivo de configuración
if exist "Centro-Empleado\bin\Debug\Centro-Empleado.exe.config" (
    echo ✅ Centro-Empleado.exe.config encontrado
) else (
    echo ❌ Centro-Empleado.exe.config NO encontrado
)

echo.

REM Verificar base de datos
if exist "Centro-Empleado\bin\Debug\CentroEmpleado.db" (
    echo ✅ CentroEmpleado.db encontrado
    for %%F in ("Centro-Empleado\bin\Debug\CentroEmpleado.db") do echo    Tamaño: %%~zF bytes
) else (
    echo ❌ CentroEmpleado.db NO encontrado
)

echo.

REM Verificar archivos HTML
if exist "Centro-Empleado\bin\Debug\recetaFinal.html" (
    echo ✅ recetaFinal.html encontrado
) else (
    echo ❌ recetaFinal.html NO encontrado
)

if exist "Centro-Empleado\bin\Debug\bonoFinal.html" (
    echo ✅ bonoFinal.html encontrado
) else (
    echo ❌ bonoFinal.html NO encontrado
)

echo.

REM Verificar DLLs de SQLite
if exist "Centro-Empleado\bin\Debug\SQLite.Interop.dll" (
    echo ✅ SQLite.Interop.dll encontrado
) else (
    echo ❌ SQLite.Interop.dll NO encontrado
)

if exist "Centro-Empleado\bin\Debug\System.Data.SQLite.dll" (
    echo ✅ System.Data.SQLite.dll encontrado
) else (
    echo ❌ System.Data.SQLite.dll NO encontrado
)

echo.

REM Verificar archivos del instalador
if exist "Centro-Empleados-Setup\Release\Centro-Empleados-Setup.msi" (
    echo ✅ Centro-Empleados-Setup.msi encontrado
    for %%F in ("Centro-Empleados-Setup\Release\Centro-Empleados-Setup.msi") do echo    Tamaño: %%~zF bytes
    for %%F in ("Centro-Empleados-Setup\Release\Centro-Empleados-Setup.msi") do echo    Fecha: %%~tF
) else (
    echo ❌ Centro-Empleados-Setup.msi NO encontrado
    echo    Ruta esperada: Centro-Empleados-Setup\Release\Centro-Empleados-Setup.msi
)

echo.
echo ========================================
echo    RESUMEN DE VERIFICACION
echo ========================================
echo.

REM Contar archivos faltantes
set /a missing=0

if not exist "Centro-Empleado\bin\Debug\Centro-Empleado.exe" set /a missing+=1
if not exist "Centro-Empleado\bin\Debug\Centro-Empleado.exe.config" set /a missing+=1
if not exist "Centro-Empleado\bin\Debug\CentroEmpleado.db" set /a missing+=1
if not exist "Centro-Empleados-Setup\Release\Centro-Empleados-Setup.msi" set /a missing+=1

if %missing% equ 0 (
    echo ✅ TODOS LOS ARCHIVOS ESTAN PRESENTES
    echo El instalador esta listo para distribuir
) else (
    echo ❌ FALTAN %missing% ARCHIVOS
    echo Por favor, compile el proyecto desde Visual Studio
)

echo.
echo Presione cualquier tecla para continuar...
pause >nul
