using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos
{
    public class CD_Pago
    {
        public List<venta> Buscar(string texto, bool pendientes) => CD_Venta.Buscar(texto).Where(v => !pendientes || v.SaldoPendiente > 0).ToList();
        public List<PagoRegistrado> Listar(int venta)
        {
            using var cn = new SqlConnection(Conexion.ObtenerCadena());
            using var cmd = new SqlCommand(@"SELECT p.*,m.nombre_medio_pago FROM Pago p JOIN Medio_Pago m ON m.id_medio_pago=p.id_medio_pago
                WHERE p.id_venta=@venta ORDER BY p.fecha_pago DESC,p.id_pago DESC", cn);
            cmd.Parameters.Add("@venta", SqlDbType.Int).Value = venta;
            cn.Open();
            using var r = cmd.ExecuteReader();
            var pagos = new List<PagoRegistrado>();
            while (r.Read()) pagos.Add(new PagoRegistrado { IdPago=(int)r["id_pago"], Fecha=(DateTime)r["fecha_pago"],
                Medio=(string)r["nombre_medio_pago"], Cuotas=r["cuotas"] == DBNull.Value ? 1 : (int)r["cuotas"],
                ImporteBase=(decimal)r["monto"], Cobrado=(decimal)r["monto_final"] });
            return pagos;
        }
        public void Registrar(CobroVenta pago, int usuario)
        {
            using var cn = new SqlConnection(Conexion.ObtenerCadena());
            cn.Open();
            using var tx = cn.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                DateTime ahora = CD_Caja.Ahora(cn, tx);
                CD_Caja.ExigirDiaAbierto(cn, tx, ahora);
                ValidarVenta(cn, tx, pago.IdVenta);
                using var saldoCmd = Comando(cn, tx, "SELECT saldo FROM (" + CD_Venta.SaldosSql + ") v WHERE id_venta=@venta", ("@venta", pago.IdVenta));
                decimal saldo = Convert.ToDecimal(saldoCmd.ExecuteScalar());
                if (pago.ImporteBase <= 0 || pago.ImporteBase > saldo) throw new InvalidOperationException("El importe supera el saldo actual. Actualice la venta.");
                using var medioCmd = Comando(cn, tx, @"SELECT CASE @cuotas WHEN 2 THEN recargo_2 WHEN 3 THEN recargo_3 WHEN 6 THEN recargo_6 WHEN 9 THEN recargo_9
                    WHEN 12 THEN recargo_12 ELSE recargo_medio_pago END FROM Medio_Pago WITH (HOLDLOCK)
                    WHERE id_medio_pago=@medio AND activo=1", ("@cuotas", pago.Cuotas), ("@medio", pago.IdMedioPago));
                object? porcentaje = medioCmd.ExecuteScalar();
                if (porcentaje == null || Convert.ToDecimal(porcentaje == DBNull.Value ? 0 : porcentaje) != pago.Porcentaje)
                    throw new InvalidOperationException("El medio de pago cambió o está inactivo. Vuelva a cargarlo.");
                // Agrega solo el nuevo recargo: total y monto_final permanecen comparables.
                int estado = Catalogo(cn, tx, "Estado_Venta", "id_estado_venta", "nombre_estado_venta", pago.ImporteBase == saldo ? "Pagada" : "Parcial");
                using var insertar = Comando(cn, tx, @"INSERT INTO Pago(id_venta,id_usuario,id_medio_pago,cuotas,monto,monto_final,fecha_pago,tipo_caja,cuenta_destino)
                    SELECT @venta,@usuario,@medio,@cuotas,@base,@final,@fecha,tipo_caja,@cuenta FROM Medio_Pago WHERE id_medio_pago=@medio;
                    UPDATE Venta SET total=total+@recargo,id_estado_venta=@estado WHERE id_venta=@venta;",
                    ("@venta",pago.IdVenta),("@usuario",usuario),("@medio",pago.IdMedioPago),("@cuotas",pago.Cuotas),
                    ("@base",pago.ImporteBase),("@final",pago.ImporteBase+pago.Recargo),("@recargo",pago.Recargo),("@estado",estado),("@fecha",ahora),("@cuenta",pago.CuentaDestino));
                insertar.ExecuteNonQuery();
                RegistrarActividad(cn,tx,usuario,"REGISTRAR_PAGO",pago.IdVenta,$"Base: {pago.ImporteBase:N2}; recargo: {pago.Recargo:N2}");
                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }
    }
}
