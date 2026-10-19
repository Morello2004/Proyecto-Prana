using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System.Data;

namespace Prana.Datos
{
    // Crea y elimina lotes de datos de prueba en una transaccion.
    public class CD_DatosPrueba
    {
        // Obtiene el lote activo y las fechas de sus cierres.
        public LoteDatosPrueba? ObtenerActivo()
        {
            const string consulta = @"
SELECT TOP (1) l.id_lote, l.token, l.fecha_creacion, MIN(r.fecha), MAX(r.fecha)
FROM dbo.LoteDatosPrueba l
LEFT JOIN dbo.RegistroDatosPrueba r ON r.id_lote = l.id_lote AND r.tabla = N'Caja_Cierre'
WHERE l.activo = 1
GROUP BY l.id_lote, l.token, l.fecha_creacion
ORDER BY l.fecha_creacion DESC;";
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            conexion.Open();
            using SqlDataReader lector = comando.ExecuteReader();
            if (!lector.Read()) return null;
            return new LoteDatosPrueba
            {
                Id = lector.GetGuid(0),
                Token = lector.GetString(1),
                Creado = lector.GetDateTime(2),
                FechaDesde = lector.IsDBNull(3) ? null : lector.GetDateTime(3),
                FechaHasta = lector.IsDBNull(4) ? null : lector.GetDateTime(4)
            };
        }

        // Crea cien clientes productos ventas pagos y cierres de caja historicos.
        public LoteDatosPrueba Cargar(int idAdministrador)
        {
            Guid idLote = Guid.NewGuid();
            string token = "PRUEBA-" + idLote.ToString("N").ToUpperInvariant();
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                BloquearLotes(conexion, transaccion);
                using SqlCommand comando = new SqlCommand(ConsultaCarga, conexion, transaccion);
                comando.CommandTimeout = 180;
                comando.Parameters.Add("@IdLote", SqlDbType.UniqueIdentifier).Value = idLote;
                comando.Parameters.Add("@Token", SqlDbType.NVarChar, 50).Value = token;
                comando.Parameters.Add("@Administrador", SqlDbType.Int).Value = idAdministrador;
                using SqlDataReader lector = comando.ExecuteReader();
                if (!lector.Read()) throw new InvalidOperationException("No se genero el lote de prueba.");
                LoteDatosPrueba lote = new LoteDatosPrueba
                {
                    Id = idLote,
                    Token = token,
                    Creado = lector.GetDateTime(0),
                    FechaDesde = lector.GetDateTime(1),
                    FechaHasta = lector.GetDateTime(2)
                };
                lector.Close();
                transaccion.Commit();
                return lote;
            }
            catch
            {
                if (transaccion.Connection != null) transaccion.Rollback();
                throw;
            }
        }

        // Elimina solo los registros registrados dentro del lote activo.
        public void EliminarActivo(int idAdministrador)
        {
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            try
            {
                BloquearLotes(conexion, transaccion);
                using SqlCommand loteCmd = new SqlCommand("SELECT TOP (1) id_lote, token FROM dbo.LoteDatosPrueba WITH (UPDLOCK, HOLDLOCK) WHERE activo = 1 ORDER BY fecha_creacion DESC;", conexion, transaccion);
                using SqlDataReader lector = loteCmd.ExecuteReader();
                if (!lector.Read()) throw new InvalidOperationException("No hay un lote de prueba activo para eliminar.");
                Guid idLote = lector.GetGuid(0);
                string token = lector.GetString(1);
                lector.Close();

                using SqlCommand comando = new SqlCommand(ConsultaEliminacion, conexion, transaccion);
                comando.CommandTimeout = 180;
                comando.Parameters.Add("@IdLote", SqlDbType.UniqueIdentifier).Value = idLote;
                comando.Parameters.Add("@Token", SqlDbType.NVarChar, 50).Value = token;
                comando.Parameters.Add("@Administrador", SqlDbType.Int).Value = idAdministrador;
                comando.ExecuteNonQuery();
                transaccion.Commit();
            }
            catch
            {
                if (transaccion.Connection != null) transaccion.Rollback();
                throw;
            }
        }

        // Serializa las operaciones para que no se creen ni borren lotes al mismo tiempo.
        private static void BloquearLotes(SqlConnection conexion, SqlTransaction transaccion)
        {
            using SqlCommand comando = new SqlCommand(@"
DECLARE @resultado int;
EXEC @resultado = sys.sp_getapplock @Resource=N'Prana.DatosPrueba',
    @LockMode=N'Exclusive', @LockOwner=N'Transaction', @LockTimeout=15000;
IF @resultado < 0 THROW 51000, N'No se pudo bloquear la operacion de datos de prueba. Reintente.', 1;", conexion, transaccion);
            comando.ExecuteNonQuery();
        }

        // Conserva la carga SQL de forma aislada dentro de la capa de datos.
        private const string ConsultaCarga = @"
SET XACT_ABORT ON;
IF EXISTS(SELECT 1 FROM dbo.LoteDatosPrueba WITH (UPDLOCK, HOLDLOCK) WHERE activo = 1)
    THROW 51001, N'Ya existe un lote activo. Elimínelo antes de cargar otro.', 1;
IF NOT EXISTS(SELECT 1 FROM dbo.Usuario WHERE id_usuario=@Administrador AND activo=1 AND id_rol_usuario=1)
    THROW 51002, N'Solo un administrador activo puede cargar los datos de prueba.', 1;
DECLARE @Medio int, @Recargo decimal(5,2), @EstadoVenta int, @EstadoDetalle int, @EstadoEnvio int, @Modalidad int;
SELECT TOP (1) @Medio=id_medio_pago, @Recargo=ISNULL(recargo_medio_pago,0)
FROM dbo.Medio_Pago
WHERE activo=1 AND nombre_medio_pago COLLATE Latin1_General_100_BIN2=N'Efectivo'
  AND tipo_caja COLLATE Latin1_General_100_BIN2=N'Efectivo';
IF @Medio IS NULL THROW 51003, N'No se encuentra el medio de pago Efectivo.', 1;
INSERT dbo.LoteDatosPrueba(id_lote, token, creado_por) VALUES(@IdLote,@Token,@Administrador);
DECLARE @Registros TABLE(tabla nvarchar(50), id_registro int, fecha date NULL);
DECLARE @Id int;
INSERT dbo.Categoria(nombre_categoria) VALUES(CONCAT(N'Prueba ',@Token));
SET @Id=CONVERT(int,SCOPE_IDENTITY());
INSERT @Registros(tabla,id_registro) VALUES(N'Categoria',@Id);
INSERT dbo.Marca(nombre_marca) VALUES(CONCAT(N'Prueba ',@Token));
SET @Id=CONVERT(int,SCOPE_IDENTITY());
INSERT @Registros(tabla,id_registro) VALUES(N'Marca',@Id);
INSERT dbo.Localidad(nombre_localidad,codigo_postal) VALUES(CONCAT(N'Prueba ',@Token),NULL);
SET @Id=CONVERT(int,SCOPE_IDENTITY());
INSERT @Registros(tabla,id_registro) VALUES(N'Localidad',@Id);
IF NOT EXISTS(SELECT 1 FROM dbo.Estado_Venta WHERE nombre_estado_venta=N'Pagada')
BEGIN
    INSERT dbo.Estado_Venta(nombre_estado_venta) VALUES(N'Pagada');
    SET @Id=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Estado_Venta',@Id);
END;
SELECT TOP (1) @EstadoVenta=id_estado_venta FROM dbo.Estado_Venta WHERE nombre_estado_venta=N'Pagada' ORDER BY id_estado_venta;
IF NOT EXISTS(SELECT 1 FROM dbo.Estado_Variante_Producto WHERE nombre_estado_variante_producto=N'Pendiente')
BEGIN
    INSERT dbo.Estado_Variante_Producto(nombre_estado_variante_producto) VALUES(N'Pendiente');
    SET @Id=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Estado_Variante_Producto',@Id);
END;
SELECT TOP (1) @EstadoDetalle=id_estado_variante_producto FROM dbo.Estado_Variante_Producto WHERE nombre_estado_variante_producto=N'Pendiente' ORDER BY id_estado_variante_producto;
IF NOT EXISTS(SELECT 1 FROM dbo.Estado_Envio WHERE nombre=N'Pendiente')
BEGIN
    INSERT dbo.Estado_Envio(nombre) VALUES(N'Pendiente');
    SET @Id=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Estado_Envio',@Id);
END;
SELECT TOP (1) @EstadoEnvio=id_estado_envio FROM dbo.Estado_Envio WHERE nombre=N'Pendiente' ORDER BY id_estado_envio;
IF NOT EXISTS(SELECT 1 FROM dbo.Modalidad_Entrega WHERE nombre_modalidad_entrega=N'Envío a domicilio')
BEGIN
    INSERT dbo.Modalidad_Entrega(nombre_modalidad_entrega) VALUES(N'Envío a domicilio');
    SET @Id=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Modalidad_Entrega',@Id);
END;
SELECT TOP (1) @Modalidad=id_modalidad_entrega FROM dbo.Modalidad_Entrega WHERE nombre_modalidad_entrega=N'Envío a domicilio' ORDER BY id_modalidad_entrega;
DECLARE @Categoria int, @Marca int, @Localidad int, @InicioDni bigint=80000000;
SELECT @Categoria=id_registro FROM @Registros WHERE tabla=N'Categoria';
SELECT @Marca=id_registro FROM @Registros WHERE tabla=N'Marca';
SELECT @Localidad=id_registro FROM @Registros WHERE tabla=N'Localidad';
WHILE @InicioDni<=99999900 AND EXISTS(
    SELECT 1 FROM dbo.Cliente
    WHERE TRY_CONVERT(bigint,dni) BETWEEN @InicioDni AND @InicioDni+99)
    SET @InicioDni=@InicioDni+100;
IF @InicioDni>99999900 THROW 51004, N'No se encontro un rango de DNI libre para los clientes de prueba.', 1;
DECLARE @Fechas TABLE(numero int PRIMARY KEY, fecha date UNIQUE);
DECLARE @FechaCandidata date=DATEADD(day,-3650,CONVERT(date,SYSDATETIME()));
DECLARE @NumeroFecha int=1;
DECLARE @Hoy date=CONVERT(date,SYSDATETIME());
WHILE @NumeroFecha<=100 AND @FechaCandidata<@Hoy
BEGIN
    IF NOT EXISTS(SELECT 1 FROM dbo.Caja_Cierre WHERE fecha=@FechaCandidata)
    BEGIN
        INSERT @Fechas(numero,fecha) VALUES(@NumeroFecha,@FechaCandidata);
        SET @NumeroFecha=@NumeroFecha+1;
    END;
    SET @FechaCandidata=DATEADD(day,1,@FechaCandidata);
END;
IF @NumeroFecha<=100 THROW 51005, N'No hay suficientes dias historicos disponibles para crear cien cierres.', 1;
DECLARE @Clientes TABLE(numero int PRIMARY KEY, id int);
DECLARE @Productos TABLE(numero int PRIMARY KEY, id int);
DECLARE @Variantes TABLE(numero int PRIMARY KEY, id int);
DECLARE @Ventas TABLE(numero int PRIMARY KEY, id int, fecha date);
DECLARE @Pagos TABLE(numero int PRIMARY KEY, id int);
DECLARE @i int=1, @Cliente int, @Producto int, @Variante int, @Venta int, @Pago int, @Remito int, @IdDetalleRemito int, @Envio int;
DECLARE @Fecha date, @Base decimal(10,2), @Final decimal(10,2), @NombreCliente nvarchar(200);
DECLARE @Movimientos nvarchar(max), @Datos nvarchar(max);
WHILE @i<=100
BEGIN
    INSERT dbo.Cliente(nombre_cliente,apellido_cliente,dni,telefono,email,direccion,fecha_registro,saldo_a_favor,id_localidad)
    VALUES(CONCAT(N'Cliente ',RIGHT(N'000'+CONVERT(nvarchar(3),@i),3)),@Token,
        RIGHT(N'00000000'+CONVERT(nvarchar(20),@InicioDni+@i-1),8),
        CONCAT(N'11',RIGHT(N'00000000'+CONVERT(nvarchar(8),@i),8)),NULL,
        CONCAT(N'Direccion de prueba ',@i),SYSDATETIME(),0,@Localidad);
    SET @Cliente=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Clientes(numero,id) VALUES(@i,@Cliente);
    INSERT @Registros(tabla,id_registro) VALUES(N'Cliente',@Cliente);
    INSERT dbo.Producto(nombre_producto,id_marca,descripcion,id_categoria,modelo)
    VALUES(CONCAT(N'Prueba ',@Token,N' Producto ',RIGHT(N'000'+CONVERT(nvarchar(3),@i),3)),@Marca,
        CONCAT(N'Producto generado para pruebas del lote ',@Token),@Categoria,
        CONCAT('PA',RIGHT('000'+CONVERT(varchar(3),@i),3)));
    SET @Producto=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Productos(numero,id) VALUES(@i,@Producto);
    INSERT @Registros(tabla,id_registro) VALUES(N'Producto',@Producto);
    SET @Base=CONVERT(decimal(10,2),10000+(@i*100));
    INSERT dbo.Variante_Producto(color,condicion_producto,precio,stock_disponible,id_producto,activo,costo)
    VALUES(N'Negro',N'Nuevo',@Base,10,@Producto,1,ROUND(@Base*0.7,2));
    SET @Variante=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Variantes(numero,id) VALUES(@i,@Variante);
    INSERT @Registros(tabla,id_registro) VALUES(N'Variante_Producto',@Variante);
    SELECT @Fecha=fecha FROM @Fechas WHERE numero=@i;
    SET @Final=ROUND(@Base+( @Base*@Recargo/100.0),2);
    INSERT dbo.Venta(id_cliente,id_usuario,fecha_venta,id_estado_venta,total,id_modalidad_entrega,descuento)
    VALUES(@Cliente,@Administrador,DATEADD(hour,12,CONVERT(datetime2,@Fecha)),@EstadoVenta,@Final,@Modalidad,0);
    SET @Venta=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Ventas(numero,id,fecha) VALUES(@i,@Venta,@Fecha);
    INSERT @Registros(tabla,id_registro,fecha) VALUES(N'Venta',@Venta,@Fecha);
    INSERT dbo.Detalle_Venta(id_venta,id_variante,cantidad,precio_unitario,subtotal,id_estado_variante_producto)
    VALUES(@Venta,@Variante,1,@Base,@Base,@EstadoDetalle);
    SET @Id=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Detalle_Venta',@Id);
    INSERT dbo.Pago(id_venta,id_usuario,id_medio_pago,cuotas,monto,monto_final,fecha_pago,tipo_caja,cuenta_destino)
    VALUES(@Venta,@Administrador,@Medio,1,@Base,@Final,DATEADD(hour,12,CONVERT(datetime2,@Fecha)),N'Efectivo',N'Caja principal');
    SET @Pago=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Pagos(numero,id) VALUES(@i,@Pago);
    INSERT @Registros(tabla,id_registro) VALUES(N'Pago',@Pago);
    INSERT dbo.Venta_Entrega(id_venta,domicilio,fecha_estimada,horario_sugerido,observaciones,tipo_logistica,costo_envio,observacion_pago,condicion_pago)
    VALUES(@Venta,CONCAT(N'Direccion de prueba ',@i),DATEADD(day,1,CONVERT(datetime2,@Fecha)),N'09:00 a 12:00',
        CONCAT(N'Lote ',@Token),N'Logística propia',0,N'Pago de prueba en efectivo',N'Pago total');
    INSERT @Registros(tabla,id_registro) VALUES(N'Venta_Entrega',@Venta);
    INSERT dbo.Remito(id_venta,fecha_emision) VALUES(@Venta,DATEADD(hour,12,CONVERT(datetime2,@Fecha)));
    SET @Remito=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Remito',@Remito);
    INSERT dbo.Detalle_Remito(id_remito,id_variante,cantidad,descripcion)
    VALUES(@Remito,@Variante,1,CONCAT(N'Producto de prueba ',@i,N' lote ',@Token));
    SET @IdDetalleRemito=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Detalle_Remito',@IdDetalleRemito);
    INSERT dbo.Envio(id_remito,direccion,horario_sugerido,tipo_envio,costo,id_estado_envio,fecha_entrega)
    VALUES(@Remito,CONCAT(N'Direccion de prueba ',@i),N'09:00 a 12:00',N'Envío a domicilio',0,@EstadoEnvio,
        DATEADD(day,1,CONVERT(datetime2,@Fecha)));
    SET @Envio=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro) VALUES(N'Envio',@Envio);
    SET @i=@i+1;
END;
SET @i=1;
WHILE @i<=100
BEGIN
    SELECT @Fecha=fecha,@Venta=id FROM @Ventas WHERE numero=@i;
    SELECT @Pago=id FROM @Pagos WHERE numero=@i;
    SELECT @NombreCliente=CONCAT(c.nombre_cliente,N' ',c.apellido_cliente)
    FROM dbo.Venta v JOIN dbo.Cliente c ON c.id_cliente=v.id_cliente WHERE v.id_venta=@Venta;
    SELECT @Movimientos=(
        SELECT p.id_pago AS Id,p.id_venta AS Venta,p.fecha_pago AS Fecha,
            CONCAT(@NombreCliente,N' - Efectivo') AS Concepto,N'Cobro venta' AS Tipo,
            N'Efectivo' AS Medio,N'ARS' AS Moneda,p.monto_final AS Importe,
            p.cuenta_destino AS Cuenta,CONVERT(nvarchar(20),p.id_usuario) AS Usuario
        FROM dbo.Pago p WHERE p.id_pago=@Pago FOR JSON PATH);
    SELECT @Datos=(
        SELECT 0 AS Numero,@Fecha AS Fecha,DATEADD(hour,9,CONVERT(datetime2,@Fecha)) AS Inicio,
            DATEADD(hour,18,CONVERT(datetime2,@Fecha)) AS Fin,CONVERT(decimal(10,2),0) AS SaldoInicial,
            CONVERT(decimal(10,2),0) AS SaldoInicialUsd,CONCAT(N'Lote de prueba ',@Token) AS Observaciones,
            u.nombre_usuario AS Usuario,JSON_QUERY(@Movimientos) AS Movimientos,JSON_QUERY(N'[]') AS Billetes
        FROM dbo.Usuario u WHERE u.id_usuario=@Administrador FOR JSON PATH,WITHOUT_ARRAY_WRAPPER);
    INSERT dbo.Caja_Cierre(fecha,id_usuario,fecha_cierre,datos)
    VALUES(@Fecha,@Administrador,DATEADD(hour,18,CONVERT(datetime2,@Fecha)),@Datos);
    SET @Id=CONVERT(int,SCOPE_IDENTITY());
    INSERT @Registros(tabla,id_registro,fecha) VALUES(N'Caja_Cierre',@Id,@Fecha);
    SET @i=@i+1;
END;
INSERT dbo.RegistroDatosPrueba(id_lote,tabla,id_registro,fecha)
SELECT @IdLote,tabla,id_registro,fecha FROM @Registros;
INSERT dbo.LogActividad(id_usuario,accion,entidad,id_entidad,detalle)
VALUES(@Administrador,N'CARGAR_DATOS_PRUEBA',N'LoteDatosPrueba',NULL,@Token);
SELECT SYSDATETIME(),MIN(fecha),MAX(fecha) FROM @Fechas;";

        // Conserva las comprobaciones y el borrado ordenado del lote de prueba.
        private const string ConsultaEliminacion = @"
SET XACT_ABORT ON;
IF NOT EXISTS(SELECT 1 FROM dbo.LoteDatosPrueba WITH (UPDLOCK,HOLDLOCK) WHERE id_lote=@IdLote AND activo=1)
    THROW 51010, N'El lote ya no esta activo.', 1;
IF EXISTS(SELECT 1 FROM dbo.Detalle_Venta d
    JOIN dbo.RegistroDatosPrueba rv ON rv.id_lote=@IdLote AND rv.tabla=N'Variante_Producto' AND rv.id_registro=d.id_variante
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba r WHERE r.id_lote=@IdLote AND r.tabla=N'Venta' AND r.id_registro=d.id_venta))
    THROW 51011, N'No se puede borrar el lote porque una variante de prueba se uso en otra venta.', 1;
IF EXISTS(SELECT 1 FROM dbo.Detalle_Ingreso d
    JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Variante_Producto' AND r.id_registro=d.id_variante)
    THROW 51012, N'No se puede borrar el lote porque una variante de prueba se uso en un ingreso de stock.', 1;
IF EXISTS(SELECT 1 FROM dbo.Detalle_Remito d
    JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Variante_Producto' AND r.id_registro=d.id_variante
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba x WHERE x.id_lote=@IdLote AND x.tabla=N'Detalle_Remito' AND x.id_registro=d.id_detalle_remito))
    THROW 51013, N'No se puede borrar el lote porque una variante de prueba se uso en un remito ajeno al lote.', 1;
IF EXISTS(SELECT 1 FROM dbo.Remito r
    JOIN dbo.RegistroDatosPrueba v ON v.id_lote=@IdLote AND v.tabla=N'Venta' AND v.id_registro=r.id_venta
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba x WHERE x.id_lote=@IdLote AND x.tabla=N'Remito' AND x.id_registro=r.id_remito))
    THROW 51014, N'No se puede borrar el lote porque una venta de prueba tiene un remito ajeno al lote.', 1;
IF EXISTS(SELECT 1 FROM dbo.Envio e
    JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Remito' AND r.id_registro=e.id_remito
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba x WHERE x.id_lote=@IdLote AND x.tabla=N'Envio' AND x.id_registro=e.id_envio))
    THROW 51022, N'No se puede borrar el lote porque un remito de prueba tiene un envio ajeno al lote.', 1;
IF EXISTS(SELECT 1 FROM dbo.Detalle_Remito d
    JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Remito' AND r.id_registro=d.id_remito
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba x WHERE x.id_lote=@IdLote AND x.tabla=N'Detalle_Remito' AND x.id_registro=d.id_detalle_remito))
    THROW 51023, N'No se puede borrar el lote porque un remito de prueba tiene detalles ajenos al lote.', 1;
IF EXISTS(SELECT 1 FROM dbo.Pago p
    JOIN dbo.RegistroDatosPrueba v ON v.id_lote=@IdLote AND v.tabla=N'Venta' AND v.id_registro=p.id_venta
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba r WHERE r.id_lote=@IdLote AND r.tabla=N'Pago' AND r.id_registro=p.id_pago))
    THROW 51015, N'No se puede borrar el lote porque una venta de prueba tiene un pago ajeno al lote.', 1;
IF EXISTS(SELECT 1 FROM dbo.Detalle_Venta d
    JOIN dbo.RegistroDatosPrueba v ON v.id_lote=@IdLote AND v.tabla=N'Venta' AND v.id_registro=d.id_venta
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba r WHERE r.id_lote=@IdLote AND r.tabla=N'Detalle_Venta' AND r.id_registro=d.id_detalle_venta))
    THROW 51016, N'No se puede borrar el lote porque una venta de prueba tiene detalles ajenos al lote.', 1;
IF EXISTS(SELECT 1 FROM dbo.Venta v
    JOIN dbo.RegistroDatosPrueba c ON c.id_lote=@IdLote AND c.tabla=N'Cliente' AND c.id_registro=v.id_cliente
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba r WHERE r.id_lote=@IdLote AND r.tabla=N'Venta' AND r.id_registro=v.id_venta))
    THROW 51017, N'No se puede borrar el lote porque un cliente de prueba tiene otra venta.', 1;
IF EXISTS(SELECT 1 FROM dbo.Recibo rc
    JOIN dbo.RegistroDatosPrueba rp ON rp.id_lote=@IdLote AND rp.tabla=N'Pago' AND rp.id_registro=rc.id_pago)
    THROW 51018, N'No se puede borrar el lote porque se emitio un recibo para un pago de prueba.', 1;
IF EXISTS(SELECT 1 FROM dbo.Producto p
    JOIN dbo.RegistroDatosPrueba c ON c.id_lote=@IdLote AND c.tabla=N'Categoria' AND c.id_registro=p.id_categoria
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba r WHERE r.id_lote=@IdLote AND r.tabla=N'Producto' AND r.id_registro=p.id_producto))
    THROW 51019, N'No se puede borrar el lote porque la categoria de prueba se uso en otro producto.', 1;
IF EXISTS(SELECT 1 FROM dbo.Producto p
    JOIN dbo.RegistroDatosPrueba m ON m.id_lote=@IdLote AND m.tabla=N'Marca' AND m.id_registro=p.id_marca
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba r WHERE r.id_lote=@IdLote AND r.tabla=N'Producto' AND r.id_registro=p.id_producto))
    THROW 51020, N'No se puede borrar el lote porque la marca de prueba se uso en otro producto.', 1;
IF EXISTS(SELECT 1 FROM dbo.Cliente c
    JOIN dbo.RegistroDatosPrueba l ON l.id_lote=@IdLote AND l.tabla=N'Localidad' AND l.id_registro=c.id_localidad
    WHERE NOT EXISTS(SELECT 1 FROM dbo.RegistroDatosPrueba r WHERE r.id_lote=@IdLote AND r.tabla=N'Cliente' AND r.id_registro=c.id_cliente))
    THROW 51021, N'No se puede borrar el lote porque la localidad de prueba se uso en otro cliente.', 1;
DELETE c FROM dbo.Caja_Cierre c JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Caja_Cierre' AND r.id_registro=c.id;
DELETE p FROM dbo.Pago p JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Pago' AND r.id_registro=p.id_pago;
DELETE e FROM dbo.Envio e JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Envio' AND r.id_registro=e.id_envio;
DELETE d FROM dbo.Detalle_Remito d JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Detalle_Remito' AND r.id_registro=d.id_detalle_remito;
DELETE d FROM dbo.Detalle_Venta d JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Detalle_Venta' AND r.id_registro=d.id_detalle_venta;
DELETE e FROM dbo.Venta_Entrega e JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Venta_Entrega' AND r.id_registro=e.id_venta;
DELETE r FROM dbo.Remito r JOIN dbo.RegistroDatosPrueba x ON x.id_lote=@IdLote AND x.tabla=N'Remito' AND x.id_registro=r.id_remito;
DELETE v FROM dbo.Venta v JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Venta' AND r.id_registro=v.id_venta;
DELETE v FROM dbo.Variante_Producto v JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Variante_Producto' AND r.id_registro=v.id_variante;
DELETE p FROM dbo.Producto p JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Producto' AND r.id_registro=p.id_producto;
DELETE c FROM dbo.Cliente c JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Cliente' AND r.id_registro=c.id_cliente;
DELETE c FROM dbo.Categoria c JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Categoria' AND r.id_registro=c.id_categoria;
DELETE m FROM dbo.Marca m JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Marca' AND r.id_registro=m.id_marca;
DELETE l FROM dbo.Localidad l JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Localidad' AND r.id_registro=l.id_localidad;
DELETE s FROM dbo.Estado_Venta s JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Estado_Venta' AND r.id_registro=s.id_estado_venta
    WHERE NOT EXISTS(SELECT 1 FROM dbo.Venta v WHERE v.id_estado_venta=s.id_estado_venta);
DELETE s FROM dbo.Estado_Variante_Producto s JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Estado_Variante_Producto' AND r.id_registro=s.id_estado_variante_producto
    WHERE NOT EXISTS(SELECT 1 FROM dbo.Detalle_Venta d WHERE d.id_estado_variante_producto=s.id_estado_variante_producto);
DELETE s FROM dbo.Estado_Envio s JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Estado_Envio' AND r.id_registro=s.id_estado_envio
    WHERE NOT EXISTS(SELECT 1 FROM dbo.Envio e WHERE e.id_estado_envio=s.id_estado_envio);
DELETE m FROM dbo.Modalidad_Entrega m JOIN dbo.RegistroDatosPrueba r ON r.id_lote=@IdLote AND r.tabla=N'Modalidad_Entrega' AND r.id_registro=m.id_modalidad_entrega
    WHERE NOT EXISTS(SELECT 1 FROM dbo.Venta v WHERE v.id_modalidad_entrega=m.id_modalidad_entrega);
DELETE FROM dbo.RegistroDatosPrueba WHERE id_lote=@IdLote;
UPDATE dbo.LoteDatosPrueba SET activo=0 WHERE id_lote=@IdLote;
INSERT dbo.LogActividad(id_usuario,accion,entidad,id_entidad,detalle)
VALUES(@Administrador,N'ELIMINAR_DATOS_PRUEBA',N'LoteDatosPrueba',NULL,@Token);";
    }
}
