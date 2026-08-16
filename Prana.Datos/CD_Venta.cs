using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Prana.Entidades;

namespace Prana.Datos
{
    public class CD_Venta
    {
        public static List<venta> Top10Ventas()
        {
            List<venta> ventas = new List<venta>();
            string consulta = @"
                            SELECT TOP 10
                                v.id_venta,
                                 c.id_cliente,
                                c.nombre_cliente,
                                c.apellido_cliente,
                                v.fecha_venta,
                                v.total,
                                ev.nombre_estado_venta
                            FROM Venta v
                            INNER JOIN Cliente c
                                ON c.id_cliente = v.id_cliente
                            INNER JOIN Estado_Venta ev
                                ON ev.id_estado_venta = v.id_estado_venta
                            ORDER BY v.fecha_venta DESC;";
            // Crea la conexión usando la cadena obtenida desde App.config
            // El using cerrará y liberará automáticamente la conexión
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());

            // Prepara la consulta para ejecutarla mediante la conexión
            using SqlCommand comando = new SqlCommand(consulta, conexion);

            // Abre la conexión con SQL Server.
            conexion.Open();

            // Ejecuta el SELECT y obtiene un lector con los resultados
                using SqlDataReader lector = comando.ExecuteReader();

       
            while (lector.Read())
            { //crea un objeto venta con los valores encontrados
                venta venta = new venta
                {
                    IdVenta = lector.GetInt32(
                        lector.GetOrdinal("id_venta")),

                    Fecha = lector.GetDateTime(
                        lector.GetOrdinal("fecha_venta")),

                    Total = lector.GetDecimal(
                        lector.GetOrdinal("total")),

                    Estado = lector.GetString(
                        lector.GetOrdinal("nombre_estado_venta")),

                    Cliente = new Cliente
                    {
                        IdCliente = lector.GetInt32(
                            lector.GetOrdinal("id_cliente")),

                        Nombre = lector.GetString(
                            lector.GetOrdinal("nombre_cliente")),

                        Apellido = lector.GetString(
                            lector.GetOrdinal("apellido_cliente"))
                    }
                };


                ventas.Add(venta);

                
            }
            return ventas;
        }  
    }
}
