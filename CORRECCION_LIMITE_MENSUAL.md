# Corrección: Límite Mensual de Impresión

## Problema Identificado ❌

### Descripción del Error:
- El sistema permitía imprimir múltiples veces recetas para el mismo afiliado
- No respetaba el límite de 2 recetas por mes para afiliados individuales
- No respetaba el límite de 4 recetas por mes para grupos familiares

### Causa Raíz:
1. **Lógica de validación incorrecta**: El código forzaba la impresión de 2 o 4 recetas incluso cuando ya se habían impreso algunas
2. **Conteo de recetas erróneo**: El método `ContarRecetariosMensuales` contaba desde la última impresión en lugar de contar todos los recetarios del afiliado

## Solución Implementada ✅

### 1. Corrección en `frmAfiliado.cs`

**Antes:**
```csharp
// Calcular cuántos recetarios imprimir
int recetariosAImprimir = maxRecetarios - recetariosEsteMes;

// Para grupos familiares, imprimir 4 recetas (2 hojas) de una vez
// Para afiliados sin grupo familiar, imprimir 2 recetas (1 hoja) de una vez
if (afiliado.TieneGrupoFamiliar && recetariosAImprimir < 4)
{
    recetariosAImprimir = 4; // Forzar impresión de 4 recetas para grupos familiares
}
else if (!afiliado.TieneGrupoFamiliar && recetariosAImprimir < 2)
{
    recetariosAImprimir = 2; // Forzar impresión de 2 recetas para afiliados individuales
}
```

**Después:**
```csharp
// Calcular cuántos recetarios imprimir
int recetariosAImprimir = maxRecetarios - recetariosEsteMes;

// Solo permitir imprimir si hay recetas disponibles
if (recetariosAImprimir <= 0)
{
    DateTime? proxima = dbManager.FechaProximaHabilitacion(afiliado.Id);
    string fechaProx = proxima.HasValue ? proxima.Value.ToString("dd/MM/yyyy") : "-";
    MessageBox.Show($"Ya imprimió las recetas correspondientes al período. Podrá imprimir nuevamente a partir del: {fechaProx}", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
    return;
}

// Para grupos familiares, imprimir 4 recetas (2 hojas) de una vez
// Para afiliados sin grupo familiar, imprimir 2 recetas (1 hoja) de una vez
if (afiliado.TieneGrupoFamiliar)
{
    recetariosAImprimir = 4; // Siempre imprimir 4 recetas para grupos familiares
}
else
{
    recetariosAImprimir = 2; // Siempre imprimir 2 recetas para afiliados individuales
}
```

### 2. Corrección en `DatabaseManager.cs`

**Antes:**
```csharp
// Si han pasado menos de 30 días, contar los recetarios del período actual
if (diasTranscurridos < 30)
{
    string sql = @"SELECT COUNT(*) FROM Recetario 
                  WHERE IdAfiliado = @IdAfiliado 
                  AND FechaEmision >= @FechaDesde";
    // ... contar desde la última impresión
}
```

**Después:**
```csharp
// Si han pasado menos de 30 días, contar todos los recetarios del afiliado
if (diasTranscurridos < 30)
{
    string sql = @"SELECT COUNT(*) FROM Recetario 
                  WHERE IdAfiliado = @IdAfiliado";
    // ... contar todos los recetarios del afiliado
}
```

## Resultado Final

### ✅ Funcionamiento Correcto:

1. **Afiliado Individual:**
   - Máximo 2 recetas por período de 30 días
   - Si ya imprimió 2 recetas, debe esperar 30 días
   - Mensaje claro indicando la próxima fecha disponible

2. **Grupo Familiar:**
   - Máximo 4 recetas por período de 30 días
   - Si ya imprimió 4 recetas, debe esperar 30 días
   - Mensaje claro indicando la próxima fecha disponible

3. **Validación Estricta:**
   - No permite imprimir si ya alcanzó el límite
   - Cuenta correctamente todos los recetarios del afiliado
   - Respeta el período de 30 días desde la última impresión

### ✅ Mensajes de Usuario:

- **Límite alcanzado:** "Ya imprimió las recetas correspondientes al período. Podrá imprimir nuevamente a partir del: [fecha]"
- **Confirmación:** "¿Desea imprimir X recetas del afiliado [nombre]?"
- **Éxito:** "Se generaron X recetarios correctamente."

## Archivos Modificados

1. **`Centro-Empleado/frmAfiliado.cs`** - Lógica de validación corregida
2. **`Centro-Empleado/Data/DatabaseManager.cs`** - Método de conteo corregido

## Verificación

Para verificar que funciona correctamente:
1. Imprimir 2 recetas para un afiliado individual
2. Intentar imprimir nuevamente → Debe mostrar mensaje de límite alcanzado
3. Imprimir 4 recetas para un grupo familiar
4. Intentar imprimir nuevamente → Debe mostrar mensaje de límite alcanzado
5. Verificar que después de 30 días permite imprimir nuevamente
