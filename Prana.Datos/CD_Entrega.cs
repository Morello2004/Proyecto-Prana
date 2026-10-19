using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Collections.Generic;
using System.Data;
using static Prana.Datos.GestionVentaSql;

namespace Prana.Datos
{
    public class CD_Entrega
    {
        // Un remito reserva unidades; una entrega confirmada cambia su estado, no vuelve a descontar stock.
        private const string PreparadosSql = @"SELECT r.id_venta,dr.id_variante,SUM(dr.cantidad) cantidad
            FROM Remito r JOIN Detalle_Remito dr ON dr.id_remito=r.id_remito
            WHERE NOT EXISTS(SELECT 1 FROM Envio e JOIN Estado_Envio ee ON ee.id_estado_envio=e.id_estado_envio
                WHERE e.id_remito=r.id_remito AND ee.nombre LIKE N'Cancelad%') GROUP BY r.id_venta,dr.id_variante";

        // Consulta las cantidades pendientes de preparar y el stock disponible de cada variante.
        public List<AlertaStockPendiente> ObtenerNecesidadesStock()
        {
            // Crea la conexión con la base de datos.
            using var cn = new SqlConnection(Conexion.ObtenerCadena());

            // Vendidos suma las unidades de cada variante dentro de cada venta.
            // Preparados reutiliza la consulta existente de unidades incluidas en remitos vigentes.
            // Necesidades resta las unidades preparadas y suma lo pendiente entre todas las ventas.
            // Se excluyen las ventas anuladas o canceladas.
            using var cmd = new SqlCommand(@"
        WITH Vendidos AS
        (
            SELECT id_venta, id_variante, SUM(cantidad) cantidad
            FROM Detalle_Venta
            GROUP BY id_venta, id_variante
        ),
        Preparados AS
        (
            " + PreparadosSql + @"
        ),
        Necesidades AS
        (
            SELECT d.id_variante, SUM(d.cantidad - COALESCE(r.cantidad, 0)) pendientes
            FROM Vendidos d
            JOIN Venta v ON v.id_venta = d.id_venta
            JOIN Estado_Venta ev ON ev.id_estado_venta = v.id_estado_venta
            LEFT JOIN Preparados r ON r.id_venta = d.id_venta AND r.id_variante = d.id_variante
            WHERE d.cantidad > COALESCE(r.cantidad, 0)
              AND ev.nombre_estado_venta NOT LIKE N'Anulad%'
              AND ev.nombre_estado_venta NOT LIKE N'Cancelad%'
            GROUP BY d.id_variante
        )
        SELECT vp.id_variante, CONCAT(p.nombre_producto, ' ', p.modelo, ' (', vp.color, ', ', vp.condicion_producto, ')') producto, n.pendientes, vp.stock_disponible disponible
        FROM Necesidades n
        JOIN Variante_Producto vp ON vp.id_variante = n.id_variante
        JOIN Producto p ON p.id_producto = vp.id_producto
        ORDER BY p.nombre_producto, vp.id_variante", cn);

            // Abre la conexión y ejecuta la consulta.
            cn.Open();
            using var lector = cmd.ExecuteReader();

            // Prepara la lista que se devuelve a la capa de negocio.
            var resultado = new List<AlertaStockPendiente>();

            // Recorre cada variante obtenida y carga sus cantidades.
            while (lector.Read())
            {
                resultado.Add(new AlertaStockPendiente
                {
                    IdVariante = lector.GetInt32(0), // Identificador de la variante.
                    Producto = lector.GetString(1), // Nombre, modelo, color y condición.
                    Pendientes = lector.GetInt32(2), // Unidades vendidas que todavía no se prepararon.
                    Disponible = lector.GetInt32(3) // Unidades disponibles para preparar.
                });
            }

            // Devuelve los datos; la capa de negocio calcula y filtra los faltantes.
            return resultado;
        }

      

        public List<ProductoPorEntregar> Pendientes(string texto)
        {
            using var cn = new SqlConnection(Conexion.ObtenerCadena());
            cn.Open();
            using var existe = new SqlCommand("SELECT OBJECT_ID('dbo.Venta_Entrega')",cn);
            bool tieneEntrega = existe.ExecuteScalar() != DBNull.Value;
            string direccion = tieneEntrega ? "COALESCE(NULLIF(ve.domicilio,''),c.direccion,'')" : "COALESCE(c.direccion,'')";
            using var cmd = new SqlCommand(@"SELECT v.id_venta,d.id_variante,CONCAT(c.nombre_cliente,' ',c.apellido_cliente) cliente,
                CONCAT(p.nombre_producto,' (',vp.color,', ',vp.condicion_producto,')') producto,
                m.nombre_modalidad_entrega modalidad,d.cantidad vendidos,COALESCE(r.cantidad,0) preparados," + direccion + @" direccion
                FROM Venta v JOIN Cliente c ON c.id_cliente=v.id_cliente JOIN Estado_Venta ev ON ev.id_estado_venta=v.id_estado_venta
                JOIN Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega
                JOIN (SELECT id_venta,id_variante,SUM(cantidad) cantidad FROM Detalle_Venta GROUP BY id_venta,id_variante) d ON d.id_venta=v.id_venta
                JOIN Variante_Producto vp ON vp.id_variante=d.id_variante JOIN Producto p ON p.id_producto=vp.id_producto
                LEFT JOIN (" + PreparadosSql + @") r ON r.id_venta=v.id_venta AND r.id_variante=d.id_variante " +
                (tieneEntrega ? " LEFT JOIN Venta_Entrega ve ON ve.id_venta=v.id_venta " : "") + @"
                WHERE d.cantidad>COALESCE(r.cantidad,0) AND ev.nombre_estado_venta NOT LIKE N'Anulad%' AND ev.nombre_estado_venta NOT LIKE N'Cancelad%'",cn);
            AgregarFiltro(cmd,texto,"CONCAT(v.id_venta,' ',c.nombre_cliente,' ',c.apellido_cliente,' ',c.dni,' ',p.nombre_producto)");
            cmd.CommandText += " ORDER BY v.fecha_venta,v.id_venta,d.id_variante";
            using var lector = cmd.ExecuteReader();
            var lista = new List<ProductoPorEntregar>();
            while (lector.Read()) lista.Add(new ProductoPorEntregar { IdVenta=(int)lector["id_venta"],IdVariante=(int)lector["id_variante"],
                Cliente=(string)lector["cliente"],Producto=(string)lector["producto"],Modalidad=(string)lector["modalidad"],
                Vendidos=(int)lector["vendidos"],Preparados=(int)lector["preparados"],Direccion=(string)lector["direccion"] });
            return lista;
        }
        public List<EntregaRegistrada> Listar(string texto, bool soloPendientes)
        {
            using var cn = new SqlConnection(Conexion.ObtenerCadena());
            using var cmd = new SqlCommand(@"SELECT e.id_envio,r.id_venta,r.id_remito,CONCAT(c.nombre_cliente,' ',c.apellido_cliente) cliente,
                e.direccion,ee.nombre,e.fecha_entrega,
                COALESCE((SELECT STRING_AGG(CAST(CONCAT(dr.cantidad,' × ',dr.descripcion) AS nvarchar(max)),'; ')
                    FROM Detalle_Remito dr WHERE dr.id_remito=r.id_remito),'') productos
                FROM Envio e JOIN Remito r ON r.id_remito=e.id_remito JOIN Venta v ON v.id_venta=r.id_venta
                JOIN Cliente c ON c.id_cliente=v.id_cliente JOIN Estado_Envio ee ON ee.id_estado_envio=e.id_estado_envio WHERE 1=1",cn);
            if (soloPendientes) cmd.CommandText += " AND ee.nombre=N'Pendiente'";
            AgregarFiltro(cmd,texto,"CONCAT(v.id_venta,' ',c.nombre_cliente,' ',c.apellido_cliente,' ',c.dni)");
            cmd.CommandText += " ORDER BY r.fecha_emision DESC,e.id_envio DESC";
            cn.Open();
            using var r = cmd.ExecuteReader();
            var lista = new List<EntregaRegistrada>();
            while(r.Read()) lista.Add(new EntregaRegistrada { IdEnvio=(int)r["id_envio"],IdVenta=(int)r["id_venta"],IdRemito=(int)r["id_remito"],
                Cliente=(string)r["cliente"],Productos=(string)r["productos"],Direccion=(string)r["direccion"],Estado=(string)r["nombre"],
                FechaEntrega=r["fecha_entrega"] == DBNull.Value ? null : (DateTime)r["fecha_entrega"] });
            return lista;
        }
        private static void AgregarFiltro(SqlCommand cmd,string texto,string expresion)
        {
            var palabras=texto.Split((char[]?)null,StringSplitOptions.RemoveEmptyEntries);
            for(int i=0;i<palabras.Length;i++)
            {
                cmd.CommandText += $" AND {expresion} COLLATE Latin1_General_CI_AI LIKE @p{i} ESCAPE '~'";
                cmd.Parameters.Add("@p"+i,SqlDbType.NVarChar,-1).Value="%"+CD_Venta.EscaparBusqueda(palabras[i])+"%";
            }
        }
        public void Preparar(int venta,int variante,int cantidad,string direccion,string horario,int usuario)
        {
            using var cn=new SqlConnection(Conexion.ObtenerCadena()); cn.Open();
            using var tx=cn.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                ValidarVenta(cn,tx,venta);
                using var pendiente=Comando(cn,tx,@"SELECT COALESCE(SUM(cantidad),0)-(SELECT COALESCE(SUM(cantidad),0) FROM ("+PreparadosSql+@") r
                    WHERE r.id_venta=@venta AND r.id_variante=@variante) FROM Detalle_Venta WHERE id_venta=@venta AND id_variante=@variante",
                    ("@venta",venta),("@variante",variante));
                if(cantidad<=0 || cantidad>Convert.ToInt32(pendiente.ExecuteScalar())) throw new InvalidOperationException("La cantidad supera las unidades pendientes. Actualice el listado.");
                // El descuento condicional impide stock negativo incluso con preparaciones simultáneas.
                using var stock=Comando(cn,tx,"UPDATE Variante_Producto SET stock_disponible=stock_disponible-@cantidad WHERE id_variante=@variante AND stock_disponible>=@cantidad",
                    ("@cantidad",cantidad),("@variante",variante));
                if(stock.ExecuteNonQuery()!=1) throw new InvalidOperationException("No hay stock disponible para preparar esa cantidad.");
                int estado=Catalogo(cn,tx,"Estado_Envio","id_estado_envio","nombre","Pendiente");
                using var modalidad=Comando(cn,tx,@"SELECT m.nombre_modalidad_entrega FROM Venta v JOIN Modalidad_Entrega m ON m.id_modalidad_entrega=v.id_modalidad_entrega WHERE id_venta=@venta",("@venta",venta));
                string tipo=Convert.ToString(modalidad.ExecuteScalar())!.StartsWith("Retiro",StringComparison.OrdinalIgnoreCase)?"Retiro en local":"Envío a domicilio";
                if(tipo=="Retiro en local") direccion="Retiro en local";
                using var remito=Comando(cn,tx,@"INSERT INTO Remito(id_venta,fecha_emision) OUTPUT INSERTED.id_remito
                    VALUES(@venta,SYSDATETIME())",("@venta",venta));
                int idRemito=Convert.ToInt32(remito.ExecuteScalar());
                using var detalle=Comando(cn,tx,@"INSERT INTO Detalle_Remito(id_remito,id_variante,cantidad,descripcion)
                    SELECT @remito,vp.id_variante,@cantidad,LEFT(CONCAT(p.nombre_producto,' (',vp.color,', ',vp.condicion_producto,')'),300)
                    FROM Variante_Producto vp JOIN Producto p ON p.id_producto=vp.id_producto WHERE vp.id_variante=@variante;
                    INSERT INTO Envio(id_remito,direccion,horario_sugerido,tipo_envio,costo,id_estado_envio)
                    VALUES(@remito,@direccion,@horario,@tipo,0,@estado)",("@remito",idRemito),("@cantidad",cantidad),("@variante",variante),
                    ("@direccion",direccion),("@horario",horario),("@tipo",tipo),("@estado",estado));
                detalle.ExecuteNonQuery();
                RegistrarActividad(cn,tx,usuario,"PREPARAR_ENTREGA",venta,$"Remito {venta}; preparación interna {idRemito}; variante {variante}; cantidad {cantidad}");
                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }
        public void Confirmar(int envio,int usuario)
        {
            using var cn=new SqlConnection(Conexion.ObtenerCadena()); cn.Open();
            using var tx=cn.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                using var buscar=Comando(cn,tx,"SELECT r.id_venta FROM Envio e JOIN Remito r ON r.id_remito=e.id_remito WHERE e.id_envio=@envio",("@envio",envio));
                object? id=buscar.ExecuteScalar();
                if(id==null) throw new InvalidOperationException("Seleccione una entrega existente.");
                int venta=Convert.ToInt32(id); ValidarVenta(cn,tx,venta);
                int estado=Catalogo(cn,tx,"Estado_Envio","id_estado_envio","nombre","Entregado");
                using var actualizar=Comando(cn,tx,@"UPDATE e SET id_estado_envio=@estado,fecha_entrega=SYSDATETIME()
                    FROM Envio e JOIN Estado_Envio ee ON ee.id_estado_envio=e.id_estado_envio WHERE e.id_envio=@envio AND ee.nombre=N'Pendiente'",
                    ("@estado",estado),("@envio",envio));
                if(actualizar.ExecuteNonQuery()!=1) throw new InvalidOperationException("La entrega ya fue confirmada o no está pendiente.");
                int entregado=Catalogo(cn,tx,"Estado_Variante_Producto","id_estado_variante_producto","nombre_estado_variante_producto","Entregado");
                // Marca completos los productos cuya cantidad vendida ya se entregó; el estado de pago se conserva.
                using var detalles=Comando(cn,tx,@"UPDATE d SET id_estado_variante_producto=@estado FROM Detalle_Venta d
                    WHERE d.id_venta=@venta AND (SELECT SUM(dv.cantidad) FROM Detalle_Venta dv WHERE dv.id_venta=d.id_venta AND dv.id_variante=d.id_variante)
                    <=(SELECT COALESCE(SUM(dr.cantidad),0) FROM Detalle_Remito dr JOIN Remito r ON r.id_remito=dr.id_remito
                    WHERE r.id_venta=d.id_venta AND dr.id_variante=d.id_variante AND EXISTS(SELECT 1 FROM Envio e JOIN Estado_Envio ee
                    ON ee.id_estado_envio=e.id_estado_envio WHERE e.id_remito=r.id_remito AND ee.nombre=N'Entregado'))",("@estado",entregado),("@venta",venta));
                detalles.ExecuteNonQuery();
                RegistrarActividad(cn,tx,usuario,"CONFIRMAR_ENTREGA",venta,$"Entrega {envio} confirmada");
                tx.Commit();
            }
            catch { tx.Rollback(); throw; }
        }

        internal static void RegistrarRetiroInmediato(
    SqlConnection conexion,
    SqlTransaction transaccion,
    int idVenta,
    NuevaVenta venta)
        {
            // Comprueba la venta y bloquea su encabezado durante la operación.
            ValidarVenta(conexion, transaccion, idVenta);

            int estadoEnvio = Catalogo(
                conexion, transaccion,
                "Estado_Envio",
                "id_estado_envio",
                "nombre",
                "Entregado");

            int estadoDetalle = Catalogo(
                conexion, transaccion,
                "Estado_Variante_Producto",
                "id_estado_variante_producto",
                "nombre_estado_variante_producto",
                "Entregado");

            // Crea un remito para todos los productos retirados.
            using var remito = Comando(
                conexion,
                transaccion,
                @"INSERT INTO Remito
            (id_venta, fecha_emision)
          OUTPUT INSERTED.id_remito
          VALUES (@venta, SYSDATETIME())",
                ("@venta", idVenta));

            int idRemito = Convert.ToInt32(remito.ExecuteScalar());

            foreach (DetalleNuevaVenta producto in venta.Detalles)
            {
                // Descuenta stock solamente si alcanza para la cantidad solicitada.
                using var stock = Comando(
                    conexion,
                    transaccion,
                    @"UPDATE Variante_Producto
              SET stock_disponible = stock_disponible - @cantidad
              WHERE id_variante = @variante
                AND stock_disponible >= @cantidad",
                    ("@cantidad", producto.Cantidad),
                    ("@variante", producto.IdVariante));

                if (stock.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "No hay stock suficiente para registrar el retiro.");
                }

                // Registra la cantidad retirada y la descripción del producto.
                using var detalle = Comando(
                    conexion,
                    transaccion,
                    @"INSERT INTO Detalle_Remito
                (id_remito, id_variante, cantidad, descripcion)
              SELECT
                @remito,
                vp.id_variante,
                @cantidad,
                LEFT(
                    CONCAT(
                        p.nombre_producto,
                        ' (', vp.color,
                        ', ', vp.condicion_producto, ')'),
                    300)
              FROM Variante_Producto vp
              JOIN Producto p ON p.id_producto = vp.id_producto
              WHERE vp.id_variante = @variante",
                    ("@remito", idRemito),
                    ("@cantidad", producto.Cantidad),
                    ("@variante", producto.IdVariante));

                if (detalle.ExecuteNonQuery() != 1)
                {
                    throw new InvalidOperationException(
                        "No se pudo registrar un producto en el remito.");
                }
            }

            // Registra el retiro como entregado desde el momento de la venta.
            using var envio = Comando(
                conexion,
                transaccion,
                @"INSERT INTO Envio
            (id_remito, direccion, horario_sugerido,
             tipo_envio, costo, id_estado_envio, fecha_entrega)
          VALUES
            (@remito, @direccion, @horario,
             N'Retiro en local', 0, @estado, SYSDATETIME())",
                ("@remito", idRemito),
                ("@direccion", "Retiro en local"),
                ("@horario", ""),
                ("@estado", estadoEnvio));

            envio.ExecuteNonQuery();

            // Marca los productos de esta venta como entregados.
            using var actualizarDetalles = Comando(
                conexion,
                transaccion,
                @"UPDATE Detalle_Venta
          SET id_estado_variante_producto = @estado
          WHERE id_venta = @venta",
                ("@estado", estadoDetalle),
                ("@venta", idVenta));

            actualizarDetalles.ExecuteNonQuery();

            // Deja constancia del retiro en el historial.
            RegistrarActividad(
                conexion,
                transaccion,
                venta.IdUsuario,
                "CONFIRMAR_ENTREGA",
                idVenta,
                $"Retiro inmediato en local; remito interno {idRemito}");
        }
    }
}
