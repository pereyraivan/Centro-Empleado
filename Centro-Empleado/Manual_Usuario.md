# Manual de Usuario - Centro de Empleados de Comercio

## Índice
1. [Introducción](#introducción)
2. [Instalación](#instalación)
3. [Funcionalidades Principales](#funcionalidades-principales)
4. [Gestión de Afiliados](#gestión-de-afiliados)
5. [Impresión de Recetas](#impresión-de-recetas)
6. [Sistema de Bonos](#sistema-de-bonos)
7. [Control de Caja](#control-de-caja)
8. [Casos de Prueba](#casos-de-prueba)
9. [Solución de Problemas](#solución-de-problemas)

---

## Introducción

El Sistema Centro de Empleados de Comercio es una aplicación desarrollada para gestionar afiliados, imprimir recetas médicas y controlar bonos de cobro para atención médica.

### Características Principales
- ✅ Gestión completa de afiliados
- ✅ Sistema de recetas mensuales (3 por afiliado por mes)
- ✅ Sistema de recetas extraordinarias (1 por afiliado por mes)
- ✅ Sistema de bonos de cobro
- ✅ Control de caja diaria
- ✅ Base de datos SQLite integrada
- ✅ Interfaz intuitiva y fácil de usar
- ✅ Historial completo de recetas extraordinarias

---

## Instalación

### Requisitos del Sistema
- Windows 7 o superior
- .NET Framework 4.7.2 o superior
- 100 MB de espacio en disco
- Impresora configurada

### Pasos de Instalación
1. Ejecutar el archivo `Centro-Empleado-Setup.exe`
2. Seguir las instrucciones del asistente de instalación
3. Seleccionar la carpeta de destino
4. Completar la instalación
5. Ejecutar la aplicación desde el menú Inicio

---

## Funcionalidades Principales

### Panel de Operaciones
El sistema incluye un panel de operaciones con las siguientes opciones:

#### 🔍 **Ver Caja**
- Permite visualizar el control de caja diaria
- Muestra bonos generados por rango de fechas
- Exporta datos a CSV

#### 🖨️ **Imprimir Cupón**
- Genera bonos de cobro para atención médica
- Busca afiliados por DNI
- Imprime orden de consulta

#### ⚠️ **Receta Extraordinaria**
- Permite solicitar recetas extraordinarias para casos especiales
- Requiere motivo obligatorio
- Límite de 1 receta extraordinaria por afiliado por mes

#### 📋 **Historial Extraordinarias**
- Muestra el historial completo de recetas extraordinarias
- Incluye fecha, número de recetario y motivo
- Solo disponible para afiliados seleccionados

#### 📖 **Manual**
- Descarga el manual de usuario en PDF
- Acceso rápido a la documentación

---

## Gestión de Afiliados

### Agregar Nuevo Afiliado
1. Hacer clic en **"Nuevo Afiliado"**
2. Completar los campos obligatorios:
   - **DNI**: Número de documento (obligatorio, solo números)
   - **Número de Afiliado**: Número único del afiliado (obligatorio)
   - **Apellido y Nombre**: Nombre completo del afiliado
   - **Empresa**: Empresa donde trabaja
3. Hacer clic en **"Guardar"**

### Buscar Afiliado
1. Ingresar el DNI en el campo de búsqueda
2. Hacer clic en **"Buscar"**
3. Los datos del afiliado se cargarán automáticamente

### Editar Afiliado
1. Buscar el afiliado por DNI
2. Modificar los campos necesarios
3. Hacer clic en **"Guardar"**

### Eliminar Afiliado
1. Buscar el afiliado por DNI
2. Hacer clic en **"Eliminar"**
3. Confirmar la eliminación

---

## Sistema de Recetas

### Recetas Mensuales (3 por mes)
1. Buscar el afiliado por DNI
2. Hacer clic en **"Imprimir"**
3. Seleccionar cantidad de recetas (1, 2 o 3)
4. Elegir formato de impresión:
   - **1 por hoja**: Una receta por página
   - **2 por hoja**: Dos recetas por página
5. Confirmar la impresión
6. Las recetas se abrirán en el navegador

### Recetas Extraordinarias (1 por mes)
1. Seleccionar un afiliado de la lista
2. Hacer clic en **"⚠️ Receta Extraordinaria"** en el panel de operaciones
3. Ingresar motivo obligatorio
4. Hacer clic en **"Aprobar"**
5. La receta se generará automáticamente

### Límites del Sistema
- **Recetas mensuales**: Máximo 3 por afiliado por mes
- **Recetas extraordinarias**: Máximo 1 por afiliado por mes
- **Control automático**: El sistema verifica límites antes de permitir impresión
- **Reinicio mensual**: Los contadores se reinician automáticamente

### Formato de Receta
La receta incluye:
- Logo de la institución
- Datos del afiliado
- Número de receta único
- Fecha de emisión y vencimiento
- Espacios para medicamentos y cantidades

### Historial de Recetas Extraordinarias
1. Seleccionar un afiliado de la lista
2. Hacer clic en **"📋 Historial Extraordinarias"** en el panel de operaciones
3. Ver el historial completo que incluye:
   - **Fecha y hora** de cada receta extraordinaria
   - **Número de recetario** generado
   - **Motivo** de la solicitud
4. Usar **"Actualizar"** para refrescar la información
5. Hacer clic en **"Cerrar"** para salir

### Indicadores en la Lista de Afiliados
La grilla principal muestra:
- **Columna "Rec. Extraordinaria"**: Indica "Sí" si el afiliado imprimió una receta extraordinaria este mes
- **Columna "Próxima habilitación"**: Muestra cuándo se habilitará el próximo recetario
- **Columna "Última impresión"**: Fecha de la última receta impresa

---

## Sistema de Bonos

### Generar Bono de Cobro
1. Hacer clic en **"Imprimir Cupón"** en el panel de operaciones
2. Ingresar el DNI del afiliado
3. Hacer clic en **"Buscar"**
4. Completar los campos:
   - **Monto**: Cantidad a cobrar (obligatorio)
   - **Concepto**: Descripción del servicio
   - **Observaciones**: Notas adicionales
5. Hacer clic en **"Imprimir Bono"**

### Formato del Bono
El bono incluye:
- Logo de la institución
- Número de orden único
- Datos del beneficiario
- Detalle del servicio y monto
- Espacios para firma del médico y beneficiario

### Numeración Automática
- Los bonos se numeran automáticamente (BON-000001, BON-000002, etc.)
- Cada bono tiene un número único
- Se registra en la base de datos para control

---

## Control de Caja

### Ver Caja Diaria
1. Hacer clic en **"Ver Caja"** en el panel de operaciones
2. Seleccionar rango de fechas
3. Hacer clic en **"Buscar"**

### Información Mostrada
- **Fecha**: Fecha de emisión del bono
- **Número de Bono**: Identificador único
- **Afiliado**: Nombre del beneficiario
- **DNI**: Documento del afiliado
- **Monto**: Cantidad cobrada
- **Concepto**: Descripción del servicio

### Exportar Datos
1. Hacer clic en **"Exportar a CSV"**
2. Seleccionar ubicación para guardar el archivo
3. El archivo se puede abrir en Excel

---

## Casos de Prueba

### Prueba de Receta Mensual
1. **Crear afiliado de prueba**:
   - DNI: 12345678
   - Nombre: Juan Pérez
   - Empresa: Empresa Test

2. **Imprimir primera receta**:
   - Buscar por DNI: 12345678
   - Imprimir receta
   - Verificar que se genere correctamente

3. **Intentar segunda receta**:
   - Intentar imprimir otra receta
   - Verificar que el sistema no permita imprimir

4. **Esperar al mes siguiente**:
   - Cambiar la fecha del sistema al mes siguiente
   - Verificar que se pueda imprimir nuevamente

### Prueba de Bono
1. **Generar bono**:
   - Buscar afiliado por DNI
   - Ingresar monto: $1000
   - Concepto: Consulta médica
   - Imprimir bono

2. **Verificar en caja**:
   - Ir a "Ver Caja"
   - Buscar por fecha actual
   - Verificar que aparezca el bono generado

---

## Solución de Problemas

### Problema: No se muestra el logo
**Solución**:
- Verificar que el archivo `logo_cec1.png` esté en la carpeta de la aplicación
- Reiniciar la aplicación

### Problema: Error al imprimir
**Solución**:
- Verificar que la impresora esté conectada y encendida
- Verificar que el navegador predeterminado esté configurado
- Intentar imprimir desde el navegador manualmente

### Problema: No encuentra afiliado
**Solución**:
- Verificar que el DNI esté ingresado correctamente
- Verificar que el afiliado exista en la base de datos
- Intentar agregar el afiliado si no existe

### Problema: Error de base de datos
**Solución**:
- Verificar que la aplicación tenga permisos de escritura
- Verificar que no haya otro proceso usando la base de datos
- Reiniciar la aplicación

### Problema: No se puede generar bono
**Solución**:
- Verificar que el afiliado exista
- Verificar que el monto sea mayor a 0
- Verificar que todos los campos obligatorios estén completos

---

## Atajos de Teclado

| Función | Atajo |
|---------|-------|
| Abrir formulario de bonos | Ctrl + B |
| Abrir control de caja | Ctrl + C |
| Buscar afiliado | Enter (en campo DNI) |
| Guardar afiliado | Ctrl + S |
| Imprimir receta | Ctrl + P |

---

## Contacto y Soporte

Para soporte técnico o consultas:
- **Institución**: Centro de Empleados de Comercio de Concepción - Aguileres
- **Dirección**: Manuela Pedraza 520, Concepción - Tucumán
- **Teléfono**: (03865) 421716
- **Código Postal**: (4146)

---

*Manual de Usuario v1.0 - Centro de Empleados de Comercio*
