@echo off
echo Abriendo proyecto en Visual Studio...

REM Buscar Visual Studio Community 2022
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" (
    start "" "%ProgramFiles%\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" "Centro-Empleado.sln"
    goto :end
)

REM Buscar Visual Studio Community 2019
if exist "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe" (
    start "" "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Community\Common7\IDE\devenv.exe" "Centro-Empleado.sln"
    goto :end
)

REM Buscar Visual Studio Professional
if exist "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe" (
    start "" "%ProgramFiles%\Microsoft Visual Studio\2022\Professional\Common7\IDE\devenv.exe" "Centro-Empleado.sln"
    goto :end
)

REM Si no encuentra Visual Studio, intentar abrir con el programa predeterminado
echo No se encontró Visual Studio, intentando abrir con programa predeterminado...
start "" "Centro-Empleado.sln"

:end
