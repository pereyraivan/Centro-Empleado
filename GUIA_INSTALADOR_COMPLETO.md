# GUÍA: Crear Instalador Completo para Centro-Empleado

## OPCIÓN RECOMENDADA: Inno Setup (GRATIS)

### PASO 1: Descargar Inno Setup
- Ve a: https://jrsoftware.org/isinfo.php
- Descarga "Inno Setup 6.x" (gratis)
- Instala en tu PC de desarrollo

### PASO 2: Preparar archivos para distribución
```
Centro-Empleado-Instalador/
├── Aplicacion/
│   ├── Centro-Empleado.exe
│   ├── Centro-Empleado.exe.config
│   ├── System.Data.SQLite.dll
│   ├── CentroEmpleado.db (base vacía)
│   └── Resources/
│       └── logo_cec.png
├── Runtimes/
│   ├── CRRuntime_64bit_13_0_XX.msi
│   └── dotnet-framework-4.7.2.exe (si es necesario)
└── setup.iss (script de Inno Setup)
```

### PASO 3: Script de Inno Setup (setup.iss)
```ini
[Setup]
AppName=Centro de Empleados - Sistema de Recetarios
AppVersion=1.0
AppPublisher=Tu Nombre/Empresa
DefaultDirName={pf}\Centro-Empleado
DefaultGroupName=Centro-Empleado
OutputDir=Output
OutputBaseFilename=Centro-Empleado-Setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Files]
; Aplicación principal
Source: "Aplicacion\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; Runtime de Crystal Reports
Source: "Runtimes\CRRuntime_64bit_13_0_XX.msi"; DestDir: "{tmp}"; Flags: deleteafterinstall

[Run]
; Instalar Crystal Reports Runtime
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\CRRuntime_64bit_13_0_XX.msi"" /quiet"; StatusMsg: "Instalando Crystal Reports Runtime..."

; Ejecutar aplicación al finalizar
Filename: "{app}\Centro-Empleado.exe"; Description: "Ejecutar Centro-Empleado"; Flags: nowait postinstall skipifsilent

[Icons]
Name: "{group}\Centro-Empleado"; Filename: "{app}\Centro-Empleado.exe"
Name: "{commondesktop}\Centro-Empleado"; Filename: "{app}\Centro-Empleado.exe"
```

## ALTERNATIVA SIMPLE: Crear batch de instalación

### install.bat
```batch
@echo off
echo Instalando Centro de Empleados - Sistema de Recetarios
echo.

echo 1. Instalando Crystal Reports Runtime...
CRRuntime_64bit_13_0_XX.msi /quiet

echo 2. Copiando archivos de aplicación...
xcopy /s /y "App\*" "C:\Program Files\Centro-Empleado\"

echo 3. Creando accesos directos...
powershell "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('$env:USERPROFILE\Desktop\Centro-Empleado.lnk'); $Shortcut.TargetPath = 'C:\Program Files\Centro-Empleado\Centro-Empleado.exe'; $Shortcut.Save()"

echo.
echo Instalación completada!
pause
```

## VERIFICACIÓN DE DEPENDENCIAS

### En tu código, agregar verificación:
```csharp
// En Program.cs o Form principal
private static bool VerificarCrystalReports()
{
    try
    {
        var assembly = System.Reflection.Assembly.LoadFrom("CrystalDecisions.CrystalReports.Engine.dll");
        return true;
    }
    catch
    {
        MessageBox.Show("Crystal Reports Runtime no está instalado.\n\n" +
                       "Por favor ejecute el instalador completo o instale:\n" +
                       "SAP Crystal Reports Runtime Engine for .NET Framework",
                       "Dependencia faltante", 
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
    }
}
```

## DISTRIBUCIÓN FINAL

### Para cada cliente necesitarás:
1. **Instalador único** (.exe) que incluya:
   - Tu aplicación
   - Crystal Reports Runtime
   - .NET Framework (si no lo tiene)
   - Base de datos SQLite vacía

2. **O instrucciones simples:**
   - "Ejecutar Setup.exe como administrador"
   - "Seguir el wizard"
   - "Listo para usar"

## VENTAJAS DEL INSTALADOR COMPLETO:
✅ Una sola instalación
✅ No requiere conocimientos técnicos del usuario
✅ Instala todas las dependencias automáticamente
✅ Crea accesos directos
✅ Configuración automática
✅ Desinstalador incluido

## TAMAÑO APROXIMADO:
- Tu aplicación: ~5MB
- Crystal Reports Runtime: ~50MB
- Instalador comprimido: ~30MB total

¿Quieres que te ayude a crear el instalador con Inno Setup?
