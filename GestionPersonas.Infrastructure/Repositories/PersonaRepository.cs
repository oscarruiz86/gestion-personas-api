
using GestionPersonas.Domain.Entities;
using GestionPersonas.Domain.Interfaces;
using GestionPersonas.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace GestionPersonas.Infrastructure.Repositories;

public class PersonaRepository(DbConnection connection) : IPersonaRepository
{
    public bool ExisteDocumento(string documentoIdentidad)
    {
        using var conexion = connection.CrearConexion();
        const string query = "SELECT COUNT(1) FROM Personas WHERE DocumentoIdentidad = @Doc";

        using var comando = new SqlCommand(query, conexion);
        comando.Parameters.AddWithValue("@Doc", documentoIdentidad);

        conexion.Open();
        int conteo = Convert.ToInt32(comando.ExecuteScalar());
        return conteo > 0;
    }

    public void Guardar(Persona persona)
    {
        using var conexion = connection.CrearConexion();
        conexion.Open();
        using var transaccion = conexion.BeginTransaction();

        try
        {
            // 1. Insertar en la tabla principal y recuperar el ID autoincremental generado
            const string queryPersona = @"
                INSERT INTO Personas (DocumentoIdentidad, Nombres, Apellidos, FechaNacimiento) 
                VALUES (@Doc, @Nom, @Ape, @Fec);
                SELECT SCOPE_IDENTITY();"; // Obtiene el ID generado en esta sesión

            int nuevoPersonaId;
            using (var cmd = new SqlCommand(queryPersona, conexion, transaccion))
            {
                cmd.Parameters.AddWithValue("@Doc", persona.DocumentoIdentidad);
                cmd.Parameters.AddWithValue("@Nom", persona.Nombres);
                cmd.Parameters.AddWithValue("@Ape", persona.Apellidos);
                cmd.Parameters.AddWithValue("@Fec", persona.FechaNacimiento);

                // ExecuteScalar nos devuelve el primer valor de la primera fila (el SCOPE_IDENTITY)
                nuevoPersonaId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // 2. Insertar Teléfonos usando el nuevoPersonaId como FK
            foreach (var tel in persona.Telefonos)
            {
                const string queryTel = "INSERT INTO Personas_Telefonos (PersonaId, Telefono) VALUES (@PersonaId, @Tel)";
                using var cmd = new SqlCommand(queryTel, conexion, transaccion);
                cmd.Parameters.AddWithValue("@PersonaId", nuevoPersonaId);
                cmd.Parameters.AddWithValue("@Tel", tel);
                cmd.ExecuteNonQuery();
            }

            // 3. Insertar Correos usando el nuevoPersonaId como FK
            foreach (var correo in persona.Correos)
            {
                const string queryCorreo = "INSERT INTO Personas_Correos (PersonaId, Correo) VALUES (@PersonaId, @Correo)";
                using var cmd = new SqlCommand(queryCorreo, conexion, transaccion);
                cmd.Parameters.AddWithValue("@PersonaId", nuevoPersonaId);
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.ExecuteNonQuery();
            }

            // 4. Insertar Direcciones Físicas usando el nuevoPersonaId como FK
            foreach (var dir in persona.DireccionesFisicas)
            {
                const string queryDir = "INSERT INTO Personas_Direcciones (PersonaId, Direccion) VALUES (@PersonaId, @Dir)";
                using var cmd = new SqlCommand(queryDir, conexion, transaccion);
                cmd.Parameters.AddWithValue("@PersonaId", nuevoPersonaId);
                cmd.Parameters.AddWithValue("@Dir", dir);
                cmd.ExecuteNonQuery();
            }

            // Si todo sale bien, consolidamos los cambios
            transaccion.Commit();
            persona.Id = nuevoPersonaId; // Asignamos el ID al objeto por si se necesita retornar en la respuesta
        }
        catch (Exception)
        {
            // Si algo falla en cualquiera de las inserciones, deshacemos todo el bloque
            transaccion.Rollback();
            throw;
        }
    }
}