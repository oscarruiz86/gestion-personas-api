using GestionPersonas.Domain.Entities;
using System.Text.RegularExpressions;

namespace GestionPersonas.Application.Validations
{
    public static class PersonaValidator
    {
        public static void Validar(Persona persona)
        {
            if (persona == null)
                throw new ArgumentNullException(nameof(persona), "Los datos de la persona no pueden ser nulos.");

            // REGLA II: Campos obligatorios
            if (string.IsNullOrWhiteSpace(persona.DocumentoIdentidad))
                throw new ArgumentException("El documento de identidad es obligatorio.");
            if (string.IsNullOrWhiteSpace(persona.Nombres))
                throw new ArgumentException("Los nombres son obligatorios.");
            if (string.IsNullOrWhiteSpace(persona.Apellidos))
                throw new ArgumentException("Los apellidos son obligatorios.");
            if (persona.FechaNacimiento == default || persona.FechaNacimiento > DateTime.Now)
                throw new ArgumentException("La fecha de nacimiento no es válida.");

            // REGLA III: Nombres y apellidos solo caracteres del alfabeto latino (sin números) 
            if (!Regex.IsMatch(persona.Nombres, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new ArgumentException("Los nombres solo aceptan caracteres del alfabeto latino.");
            if (!Regex.IsMatch(persona.Apellidos, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new ArgumentException("Los apellidos solo aceptan caracteres del alfabeto latino.");

            // REGLA III: Documento de identidad solo acepta valores alfanuméricos 
            if (!Regex.IsMatch(persona.DocumentoIdentidad, @"^[a-zA-Z0-9]+$"))
                throw new ArgumentException("El documento de identidad sólo acepta valores alfanuméricos.");

            // REGLA IV: Al menos una información de contacto (correo o dirección física)
            if (persona.Correos.Count == 0 && persona.DireccionesFisicas.Count == 0)
                throw new ArgumentException("Al menos debe registrar una información de contacto (correo electrónico o dirección física).");

            // REGLA V: Máximo 2 números telefónicos, 2 correos y 2 direcciones físicas
            if (persona.Telefonos.Count > 2)
                throw new ArgumentException("Máximo se pueden registrar 2 números telefónicos por persona.");
            if (persona.Correos.Count > 2)
                throw new ArgumentException("Máximo se pueden registrar 2 correos electrónicos por persona.");
            if (persona.DireccionesFisicas.Count > 2)
                throw new ArgumentException("Máximo se pueden registrar 2 direcciones físicas por persona.");
        }
    }
}