@echo off
echo ========================================
echo    LIMPIEZA DE BASE DE DATOS
echo    Centro de Empleados de Comercio
echo ========================================
echo.

REM Verificar si existe la base de datos
if not exist "bin\Debug\CentroEmpleado.db" (
    echo ERROR: No se encontró la base de datos CentroEmpleado.db
    echo Asegúrate de que la aplicación se haya ejecutado al menos una vez.
    pause
    exit /b 1
)

echo Base de datos encontrada: bin\Debug\CentroEmpleado.db
echo.

REM Crear copia de seguridad
echo Creando copia de seguridad...
copy "bin\Debug\CentroEmpleado.db" "bin\Debug\CentroEmpleado_backup_%date:~-4,4%%date:~-10,2%%date:~-7,2%_%time:~0,2%%time:~3,2%%time:~6,2%.db"
echo Copia de seguridad creada.
echo.

REM Compilar y ejecutar el programa de limpieza
echo Compilando programa de limpieza...
csc /reference:packages\System.Data.SQLite.Core.1.0.119.0\lib\net46\System.Data.SQLite.dll LimpiarBaseDatos.cs

if exist "LimpiarBaseDatos.exe" (
    echo Ejecutando limpieza...
    cd bin\Debug
    ..\..\LimpiarBaseDatos.exe
    cd ..\..
    
    REM Limpiar archivo temporal
    del LimpiarBaseDatos.exe
) else (
    echo ERROR: No se pudo compilar el programa de limpieza.
    echo Verifica que el compilador de C# esté disponible.
)

pause
