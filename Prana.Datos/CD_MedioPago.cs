using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Collections.Generic;
using System.Data;

namespace Prana.Datos
{
    // Accede a los medios de pago almacenados en SQL Server.
    public class CD_MedioPago
    {
        // Obtiene los cuatro medios de pago predefinidos activos.
        public List<MedioPago> ObtenerTodos()
        {
            string consulta = "SELECT id_medio_pago, nombre_medio_pago, ISNULL(recargo_medio_pago, 0), recargo_3, recargo_6, recargo_9, recargo_12, activo, tipo_caja, recargo_2 FROM dbo.Medio_Pago WHERE activo = 1 AND nombre_medio_pago COLLATE Latin1_General_100_BIN2 IN (N'Efectivo', N'Transferencia', N'Débito', N'Crédito') ORDER BY nombre_medio_pago;";
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<MedioPago> medios = new List<MedioPago>();
            while (lector.Read())
            {
                medios.Add(new MedioPago { IdMedioPago = lector.GetInt32(0), NombreMedioPago = lector.GetString(1), RecargoMedioPago = lector.GetDecimal(2), Recargo2 = lector.GetDecimal(9), Recargo3 = lector.GetDecimal(3), Recargo6 = lector.GetDecimal(4), Recargo9 = lector.GetDecimal(5), Recargo12 = lector.GetDecimal(6), Activo = lector.GetBoolean(7), TipoCaja = lector.GetString(8) });
            }
            return medios;
        }

        // Actualiza unicamente los recargos del medio predefinido seleccionado.
        public void Modificar(MedioPago medio)
        {
            string consulta = @"UPDATE dbo.Medio_Pago
                SET recargo_medio_pago = @Recargo, recargo_2 = @Recargo2, recargo_3 = @Recargo3,
                    recargo_6 = @Recargo6, recargo_9 = @Recargo9, recargo_12 = @Recargo12
                WHERE id_medio_pago = @Id AND activo = 1
                    AND nombre_medio_pago COLLATE Latin1_General_100_BIN2 = @Nombre
                    AND tipo_caja COLLATE Latin1_General_100_BIN2 = @TipoCaja
                    AND ((nombre_medio_pago COLLATE Latin1_General_100_BIN2 = N'Efectivo' AND tipo_caja COLLATE Latin1_General_100_BIN2 = N'Efectivo')
                      OR (nombre_medio_pago COLLATE Latin1_General_100_BIN2 = N'Transferencia' AND tipo_caja COLLATE Latin1_General_100_BIN2 = N'Transferencia')
                      OR (nombre_medio_pago COLLATE Latin1_General_100_BIN2 = N'Débito' AND tipo_caja COLLATE Latin1_General_100_BIN2 = N'Débito')
                      OR (nombre_medio_pago COLLATE Latin1_General_100_BIN2 = N'Crédito' AND tipo_caja COLLATE Latin1_General_100_BIN2 = N'Crédito'));";
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            comando.Parameters.Add("@Id", SqlDbType.Int).Value = medio.IdMedioPago;
            comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 32).Value = medio.NombreMedioPago;
            comando.Parameters.Add("@TipoCaja", SqlDbType.NVarChar, 20).Value = medio.TipoCaja;
            comando.Parameters.Add("@Recargo", SqlDbType.Decimal).Value = medio.RecargoMedioPago;
            comando.Parameters.Add("@Recargo2", SqlDbType.Decimal).Value = medio.Recargo2;
            comando.Parameters.Add("@Recargo3", SqlDbType.Decimal).Value = medio.Recargo3;
            comando.Parameters.Add("@Recargo6", SqlDbType.Decimal).Value = medio.Recargo6;
            comando.Parameters.Add("@Recargo9", SqlDbType.Decimal).Value = medio.Recargo9;
            comando.Parameters.Add("@Recargo12", SqlDbType.Decimal).Value = medio.Recargo12;
            conexion.Open();
            if (comando.ExecuteNonQuery() != 1) throw new System.InvalidOperationException("El medio de pago no existe o ya no es uno de los medios predefinidos.");
        }
    }
}
