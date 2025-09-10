@echo off
echo ========================================
echo    VERIFICACION POST-INSTALACION
echo    Sistema Centro de Empleados
echo ========================================
echo.

REM Verificar si existe la base de datos
if exist "CentroEmpleado.db" (
    echo ✓ Base de datos encontrada
    echo Tamaño: 
    dir "CentroEmpleado.db" | findstr CentroEmpleado
) else (
    echo ✗ ERROR: No se encontro la base de datos
    echo.
    echo Buscando respaldos disponibles...
    if exist "Backups" (
        echo Respaldos encontrados:
        dir /b "Backups\CentroEmpleado_*.db"
        echo.
        echo Si hay respaldos, contacte al soporte tecnico
        echo para restaurar los datos
    ) else (
        echo No se encontraron respaldos
        echo Contacte al soporte tecnico
    )
)

echo.
echo Verificando archivos de la aplicacion...
if exist "Centro-Empleado.exe" (
    echo ✓ Aplicacion principal encontrada
) else (
    echo ✗ ERROR: No se encontro la aplicacion principal
)

if exist "config.txt" (
    echo ✓ Archivo de configuracion encontrado
) else (
    echo ⚠ ADVERTENCIA: No se encontro archivo de configuracion
    echo Esto es normal en instalaciones nuevas
)

echo.
echo Verificando respaldos automaticos...
if exist "Backups" (
    echo ✓ Carpeta de respaldos encontrada
    echo Cantidad de respaldos:
    dir /b "Backups\CentroEmpleado_*.db" 2>nul | find /c /v ""
) else (
    echo ⚠ ADVERTENCIA: No se encontro carpeta de respaldos
    echo El sistema creara respaldos automaticamente
)

echo.
echo ========================================
echo    VERIFICACION COMPLETADA
echo ========================================
echo.
echo Si todo esta correcto, puede usar el sistema normalmente
echo Si hay errores, contacte al soporte tecnico
echo.
echo Presione cualquier tecla para continuar...
pause >nul
