# INSTRUCTIVO COMPLETO DEL INSTALADOR SQLite

## 📋 REQUISITOS DEL SISTEMA
- Windows 7 o superior
- .NET Framework 4.7.2 o superior
- **NO requiere SQL Server**
- **NO requiere configuración de base de datos**

## 🚀 INSTALACIÓN AUTOMÁTICA

### 1. ARCHIVOS DEL INSTALADOR
El instalador debe incluir estos archivos esenciales:

```
Centro-Empleado.exe              # Aplicación principal
CentroEmpleado.exe.config        # Configuración
System.Data.SQLite.dll           # Motor SQLite
System.Data.SQLite.EF6.dll       # Entity Framework
System.Data.SQLite.Core.dll      # Núcleo SQLite
System.Data.SQLite.Linq.dll      # LINQ support
create_database.sql              # Script de creación
setup_database.bat               # Script de verificación
```

### 2. PROCESO DE INSTALACIÓN
1. **Ejecutar el instalador** como administrador
2. **Seleccionar carpeta de destino** (recomendado: `C:\Program Files\Centro-Empleado\`)
3. **Instalar** - todos los archivos se copian automáticamente
4. **Finalizar** - la aplicación está lista para usar

### 3. PRIMERA EJECUCIÓN
- Al ejecutar `CentroEmpleado.exe` por primera vez:
  - ✅ Se crea automáticamente `CentroEmpleado.db`
  - ✅ Se crean las tablas: `Afiliado`, `Familiar`, `Recetario`
  - ✅ Se configuran los índices para optimizar rendimiento
  - ✅ La aplicación está lista para usar

## 🔧 CONFIGURACIÓN AUTOMÁTICA

### Base de Datos
- **Ubicación**: `[Carpeta Instalación]\CentroEmpleado.db`
- **Tamaño inicial**: ~50 KB
- **Creación**: Automática al primer uso
- **Backup**: Copiar el archivo `.db`

### Tablas Creadas
```sql
-- Tabla Afiliado
CREATE TABLE Afiliado (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ApellidoNombre TEXT NOT NULL,
    DNI TEXT NOT NULL UNIQUE,
    Empresa TEXT NOT NULL,
    TieneGrupoFamiliar INTEGER NOT NULL
);

-- Tabla Familiar
CREATE TABLE Familiar (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdAfiliadoTitular INTEGER NOT NULL,
    Nombre TEXT NOT NULL,
    DNI TEXT NOT NULL,
    FOREIGN KEY (IdAfiliadoTitular) REFERENCES Afiliado(Id)
);

-- Tabla Recetario
CREATE TABLE Recetario (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    NumeroTalonario INTEGER NOT NULL UNIQUE,
    IdAfiliado INTEGER NOT NULL,
    FechaEmision DATE NOT NULL,
    FechaVencimiento DATE NOT NULL,
    FOREIGN KEY (IdAfiliado) REFERENCES Afiliado(Id)
);
```

## 📁 ESTRUCTURA DE CARPETAS FINAL

```
C:\Program Files\Centro-Empleado\
├── Centro-Empleado.exe
├── Centro-Empleado.exe.config
├── CentroEmpleado.db (se crea automáticamente)
├── System.Data.SQLite.dll
├── System.Data.SQLite.EF6.dll
├── System.Data.SQLite.Core.dll
├── System.Data.SQLite.Linq.dll
├── create_database.sql
├── setup_database.bat
└── README_INSTALADOR.md
```

## ✅ VENTAJAS DE SQLite

| Característica | Beneficio |
|----------------|-----------|
| **Sin servidor** | No requiere instalación de SQL Server |
| **Portable** | Se incluye en el instalador |
| **Ligera** | Archivo de base de datos muy pequeño |
| **Automática** | Se crea y configura sola |
| **Sin configuración** | Funciona inmediatamente |
| **Multiplataforma** | Compatible con cualquier Windows |

## 🚨 SOLUCIÓN DE PROBLEMAS

### Error: "No se puede conectar a la base de datos"
**Solución**: Verificar que todos los archivos SQLite estén en la carpeta de instalación

### Error: "Base de datos no encontrada"
**Solución**: Ejecutar la aplicación como administrador la primera vez

### Error: "Permisos insuficientes"
**Solución**: Instalar en `C:\Program Files\` con permisos de administrador

## 📞 SOPORTE TÉCNICO

### Verificación Rápida
1. Ejecutar `setup_database.bat`
2. Verificar que todos los archivos SQLite estén presentes
3. Ejecutar `Centro-Empleado.exe` como administrador

### Logs de Error
- Los errores se registran en la consola de la aplicación
- Verificar permisos de escritura en la carpeta de instalación
- Asegurar que .NET Framework esté instalado

## 🎯 RESUMEN

**SQLite es la opción perfecta** para tu aplicación porque:
- ✅ **No requiere servidor externo**
- ✅ **Se incluye en el instalador**
- ✅ **Creación automática de base de datos**
- ✅ **Configuración automática de tablas**
- ✅ **Funciona inmediatamente después de la instalación**
- ✅ **Base de datos ligera y portable**

**La aplicación está completamente lista** para ser distribuida con el instalador. Los usuarios solo necesitan ejecutar el instalador y la aplicación funcionará automáticamente sin configuración adicional.
