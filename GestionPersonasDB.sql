CREATE DATABASE GestionPersonasDB;
GO
USE GestionPersonasDB;
GO

-- 1. Crear Tabla Principal: Personas
CREATE TABLE Personas (
    Id INT IDENTITY(1,1) NOT NULL,
    DocumentoIdentidad NVARCHAR(50) NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    FechaNacimiento DATETIME2 NOT NULL,
    CONSTRAINT PK_Personas PRIMARY KEY (Id),
    CONSTRAINT UQ_Personas_DocumentoIdentidad UNIQUE (DocumentoIdentidad) -- Regla I
);
GO

-- 2. Crear Tabla: Personas_Telefonos
CREATE TABLE Personas_Telefonos (
    Id INT IDENTITY(1,1) NOT NULL,
    PersonaId INT NOT NULL, -- Clave Foránea apuntando al Id autoincremental de Personas
    Telefono NVARCHAR(20) NOT NULL,
    CONSTRAINT PK_Personas_Telefonos PRIMARY KEY (Id),
    CONSTRAINT FK_Personas_Telefonos_Personas FOREIGN KEY (PersonaId) 
        REFERENCES Personas (Id) ON DELETE CASCADE
);
GO

-- 3. Crear Tabla: Personas_Correos
CREATE TABLE Personas_Correos (
    Id INT IDENTITY(1,1) NOT NULL,
    PersonaId INT NOT NULL, -- Clave Foránea apuntando al Id autoincremental de Personas
    Correo NVARCHAR(150) NOT NULL,
    CONSTRAINT PK_Personas_Correos PRIMARY KEY (Id),
    CONSTRAINT FK_Personas_Correos_Personas FOREIGN KEY (PersonaId) 
        REFERENCES Personas (Id) ON DELETE CASCADE
);
GO

-- 4. Crear Tabla: Personas_Direcciones
CREATE TABLE Personas_Direcciones (
    Id INT IDENTITY(1,1) NOT NULL,
    PersonaId INT NOT NULL, -- Clave Foránea apuntando al Id autoincremental de Personas
    Direccion NVARCHAR(250) NOT NULL,
    CONSTRAINT PK_Personas_Direcciones PRIMARY KEY (Id),
    CONSTRAINT FK_Personas_Direcciones_Personas FOREIGN KEY (PersonaId) 
        REFERENCES Personas (Id) ON DELETE CASCADE
);
GO

-- Índices de rendimiento
CREATE NONCLUSTERED INDEX IX_Telefonos_PersonaId ON Personas_Telefonos(PersonaId);
CREATE NONCLUSTERED INDEX IX_Correos_PersonaId ON Personas_Correos(PersonaId);
CREATE NONCLUSTERED INDEX IX_Direcciones_PersonaId ON Personas_Direcciones(PersonaId);
GO