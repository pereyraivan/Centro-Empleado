# 🔄 Guía de Actualización del Sistema Centro de Empleados

## 📋 Resumen
Esta guía explica cómo actualizar el sistema sin perder datos de la base de datos.

## 🛡️ Sistema de Respaldo Automático

### Archivos de Respaldo
- **Ubicación**: Carpeta `Backups/` dentro del directorio de la aplicación
- **Formato**: `CentroEmpleado_YYYY-MM-DD_HH-mm-ss.db`
- **Ejemplo**: `CentroEmpleado_2024-01-15_14-30-25.db`

### Crear Respaldo Manual
1. **Desde la aplicación**: Botón "💾 Crear Respaldo" en el panel de operaciones
2. **Desde línea de comandos**: Ejecutar `backup_database.bat`

## 🔄 Proceso de Actualización Segura

### Método 1: Actualización Automática (Recomendado)
1. **Ejecutar script de actualización**:
   ```batch
   actualizar_sistema.bat
   ```

2. **Seguir las instrucciones**:
   - El script creará un respaldo automático
   - Copiar archivos de la nueva versión en `NuevaVersion/`
   - El script reemplazará archivos (excepto la base de datos)

### Método 2: Actualización Manual
1. **Crear respaldo**:
   ```batch
   backup_database.bat
   ```

2. **Detener la aplicación** (cerrar completamente)

3. **Reemplazar archivos**:
   - Copiar todos los archivos de la nueva versión
   - **NO reemplazar** `CentroEmpleado.db`
   - **NO reemplazar** `config.txt` (si existe)

4. **Iniciar la nueva versión**

## 🔧 Restauración de Respaldo

### Si algo sale mal durante la actualización:
1. **Ejecutar script de restauración**:
   ```batch
   restaurar_respaldo.bat
   ```

2. **Seleccionar el respaldo** a restaurar de la lista

3. **Confirmar la restauración**

## 📁 Estructura de Archivos Importantes

### Archivos que SE DEBEN preservar:
- `CentroEmpleado.db` - Base de datos principal
- `config.txt` - Configuración de contraseñas
- `Backups/` - Carpeta de respaldos

### Archivos que se pueden reemplazar:
- `Centro-Empleado.exe` - Aplicación principal
- `*.dll` - Bibliotecas del sistema
- `Resources/` - Recursos de la aplicación
- `Manual_Usuario.html` - Manual de usuario

## ⚠️ Precauciones Importantes

### Antes de actualizar:
1. **Crear respaldo** de la base de datos
2. **Verificar** que la nueva versión es compatible
3. **Cerrar** completamente la aplicación actual

### Durante la actualización:
1. **No interrumpir** el proceso de copia de archivos
2. **Verificar** que no se reemplaza la base de datos
3. **Mantener** los archivos de configuración

### Después de actualizar:
1. **Probar** la nueva versión con datos de prueba
2. **Verificar** que todos los datos están intactos
3. **Crear** un nuevo respaldo de la versión actualizada

## 🚨 Recuperación de Emergencia

### Si se pierde la base de datos:
1. **Buscar** en la carpeta `Backups/` el respaldo más reciente
2. **Copiar** el archivo de respaldo como `CentroEmpleado.db`
3. **Reiniciar** la aplicación

### Si no hay respaldos:
1. **Verificar** si existe `CentroEmpleado.db.bak` (respaldo automático)
2. **Renombrar** el archivo `.bak` a `.db`
3. **Contactar** al soporte técnico

## 📞 Soporte Técnico

### Información útil para reportar problemas:
- Versión anterior y nueva del sistema
- Fecha y hora del problema
- Archivos de respaldo disponibles
- Mensajes de error específicos

### Archivos de log importantes:
- `Backups/` - Historial de respaldos
- `config.txt` - Configuración del sistema
- Logs de Windows (si aplica)

---

## ✅ Checklist de Actualización

- [ ] Respaldo creado antes de actualizar
- [ ] Aplicación cerrada completamente
- [ ] Archivos de nueva versión copiados
- [ ] Base de datos NO reemplazada
- [ ] Configuración preservada
- [ ] Nueva versión probada
- [ ] Datos verificados
- [ ] Nuevo respaldo creado

---

*Esta guía debe mantenerse actualizada con cada nueva versión del sistema.*
