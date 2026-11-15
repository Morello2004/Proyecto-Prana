using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd pago
    public class CD_Pago
    {
        // resuelve buscar con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<venta> Buscar(string texto, bool pendientes) => CD_Venta.Buscar(texto).Where(ventaActual => !pendientes || ventaActual.SaldoPendiente > 0).ToList();
        // consulta listar y transforma cada fila recibida en los objetos que devuelve
        public List<Pago> Listar(int venta)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(@"SELECT p.*,m.nombre_medio_pago FROM Pago p JOIN Medio_Pago m ON m.id_medio_pago=p.id_medio_pago
                WHERE p.id_venta=@venta ORDER BY p.fecha_pago DESC,p.id_pago DESC", conexion);
            comando.Parameters.Add("@venta", SqlDbType.Int).Value = venta;
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            List<Pago> pagos = new List<Pago>();
            while (lector.Read())
            {
                int cuotasRegistradas;
                if (lector["cuotas"] == DBNull.Value)
                {
                    cuotasRegistradas = 1;
                }
                else
                {
                    cuotasRegistradas = (int)lector["cuotas"];
                }
                pagos.Add(new Pago
                {
                    IdPago = (int)lector["id_pago"],
                    Fecha = (DateTime)lector["fecha_pago"],
                    Medio = (string)lector["nombre_medio_pago"],
                    Cuotas = cuotasRegistradas,
                    ImporteBase = (decimal)lector["monto"],
                    Cobrado = (decimal)lector["monto_final"],
                    Recargo = (decimal)lector["monto_final"] - (decimal)lector["monto"]

                });
            }

            return pagos;
        }

        // coordina registrar dentro de una transaccion para confirmar o revertir sus escrituras juntas
        public void Registrar(Pago pago, int usuario)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                DateTime ahora = CD_Caja.Ahora(conexion, transaccion);
                CD_Caja.ExigirDiaAbierto(conexion, transaccion, ahora);
                ValidarVenta(conexion, transaccion, pago.IdVenta);
                using SqlCommand saldoCmd = Comando(conexion, transaccion, "SELECT saldo FROM (" + CD_Venta.SaldosSql + ") v WHERE id_venta=@venta", ("@venta", pago.IdVenta));
                decimal saldo = Convert.ToDecimal(saldoCmd.ExecuteScalar());
                if (pago.ImporteBase <= 0 || pago.ImporteBase > saldo)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El importe supera el saldo actual. Actualice la venta.");
                }

                using SqlCommand medioCmd = Comando(conexion, transaccion, @"SELECT CASE @cuotas WHEN 2 THEN recargo_2 WHEN 3 THEN recargo_3 WHEN 6 THEN recargo_6 WHEN 9 THEN recargo_9
                    WHEN 12 THEN recargo_12 ELSE recargo_medio_pago END FROM Medio_Pago WITH (HOLDLOCK)
                    WHERE id_medio_pago=@medio AND activo=1", ("@cuotas", pago.Cuotas), ("@medio", pago.IdMedioPago));
                object? porcentaje = medioCmd.ExecuteScalar();
                object porcentajeConfigurado;
                if (porcentaje == DBNull.Value)
                {
                    porcentajeConfigurado = 0;
                }
                else
                {
                    porcentajeConfigurado = porcentaje;
                }
                if (porcentaje == null || Convert.ToDecimal(porcentajeConfigurado) != pago.Porcentaje)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El medio de pago cambió o está inactivo. Vuelva a cargarlo.");
                }
                string nombreEstadoVenta;
                if (pago.ImporteBase == saldo)
                {
                    nombreEstadoVenta = "Pagada";
                }
                else
                {
                    nombreEstadoVenta = "Parcial";
                }

                // agrega solo el nuevo recargo: total y monto_final permanecen comparables
                int estado = ObtenerOCrearIdPorNombre(conexion, transaccion, "Estado_Venta", "id_estado_venta", "nombre_estado_venta", nombreEstadoVenta);
                using SqlCommand insertar = Comando(conexion, transaccion, @"INSERT INTO Pago(id_venta,id_usuario,id_medio_pago,cuotas,monto,monto_final,fecha_pago,tipo_caja,cuenta_destino)
                    SELECT @venta,@usuario,@medio,@cuotas,@base,@final,@fecha,tipo_caja,@cuenta FROM Medio_Pago WHERE id_medio_pago=@medio;
                    UPDATE Venta SET total=total+@recargo,id_estado_venta=@estado WHERE id_venta=@venta;", ("@venta", pago.IdVenta), ("@usuario", usuario), ("@medio", pago.IdMedioPago), ("@cuotas", pago.Cuotas), ("@base", pago.ImporteBase), ("@final", pago.ImporteBase + pago.Recargo), ("@recargo", pago.Recargo), ("@estado", estado), ("@fecha", ahora), ("@cuenta", pago.CuentaDestino));
                insertar.ExecuteNonQuery();
                RegistrarActividad(conexion, transaccion, usuario, "REGISTRAR_PAGO", pago.IdVenta, $"Base: {pago.ImporteBase:N2}; recargo: {pago.Recargo:N2}");
                // confirma todos los cambios de esta transaccion como una unidad
                transaccion.Commit();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // revierte los cambios no confirmados de esta transaccion
                transaccion.Rollback();
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }
    }
}
