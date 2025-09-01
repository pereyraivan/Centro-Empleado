# Limpieza de Base de Datos para Instalador

## 📋 Descripción
Este documento explica cómo limpiar todos los datos de la base de datos antes de crear el instalador del sistema.

## 🎯 Propósito
- Eliminar todos los datos de prueba
- Mantener la estructura de las tablas
- Preparar la base de datos para distribución

## 🛠️ Métodos Disponibles

### 1. **Combinación de Teclas Secreta (Recomendado)**
Usar la combinación de teclas secreta:

1. **Abrir la aplicación** Centro-Empleado
2. **Presionar** `Ctrl + Shift + L` simultáneamente
3. **Confirmar** la acción cuando aparezca el mensaje de advertencia
4. **Esperar** a que se complete la limpieza

**Nota**: Esta función está oculta del usuario final y solo debe ser utilizada por desarrolladores o administradores del sistema.

### 2. **Script SQL (Alternativo)**
Si prefieres usar SQLite directamente:

1. **Abrir SQLite Browser** o cualquier cliente SQLite
2. **Conectar** a la base de datos `CentroEmpleado.db`
3. **Ejecutar** el script `limpiar_base_datos.sql`

### 3. **Script Batch (Alternativo)**
Si tienes SQLite3 instalado:

1. **Abrir** el archivo `limpiar_base_datos.bat`
2. **Seguir** las instrucciones en pantalla

## 📊 Datos que se Eliminan

### Tablas Afectadas:
- **Afiliado**: Todos los afiliados registrados
- **Familiar**: Todos los familiares asociados
- **Recetario**: Todos los recetarios emitidos
- **Bono**: Todos los bonos de cobro generados

### Contadores Reiniciados:
- Auto-incremento de todas las tablas
- Numeración de recetarios
- Numeración de bonos

## ⚠️ Advertencias Importantes

### Antes de Limpiar:
- ✅ **Crear copia de seguridad** (se hace automáticamente)
- ✅ **Verificar** que no hay datos importantes
- ✅ **Cerrar** la aplicación si está abierta

### Después de Limpiar:
- ✅ **Verificar** que las tablas están vacías
- ✅ **Probar** que la aplicación funciona correctamente
- ✅ **Crear** el instalador

## 🔄 Proceso de Limpieza

### Paso a Paso:
1. **Desactivar** verificación de claves foráneas
2. **Eliminar** todos los registros de cada tabla
3. **Reiniciar** contadores de auto-incremento
4. **Reactivar** verificación de claves foráneas
5. **Confirmar** transacción

### Verificación:
- Contar registros en cada tabla
- Verificar que los contadores se reiniciaron
- Probar funcionalidad básica

## 📁 Archivos Relacionados

### Scripts de Limpieza:
- `limpiar_base_datos.sql` - Script SQL principal
- `limpiar_base_datos.bat` - Script batch con SQLite3
- `limpiar_base_datos_simple.bat` - Script batch alternativo
- `LimpiarBaseDatos.cs` - Programa C# independiente

### Archivos de la Aplicación:
- `DatabaseManager.cs` - Método `LimpiarBaseDatos()`
- `frmAfiliado.cs` - Función secreta `LimpiarBaseDatosSecreta()` con combinación `Ctrl+Shift+L`

## 🚀 Crear el Instalador

### Después de Limpiar:
1. **Verificar** que la base de datos está limpia
2. **Comprimir** todos los archivos necesarios
3. **Crear** el instalador con tu herramienta preferida
4. **Probar** la instalación en un sistema limpio

### Archivos a Incluir:
- `Centro-Empleado.exe` - Aplicación principal
- `CentroEmpleado.db` - Base de datos limpia
- `Manual_Usuario.html` - Manual de usuario
- `bonoFinal.html` - Plantilla de bonos
- `recetaFinal.html` - Plantilla de recetas
- DLLs de SQLite y dependencias

## 🔧 Solución de Problemas

### Error: "Base de datos no encontrada"
- Verificar que la aplicación se ejecutó al menos una vez
- Buscar el archivo `CentroEmpleado.db` en `bin\Debug\`

### Error: "No se pueden eliminar registros"
- Verificar permisos de escritura
- Cerrar la aplicación si está abierta
- Verificar que no hay otros procesos usando la DB

### Error: "Transacción fallida"
- Verificar integridad de la base de datos
- Intentar recrear las tablas si es necesario

## 📞 Soporte

Si encuentras problemas durante la limpieza:
1. **Revisar** los mensajes de error
2. **Verificar** que todos los archivos están presentes
3. **Contactar** al equipo de desarrollo

---

## 🔐 Combinaciones de Teclas Secretas

### Para Desarrolladores:
- **`Ctrl + Shift + L`** - Limpiar base de datos (función de desarrollo)
- **`Ctrl + P`** - Abrir herramientas de pruebas
- **`Ctrl + B`** - Abrir formulario de bonos
- **`Ctrl + C`** - Abrir control de caja
- **`F5`** - Recargar datos desde la base de datos

### Para Usuarios Finales:
- **`Enter`** - Buscar afiliado (en campo DNI)
- **`Ctrl + S`** - Guardar afiliado

---

**Nota**: La limpieza de la base de datos es irreversible. Asegúrate de tener una copia de seguridad antes de proceder.
