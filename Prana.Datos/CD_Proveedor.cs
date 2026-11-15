using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd proveedor
    public class CD_Proveedor
    {
        // se ejecuta dentro de la transaccion del ingreso: no deja proveedores huerfanos
        internal static int ObtenerOCrear(SqlConnection conexion, SqlTransaction transaccion, string nombre)
        {
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(@"
                DECLARE @id int;
                SELECT TOP (1) @id=id_proveedor FROM dbo.Proveedor WITH (UPDLOCK,HOLDLOCK)
                WHERE UPPER(LTRIM(RTRIM(nombre_proveedor)))=UPPER(@nombre) ORDER BY id_proveedor;
                IF @id IS NULL
                BEGIN
                    INSERT dbo.Proveedor(nombre_proveedor) VALUES(@nombre);
                    SET @id=CONVERT(int,SCOPE_IDENTITY());
                END;
                SELECT @id;", conexion, transaccion);
            comando.Parameters.Add("@nombre", SqlDbType.NVarChar, 100).Value = nombre.Trim();
            return Convert.ToInt32(comando.ExecuteScalar());
        }

        // envia los parametros de nuevo proveedor y ejecuta la escritura en la base de datos
        public bool NuevoProveedor(Proveedor proveedor)
        {
            string consulta = "INSERT INTO Proveedor (nombre_proveedor, Telefono) VALUES (@nombreProveedor, @Telefono)";
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
            using (SqlCommand comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@nombreProveedor", SqlDbType.NVarChar, 100).Value = proveedor.nombreProveedor;
                comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = (object)proveedor.Telefono ?? DBNull.Value;
                conexion.Open();
                int filasAfectadas = comando.ExecuteNonQuery();
            }

            return true;
        }

        // consulta buscar proveedor y transforma cada fila recibida en los objetos que devuelve
        public List<Proveedor> buscarProveedor(string nombreProveedor)
        {
            List<Proveedor> proveedoresEncontrados = new List<Proveedor>();
            string consulta = "SELECT * FROM Proveedor WHERE UPPER(nombre_proveedor) LIKE UPPER(@nombreProveedor)";
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
            using (SqlCommand comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@nombreProveedor", SqlDbType.NVarChar, 100).Value = "%" + nombreProveedor + "%";
                conexion.Open();
                using SqlDataReader lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    Proveedor proveedor = new Proveedor
                    {
                        // ajusta los nombres de la propiedad
                        idProveedor = lector.GetInt32(lector.GetOrdinal("id_proveedor")),
                        nombreProveedor = lector["nombre_proveedor"]?.ToString() ?? string.Empty

                    };
                    proveedoresEncontrados.Add(proveedor);
                }
            }

            return proveedoresEncontrados;
        }
    }
}
