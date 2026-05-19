# Sistema de Gestión de Personas (.NET 10 & ADO.NET Pure)

Este proyecto corresponde a la solución práctica de la prueba técnica para Desarrolladores .NET. Implementa un sistema robusto, escalable y de alta disponibilidad para el registro y validación estricta de datos personales e información de contacto.

La arquitectura se diseñó bajo los lineamientos de **N-Capas (Separación de Responsabilidades)** utilizando las capacidades nativas más recientes de **.NET 10** y **C# 14**, prescindiendo de ORMs para garantizar el máximo rendimiento mediante **ADO.NET Transaccional Puro**.

---

## Arquitectura y Decisiones de Diseño

El sistema está estructurado en 4 proyectos independientes para garantizar el desacoplamiento físico y conceptual (Principio de Responsabilidad Única - SRP):

1. **`GestionPersonas.Domain`**: Capa central que contiene las entidades de negocio puras (`Persona`) e interfaces de contratos. Cero dependencias externas.
2. **`GestionPersonas.Application`**: Alberga las Reglas de Negocio (Validaciones estructurales, límites de contacto y control de alfabetos).
3. **`GestionPersonas.Infrastructure`**: Implementa el acceso a datos mediante conexiones nativas de SQL Server, gestionando transacciones atómicas (`SqlTransaction`) y la recuperación de identidades secuenciales mediante `SCOPE_IDENTITY()`.
4. **`GestionPersonas.API`**: Punto de entrada del sistema. Expone servicios RESTful, maneja el contenedor de Inyección de Dependencias (IoC) nativo de .NET 10 y mapea las excepciones de negocio a códigos de estado HTTP semánticos (`400 Bad Request`, `409 Conflict`, etc.).

---

##  Recursos de Inicialización y Pruebas

Para facilitar la revisión y el despliegue de la solución, se han adjuntado los siguientes artefactos en la raíz del repositorio:

*  **Script SQL de la Base de Datos**: Contiene el esquema relacional normalizado.
  *  [Descargar Script de Base de Datos SQL (GestionPersonasDB.sql)](#) *(Enlace local al archivo del repositorio)*
*  **Colección de Postman**: Archivo JSON con todas las peticiones listas para validar los escenarios del servicio (Casos de éxito y respuestas controladas para cada una de las Reglas de Negocio de la I a la V).
  *  [Descargar Colección de Postman (GestionPersonas.postman_collection.json)](#) *(Enlace local al archivo del repositorio)*

---

## Requisitos Previos e Instalación

1. **Entorno**: .NET 10 SDK o superior y SQL Server (LocalDB / Express / Standard).
2. **Base de Datos**: 
   * Ejecute el archivo `GestionPersonasDB.sql` en su instancia de SQL Server.
3. **Configuración de la Cadena de Conexión**:
   * Abra el archivo `src/GestionPersonas.API/appsettings.json` y valide/actualice la propiedad `ConnectionStrings:DefaultConnection`. El estándar configurado para pruebas locales es:
     ```json
     "Server=localhost;Database=GestionPersonasDB;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=False;"
     ```
4. **Ejecución**:
   * Desde la terminal en la raíz de la solución, ejecute:
     ```bash
     dotnet restore
     dotnet run --project src/GestionPersonas.API
     ```

---

## Matriz de Validación de Reglas de Negocio (Casos de Prueba)

El endpoint principal expuesto es `POST /api/personas`. A continuación se detallan los comportamientos automatizados en la colección de Postman adjunta:

| Regla de Negocio | Escenario Probado | Criterio de Aceptación | Código HTTP Esperado |
| :--- | :--- | :--- | :--- |
| **Flujo Ideal** | Registro Completo Válido | Inserta Persona y almacena contactos asociados. Retorna el nuevo `Id` generado. | `200 OK` |
| **Regla I** | Documento Identidad Duplicado | Controla la restricción de unicidad lógica antes de golpear las excepciones del motor. | `409 Conflict` |
| **Regla II** | Campos Obligatorios Vacíos | Validación de nulos o campos en blanco (`Nombres`, `Apellidos`, `Documento`). | `400 Bad Request` |
| **Regla III** | Valores numéricos en Nombres | El sistema detecta mediante Regex que los nombres/apellidos violan el alfabeto latino. | `400 Bad Request` |
| **Regla III** | Símbolos en Documento | Valida que el documento de identidad contenga únicamente caracteres alfanuméricos. | `400 Bad Request` |
| **Regla IV** | Sin Información de Contacto | Bloquea la petición si no se envía al menos un correo o una dirección física. | `400 Bad Request` |
| **Regla V** | Exceso de Canales (Límite 2) | Intento de inserción de 3 números telefónicos o 3 correos electrónicos. | `400 Bad Request` |

---

Desarrollado con altos estándares de ingeniería de software. Listo para evaluación.
