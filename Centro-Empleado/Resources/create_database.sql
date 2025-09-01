-- Script de creación automática de la base de datos CentroEmpleado
-- Este archivo se ejecuta automáticamente al iniciar la aplicación

-- Crear tabla Afiliado
CREATE TABLE IF NOT EXISTS Afiliado (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ApellidoNombre TEXT NOT NULL,
    DNI TEXT NOT NULL UNIQUE,
    Empresa TEXT NOT NULL,
    TieneGrupoFamiliar INTEGER NOT NULL
);

-- Crear tabla Familiar
CREATE TABLE IF NOT EXISTS Familiar (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdAfiliadoTitular INTEGER NOT NULL,
    Nombre TEXT NOT NULL,
    DNI TEXT NOT NULL,
    FOREIGN KEY (IdAfiliadoTitular) REFERENCES Afiliado(Id)
);

-- Crear tabla Recetario
CREATE TABLE IF NOT EXISTS Recetario (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    NumeroTalonario INTEGER NOT NULL UNIQUE,
    IdAfiliado INTEGER NOT NULL,
    FechaEmision DATE NOT NULL,
    FechaVencimiento DATE NOT NULL,
    FOREIGN KEY (IdAfiliado) REFERENCES Afiliado(Id)
);

-- Crear índices para mejorar el rendimiento
CREATE INDEX IF NOT EXISTS idx_afiliado_dni ON Afiliado(DNI);
CREATE INDEX IF NOT EXISTS idx_afiliado_empresa ON Afiliado(Empresa);
CREATE INDEX IF NOT EXISTS idx_recetario_afiliado ON Recetario(IdAfiliado);
CREATE INDEX IF NOT EXISTS idx_recetario_fecha ON Recetario(FechaEmision);
CREATE INDEX IF NOT EXISTS idx_recetario_numero ON Recetario(NumeroTalonario);
CREATE INDEX IF NOT EXISTS idx_familiar_afiliado ON Familiar(IdAfiliadoTitular);

-- Insertar datos de ejemplo (opcional)
-- INSERT OR IGNORE INTO Afiliado (ApellidoNombre, DNI, Empresa, TieneGrupoFamiliar) 
-- VALUES ('Ejemplo Usuario', '12345678', 'Empresa Demo', 1);
