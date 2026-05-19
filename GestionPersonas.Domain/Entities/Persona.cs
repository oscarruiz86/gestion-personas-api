namespace GestionPersonas.Domain.Entities
{
    public class Persona
    {
        public int Id { get; set; }
        public required string DocumentoIdentidad { get; set; } 
        public required string Nombres { get; set; }        
        public required string Apellidos { get; set; }       
        public DateTime FechaNacimiento { get; set; } 

        public List<string> Telefonos { get; set; } = new List<string>();
        public List<string> Correos { get; set; } = new List<string>();
        public List<string> DireccionesFisicas { get; set; } = new List<string>();
    }
}
