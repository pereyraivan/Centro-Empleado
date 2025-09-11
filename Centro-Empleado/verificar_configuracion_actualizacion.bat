@echo off
echo ========================================
echo    VERIFICAR CONFIGURACION ACTUALIZACION
echo    Sistema Centro de Empleados
echo ========================================
echo.

echo Verificando configuracion del instalador para actualizacion automatica...
echo.

REM Verificar RemovePreviousVersions
echo 1. Verificando RemovePreviousVersions...
findstr /C:"RemovePreviousVersions" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"TRUE"
if %ERRORLEVEL% equ 0 (
    echo    ✅ RemovePreviousVersions = TRUE (Correcto)
) else (
    echo    ❌ RemovePreviousVersions = FALSE (Incorrecto)
)

echo.

REM Verificar DetectNewerInstalledVersion
echo 2. Verificando DetectNewerInstalledVersion...
findstr /C:"DetectNewerInstalledVersion" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"TRUE"
if %ERRORLEVEL% equ 0 (
    echo    ✅ DetectNewerInstalledVersion = TRUE (Correcto)
) else (
    echo    ❌ DetectNewerInstalledVersion = FALSE (Incorrecto)
)

echo.

REM Verificar UpgradeCode
echo 3. Verificando UpgradeCode...
findstr /C:"UpgradeCode" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj"
if %ERRORLEVEL% equ 0 (
    echo    ✅ UpgradeCode configurado (Correcto)
) else (
    echo    ❌ UpgradeCode no encontrado (Incorrecto)
)

echo.

REM Verificar que la base de datos NO se incluye
echo 4. Verificando exclusion de base de datos...
findstr /C:"CentroEmpleado.db" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"Exclude.*TRUE"
if %ERRORLEVEL% equ 0 (
    echo    ✅ Base de datos EXCLUIDA del instalador (Correcto)
) else (
    echo    ❌ Base de datos INCLUIDA en el instalador (Incorrecto)
)

echo.

REM Verificar que el archivo de configuracion NO se incluye
echo 5. Verificando exclusion de archivo de configuracion...
findstr /C:"Centro-Empleado.exe.config" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"Exclude.*TRUE"
if %ERRORLEVEL% equ 0 (
    echo    ✅ Archivo de configuracion EXCLUIDO del instalador (Correcto)
) else (
    echo    ❌ Archivo de configuracion INCLUIDO en el instalador (Incorrecto)
)

echo.

REM Verificar que el ejecutable SÍ se incluye
echo 6. Verificando inclusion del ejecutable...
findstr /C:"Centro-Empleado.exe" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"Exclude.*FALSE"
if %ERRORLEVEL% equ 0 (
    echo    ✅ Ejecutable INCLUIDO en el instalador (Correcto)
) else (
    echo    ❌ Ejecutable EXCLUIDO del instalador (Incorrecto)
)

echo.
echo ========================================
echo    RESUMEN DE CONFIGURACION
echo ========================================
echo.

REM Contar configuraciones correctas
set /a correctas=0

findstr /C:"RemovePreviousVersions" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"TRUE" >nul && set /a correctas+=1
findstr /C:"DetectNewerInstalledVersion" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"TRUE" >nul && set /a correctas+=1
findstr /C:"UpgradeCode" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" >nul && set /a correctas+=1
findstr /C:"CentroEmpleado.db" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"Exclude.*TRUE" >nul && set /a correctas+=1
findstr /C:"Centro-Empleado.exe.config" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"Exclude.*TRUE" >nul && set /a correctas+=1
findstr /C:"Centro-Empleado.exe" "Centro-Empleados-Setup\Centro-Empleados-Setup.vdproj" | findstr /C:"Exclude.*FALSE" >nul && set /a correctas+=1

echo Configuraciones correctas: %correctas% de 6
echo.

if %correctas% equ 6 (
    echo ✅ CONFIGURACION PERFECTA
    echo El instalador esta configurado para:
    echo - Actualizar automaticamente sin desinstalar
    echo - Preservar la base de datos existente
    echo - Preservar la configuracion existente
    echo - Actualizar solo el ejecutable y archivos necesarios
) else (
    echo ❌ CONFIGURACION INCOMPLETA
    echo Faltan %correctas% configuraciones correctas
    echo Por favor, revise la configuracion del instalador
)

echo.
echo Presione cualquier tecla para continuar...
pause >nul
