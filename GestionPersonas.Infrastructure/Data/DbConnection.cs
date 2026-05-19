

using Microsoft.Data.SqlClient;

namespace GestionPersonas.Infrastructure.Data
{
    public class DbConnection(string connectionString)
    {
        public SqlConnection CrearConexion() => new(connectionString);
    }
}
