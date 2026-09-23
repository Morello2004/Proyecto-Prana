using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Collections.Generic;
using System.Data;

namespace Prana.Datos
{
    // Accede a los medios de pago almacenados en SQL Server.
    public class CD_MedioPago
    {
        // Obtiene los medios de pago activos.
        public List<MedioPago> ObtenerTodos()
        {
            string consulta = "SELECT id_medio_pago, nombre_medio_pago, ISNULL(recargo_medio_pago, 0), recargo_3, recargo_6, recargo_9, recargo_12, activo FROM dbo.Medio_Pago WHERE activo = 1 ORDER BY nombre_medio_pago;";
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<MedioPago> medios = new List<MedioPago>();
            while (lector.Read())
            {
                medios.Add(new MedioPago { IdMedioPago = lector.GetInt32(0), NombreMedioPago = lector.GetString(1), RecargoMedioPago = lector.GetDecimal(2), Recargo3 = lector.GetDecimal(3), Recargo6 = lector.GetDecimal(4), Recargo9 = lector.GetDecimal(5), Recargo12 = lector.GetDecimal(6), Activo = lector.GetBoolean(7) });
            }
            return medios;
        }

        // Inserta un medio de pago nuevo.
        public void Crear(MedioPago medio)
        {
            string consulta = "INSERT INTO dbo.Medio_Pago (nombre_medio_pago, recargo_medio_pago, recargo_3, recargo_6, recargo_9, recargo_12, activo) VALUES (@Nombre, @Recargo, @Recargo3, @Recargo6, @Recargo9, @Recargo12, 1);";
            EjecutarCambio(consulta, medio, false);
        }

        // Actualiza un medio de pago existente.
        public void Modificar(MedioPago medio)
        {
            string consulta = "UPDATE dbo.Medio_Pago SET nombre_medio_pago = @Nombre, recargo_medio_pago = @Recargo, recargo_3 = @Recargo3, recargo_6 = @Recargo6, recargo_9 = @Recargo9, recargo_12 = @Recargo12 WHERE id_medio_pago = @Id;";
            EjecutarCambio(consulta, medio, true);
        }

        // Desactiva un medio de pago sin borrar su historial.
        public void Desactivar(int idMedioPago)
        {
            string consulta = "UPDATE dbo.Medio_Pago SET activo = 0 WHERE id_medio_pago = @Id;";
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = idMedioPago;
            conexion.Open();
            comando.ExecuteNonQuery();
        }

        // Ejecuta una creación o modificación parametrizada.
        private void EjecutarCambio(string consulta, MedioPago medio, bool incluirId)
        {
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            if (incluirId) comando.Parameters.Add("@Id", SqlDbType.Int).Value = medio.IdMedioPago;
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 32).Value = medio.NombreMedioPago;
            comando.Parameters.Add("@Recargo", SqlDbType.Decimal).Value = medio.RecargoMedioPago;
            comando.Parameters.Add("@Recargo3", SqlDbType.Decimal).Value = medio.Recargo3;
            comando.Parameters.Add("@Recargo6", SqlDbType.Decimal).Value = medio.Recargo6;
            comando.Parameters.Add("@Recargo9", SqlDbType.Decimal).Value = medio.Recargo9;
            comando.Parameters.Add("@Recargo12", SqlDbType.Decimal).Value = medio.Recargo12;
            conexion.Open();
            comando.ExecuteNonQuery();
        }
    }
}
