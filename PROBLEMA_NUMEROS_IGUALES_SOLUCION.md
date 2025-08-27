# Problema: Números Iguales en Recetarios - Solución

## Problema Identificado

### ❌ Síntoma
- Ambos recetarios en la misma hoja muestran el mismo número (ej: "000049")
- Los números deberían ser consecutivos (ej: "000049" y "000050")

### 🔍 Análisis del Problema

El problema estaba en la lógica de reemplazo de placeholders en el HTML:

1. **Método anterior:** `ReemplazarPlaceholders()` con sufijos dinámicos
2. **Problema:** Los placeholders se sobrescribían entre sí
3. **Causa:** El método no manejaba correctamente los placeholders separados

## Solución Implementada

### ✅ Corrección Principal

**Archivo:** `RecetarioManager.cs`

#### Cambios Realizados:

1. **Separación de métodos:**
   - `ReemplazarPlaceholdersPrimeraReceta()` - Maneja placeholders sin sufijo
   - `ReemplazarPlaceholdersSegundaReceta()` - Maneja placeholders con sufijo "_2"

2. **Lógica mejorada:**
   - Cada método reemplaza solo sus placeholders específicos
   - No hay interferencia entre placeholders de diferentes recetas

#### Código Corregido:

```csharp
private string ReemplazarPlaceholdersPrimeraReceta(string html, Recetario recetario, Afiliado afiliado)
{
    // Reemplazar placeholders de la primera receta (sin sufijo)
    html = html.Replace("{{NUMERO_TALONARIO}}", recetario.NumeroTalonario.ToString("D6"));
    // ... otros placeholders
    return html;
}

private string ReemplazarPlaceholdersSegundaReceta(string html, Recetario recetario, Afiliado afiliado)
{
    // Reemplazar placeholders de la segunda receta (con sufijo _2)
    html = html.Replace("{{NUMERO_TALONARIO_2}}", recetario.NumeroTalonario.ToString("D6"));
    // ... otros placeholders
    return html;
}
```

### ✅ Logging de Depuración

**Archivo:** `frmAfiliado.cs` y `RecetarioManager.cs`

#### Agregado:
- Mensajes de debug para verificar números generados
- Verificación de números en cada paso del proceso
- Confirmación de placeholders reemplazados

## Verificación de la Solución

### Pasos para Verificar:

1. **Compilar** el proyecto
2. **Ejecutar** la aplicación
3. **Seleccionar** un afiliado
4. **Imprimir** recetarios
5. **Verificar** los mensajes de debug:
   - Números obtenidos de la base de datos
   - Recetarios generados en memoria
   - Procesamiento de cada receta

### Resultado Esperado:

- ✅ **Recetario 1:** Número consecutivo (ej: 000049)
- ✅ **Recetario 2:** Número consecutivo siguiente (ej: 000050)
- ✅ **Números únicos:** Cada receta tiene un número diferente
- ✅ **Secuencial:** Los números son consecutivos

## Archivos Modificados

- `Centro-Empleado/RecetarioManager.cs`
- `Centro-Empleado/frmAfiliado.cs`

## Notas Técnicas

- Los placeholders en el HTML deben ser exactos: `{{NUMERO_TALONARIO}}` y `{{NUMERO_TALONARIO_2}}`
- El método de reemplazo es sensible a mayúsculas/minúsculas
- Se mantiene la funcionalidad de logo y otros placeholders
- El logging se puede remover en producción

## Próximos Pasos

1. **Probar** la solución con diferentes escenarios
2. **Verificar** que funciona para grupos familiares (4 recetas)
3. **Remover** logging de debug una vez confirmado que funciona
4. **Documentar** el comportamiento esperado
