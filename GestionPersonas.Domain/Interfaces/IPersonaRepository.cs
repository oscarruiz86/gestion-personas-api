using GestionPersonas.Domain.Entities;

namespace GestionPersonas.Domain.Interfaces
{
    public interface IPersonaRepository
    {
        bool ExisteDocumento(string documentoIdentidad);
        void Guardar(Persona persona);
    }
}
