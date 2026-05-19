
using GestionPersonas.Application.Validations;
using GestionPersonas.Domain.Entities;
using GestionPersonas.Domain.Interfaces;

namespace GestionPersonas.Application.Services
{
    public class PersonaService
    {
        private readonly IPersonaRepository _personaRepository;

        public PersonaService(IPersonaRepository personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public void RegistrarPersona(Persona persona)
        {
           // Ejecutar validaciones estructurales y de formato
            PersonaValidator.Validar(persona);

            // REGLA I: No duplicados en documento de identidad
            if (_personaRepository.ExisteDocumento(persona.DocumentoIdentidad))
            {
                throw new InvalidOperationException($"No se puede registrar. Ya existe una persona con el documento de identidad '{persona.DocumentoIdentidad}'.");
            }

            // Si pasa todas las reglas, se procede a guardar
            _personaRepository.Guardar(persona);
        }
    }
}
