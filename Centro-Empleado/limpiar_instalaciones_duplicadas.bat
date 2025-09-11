@echo off
echo ========================================
echo    LIMPIAR INSTALACIONES DUPLICADAS
echo    Sistema Centro de Empleados
echo ========================================
echo.

echo Este script le ayudara a limpiar las instalaciones duplicadas
echo del sistema Centro de Empleados.
echo.

echo ADVERTENCIA: Este proceso desinstalara TODAS las versiones
echo del sistema Centro de Empleados. Sus datos se preservaran
echo en la carpeta de instalacion.
echo.

set /p confirm="¿Desea continuar? (S/N): "

if /i not "%confirm%"=="S" (
    echo Operacion cancelada
    pause
    exit /b 0
)

echo.
echo Buscando instalaciones del sistema...

REM Buscar instalaciones usando wmic
wmic product where "name like 'Centro-Empleados-Setup'" get name,version,identifyingnumber /format:table

echo.
echo INSTRUCCIONES PARA LIMPIAR:
echo.
echo 1. Abra "Programas y caracteristicas" desde el Panel de control
echo 2. Busque todas las entradas de "Centro-Empleados-Setup"
echo 3. Desinstale TODAS las versiones (una por una)
echo 4. Reinicie la computadora
echo 5. Instale la version mas reciente del MSI
echo.

echo ALTERNATIVA AUTOMATICA:
echo Si tiene problemas, puede usar el siguiente comando:
echo.
echo wmic product where "name='Centro-Empleados-Setup'" call uninstall
echo.

echo IMPORTANTE:
echo - Sus datos estan seguros en la carpeta de instalacion
echo - La base de datos CentroEmpleado.db se preserva
echo - Los respaldos en la carpeta Backups se mantienen
echo.

echo Presione cualquier tecla para continuar...
pause >nul
