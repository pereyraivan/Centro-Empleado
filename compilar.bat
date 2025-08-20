@echo off
echo Compilando aplicación Centro-Empleado...
echo.

REM Intentar compilar con Visual Studio
echo Buscando Visual Studio...
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" (
    "%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" Centro-Empleado.sln /p:Configuration=Debug /p:Platform="Any CPU"
    goto :success
)

if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" (
    "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild.exe" Centro-Empleado.sln /p:Configuration=Debug /p:Platform="Any CPU"
    goto :success
)

REM Si no encuentra Visual Studio, intentar con .NET Framework
echo Intentando compilar con .NET Framework...
if exist "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" (
    "%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" Centro-Empleado.sln /p:Configuration=Debug
    goto :success
)

if exist "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" (
    "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" Centro-Empleado.sln /p:Configuration=Debug
    goto :success
)

echo No se pudo encontrar MSBuild. Por favor compile desde Visual Studio.
pause
goto :end

:success
echo.
echo Compilación completada!
echo El ejecutable se encuentra en: Centro-Empleado\bin\Debug\
echo.
pause

:end
