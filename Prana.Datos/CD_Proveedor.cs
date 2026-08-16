using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;


namespace Prana.Datos
{
    public class CD_Proveedor
    {
        public bool NuevoProveedor(Proveedor proveedor)
        {
            string consulta = "INSERT INTO Proveedor (nombre_proveedor, email, Telefono) VALUES (@nombreProveedor, @mail, @Telefono)";

            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());





            using (SqlCommand comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@nombreProveedor", SqlDbType.NVarChar, 100).Value = proveedor.nombreProveedor;

                comando.Parameters.Add("@mail", SqlDbType.NVarChar, 100).Value = (object)proveedor.mail ?? DBNull.Value;

                comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value =(object)proveedor.Telefono ?? DBNull.Value;

                conexion.Open();

                int filasAfectadas = comando.ExecuteNonQuery();
            }



            return true;
        }

        public List<Proveedor> buscarProveedor(string nombreProveedor)
        {
            List<Proveedor> proveedoresEncontrados = new List<Proveedor>();



            string consulta = "SELECT * FROM Proveedor WHERE nombre_proveedor LIKE @nombreProveedor";
            


            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            using (SqlCommand comando = new SqlCommand(consulta, conexion))
            {

 

                comando.Parameters.Add("@nombreProveedor", SqlDbType.NVarChar, 100).Value = "%" + nombreProveedor + "%";

                conexion.Open();
                using SqlDataReader lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    var proveedor = new Proveedor
                    {
                        // Ajusta los nombres de la propiedad 
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
    




