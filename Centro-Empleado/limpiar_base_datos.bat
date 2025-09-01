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

REM Ejecutar script de limpieza
echo Ejecutando limpieza de datos...
sqlite3 "bin\Debug\CentroEmpleado.db" < "limpiar_base_datos.sql"

if %errorlevel% equ 0 (
    echo.
    echo ========================================
    echo    LIMPIEZA COMPLETADA EXITOSAMENTE
    echo ========================================
    echo.
    echo La base de datos ha sido limpiada.
    echo Todos los datos han sido eliminados.
    echo La estructura de las tablas se mantiene.
    echo.
    echo Ahora puedes crear el instalador.
    echo.
) else (
    echo.
    echo ========================================
    echo    ERROR EN LA LIMPIEZA
    echo ========================================
    echo.
    echo Ocurrió un error durante la limpieza.
    echo Verifica que SQLite3 esté instalado.
    echo.
)

pause
