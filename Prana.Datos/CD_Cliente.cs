using Microsoft.Data.SqlClient;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Prana.Datos
{
    // agrupa los datos y operaciones de cd cliente
    public class CD_Cliente
    {
        // metodo que inserta un nuevo cliente en la base de datos registra un cliente junto con su localidad
        public bool NuevoCliente(Cliente cliente)
        {
            // crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // abre la conexion con la base de datos abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // inicia una transaccion para guardar todo junto
            using SqlTransaction transaccion = conexion.BeginTransaction();
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                // obtiene la clave de localidad dentro de la misma transaccion del cliente
                int? idLocalidad = CD_Localidad.Asegurar(conexion, transaccion, cliente.NombreLocalidad);
                string consulta = "INSERT INTO Cliente (nombre_cliente, apellido_cliente, numero_documento, tipo_documento, telefono, fecha_hora_registro, direccion, saldo_a_favor, id_localidad) VALUES (@Nombre, @Apellido, @DNI, @TipoDocumento, @Telefono, @FechaRegistro, @Direccion, @SaldoFavor, @IdLocalidad)";
                // crea el comando que registrara el cliente
                using SqlCommand comando = new SqlCommand(consulta, conexion, transaccion);
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar, 11).Value = cliente.DNI;
                comando.Parameters.Add("@TipoDocumento", SqlDbType.NVarChar, 4).Value = cliente.TipoDocumento;
                // envia el telefono o un valor nulo si esta vacio
                if (string.IsNullOrWhiteSpace(cliente.Telefono))
                {
                    comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = DBNull.Value;
                }
                else
                {
                    comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = cliente.Telefono;
                }
                // envia la fecha de registro del cliente
                comando.Parameters.Add("@FechaRegistro", SqlDbType.DateTime).Value = cliente.FechaRegistro;
                // envia la direccion o un valor nulo si esta vacia
                if (string.IsNullOrWhiteSpace(cliente.Direccion))
                {
                    comando.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = DBNull.Value;
                }
                else
                {
                    comando.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = cliente.Direccion;
                }
                // envia el saldo a favor del cliente
                comando.Parameters.Add("@SaldoFavor", SqlDbType.Decimal).Value = cliente.SaldoFavor;
                // guarda la clave foranea o null si el cliente no tiene localidad
                comando.Parameters.Add("@IdLocalidad", SqlDbType.Int).Value = (object?)idLocalidad ?? DBNull.Value;
                // ejecuta la consulta y guarda la cantidad de filas modificadas
                int filasAfectadas = comando.ExecuteNonQuery();
                // confirma todos los cambios realizados
                transaccion.Commit();
                // devuelve verdadero si se registro el cliente
                return filasAfectadas > 0;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // deshace los cambios si ocurre un error
                transaccion.Rollback();
                // envia el error a la capa que llamo al metodo interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        // metodo que busca al cliente por nombre, apellido y telefono, si no se ingresa ningun parametro devuelve todos los clientes busca clientes por nombre apellido o telefono busca clientes buscarporprefijo se usa en nueva venta para que "40" encuentre datos que empiezan por 40
        public List<Cliente> buscarCliente(string? busqueda, int filtro = 0, bool buscarPorPrefijo = false)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            // recalcula activo antes de filtrar: lo mantiene si hay deuda o actividad valida dentro del ultimo ano la regla actual considera ventas, pagos, activacion/restauracion y fecha de registro
            string estadosSql = @"
                WITH Actividad AS
                (
                    -- Una venta no anulada ni cancelada cuenta como actividad en la fecha de venta.
                    SELECT v.id_cliente, v.fecha_hora_venta AS fecha
                    FROM Venta v
                    INNER JOIN Estado_Venta ev ON ev.id_estado_venta = v.id_estado_venta
                    WHERE ev.nombre_estado_venta NOT LIKE N'Anulad%'
                        AND ev.nombre_estado_venta NOT LIKE N'Cancelad%'
                    UNION ALL
                    -- También cuenta un pago de una venta vigente, aunque sea parcial.
                    SELECT v.id_cliente, p.fecha_hora_pago
                    FROM Pago p
                    INNER JOIN Venta v ON v.id_venta = p.id_venta
                    INNER JOIN Estado_Venta ev ON ev.id_estado_venta = v.id_estado_venta
                    WHERE ev.nombre_estado_venta NOT LIKE N'Anulad%'
                        AND ev.nombre_estado_venta NOT LIKE N'Cancelad%'
                    UNION ALL
                    SELECT id_entidad, fecha_hora_actividad FROM Log_Actividad
                    WHERE entidad = N'Cliente'
                        AND accion IN (N'ACTIVAR_CLIENTE', N'RESTAURAR_CLIENTE')
                        AND id_entidad IS NOT NULL
                    UNION ALL
                    -- Si todavía no hay ventas ni pagos, la fecha de alta sirve como actividad inicial.
                    SELECT id_cliente, fecha_hora_registro FROM Cliente
                ), UltimaActividad AS
                (
                    -- Conserva solo la fecha más reciente de todas las actividades anteriores.
                    SELECT id_cliente, MAX(fecha) AS fecha FROM Actividad GROUP BY id_cliente
                ), Deudas AS
                (
                    -- Suma los saldos pendientes de las ventas vigentes del cliente.
                    SELECT id_cliente, SUM(saldo) AS saldo
                    FROM (" + CD_Venta.SaldosSql + @") saldos
                    GROUP BY id_cliente
                )
                -- Una baja lógica siempre prevalece; deuda o actividad en el último año mantienen Activo.
                UPDATE c
                SET activo = CASE
                    WHEN c.eliminado = 1 THEN 0
                    WHEN COALESCE(d.saldo, 0) > 0 OR ua.fecha >= DATEADD(YEAR, -1, SYSDATETIME()) THEN 1
                    ELSE 0
                END
                FROM Cliente c
                LEFT JOIN UltimaActividad ua ON ua.id_cliente = c.id_cliente
                LEFT JOIN Deudas d ON d.id_cliente = c.id_cliente;";
            // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
            using (SqlCommand actualizar = new SqlCommand(estadosSql, conexion))
            {
                actualizar.ExecuteNonQuery();
            }

            List<Cliente> clientesEncontrados = new List<Cliente>();
            // resuelve el nombre visible de la localidad desde su clave foranea
            // los numeros son los filtros del formulario de clientes; 4 significa todos menos los eliminados
            string consulta = @"SELECT c.id_cliente, c.nombre_cliente, c.apellido_cliente,
                    c.numero_documento, c.tipo_documento, c.telefono, c.fecha_hora_registro,
                    c.direccion, c.saldo_a_favor, c.activo, c.eliminado,
                    COALESCE(d.saldo, 0) AS deuda_pendiente, l.nombre_localidad AS localidad
                FROM Cliente c
                LEFT JOIN dbo.Localidad l ON l.id_localidad=c.id_localidad
                LEFT JOIN (SELECT id_cliente, SUM(saldo) AS saldo FROM (" + CD_Venta.SaldosSql + @") saldos GROUP BY id_cliente) d ON d.id_cliente = c.id_cliente
                -- LIKE recibe el patrón completo preparado en C# (por ejemplo, '40%' para prefijo).
                WHERE (@busqueda IS NULL OR c.nombre_cliente LIKE @busqueda OR c.apellido_cliente LIKE @busqueda OR CONCAT(LTRIM(RTRIM(c.nombre_cliente)), ' ', LTRIM(RTRIM(c.apellido_cliente))) LIKE @busqueda OR c.numero_documento LIKE @busqueda OR c.telefono LIKE @busqueda)
                    -- 0 activos; 1 inactivos; 2 eliminados; 3 todos; 4 activos e inactivos, nunca eliminados.
                    AND (@filtro = 3 OR (@filtro = 0 AND c.eliminado = 0 AND c.activo = 1) OR (@filtro = 1 AND c.eliminado = 0 AND c.activo = 0) OR (@filtro = 2 AND c.eliminado = 1) OR (@filtro = 4 AND c.eliminado = 0))
                ORDER BY c.apellido_cliente, c.nombre_cliente";
            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // si no hay texto, null quita la condicion de busqueda; si hay, usa prefijo o coincidencia en cualquier parte
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                comando.Parameters.Add("@busqueda", SqlDbType.NVarChar, 200).Value = DBNull.Value;
            }
            else
            {
                if (buscarPorPrefijo)
                {
                    comando.Parameters.Add("@busqueda", SqlDbType.NVarChar, 200).Value = busqueda + "%";
                }
                else
                {
                    comando.Parameters.Add("@busqueda", SqlDbType.NVarChar, 200).Value = "%" + busqueda + "%";
                }
            }
            comando.Parameters.Add("@filtro", SqlDbType.Int).Value = filtro;
            // ejecuta la consulta
            using SqlDataReader lector = comando.ExecuteReader();
            // recorre los clientes encontrados avanza el lector de sql una fila por vez hasta agotar el resultado
            while (lector.Read())
            {
                decimal valorSaldoFavor;
                if (lector["saldo_a_favor"] == DBNull.Value)
                {
                    valorSaldoFavor = 0;
                }
                else
                {
                    valorSaldoFavor = lector.GetDecimal(lector.GetOrdinal("saldo_a_favor"));
                }

                // crea el cliente obtenido
                Cliente cliente = new Cliente
                {
                    IdCliente = lector.GetInt32(lector.GetOrdinal("id_cliente")),
                    Nombre = lector["nombre_cliente"]?.ToString() ?? string.Empty,
                    Apellido = lector["apellido_cliente"]?.ToString() ?? string.Empty,
                    DNI = lector["numero_documento"]?.ToString() ?? string.Empty,
                    TipoDocumento = lector["tipo_documento"].ToString() ?? "DNI",
                    Telefono = lector["telefono"]?.ToString() ?? string.Empty,
                    FechaRegistro = lector.GetDateTime(lector.GetOrdinal("fecha_hora_registro")),
                    Direccion = lector["direccion"]?.ToString() ?? string.Empty,
                    SaldoFavor = valorSaldoFavor,
                    Activo = lector.GetBoolean(lector.GetOrdinal("activo")),
                    Eliminado = lector.GetBoolean(lector.GetOrdinal("eliminado")),
                    DeudaPendiente = lector.GetDecimal(lector.GetOrdinal("deuda_pendiente")),
                    NombreLocalidad = lector["localidad"]?.ToString() ?? string.Empty

                };
                // agrega el cliente a los resultados
                clientesEncontrados.Add(cliente);
            }

            // devuelve los clientes encontrados
            return clientesEncontrados;
        }

        // resuelve dar de baja cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        public void DarDeBajaCliente(int idCliente, int idUsuario) => CambiarEstadoCliente(idCliente, idUsuario, "BAJA_CLIENTE", true, false);
        // resuelve activar cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        public void ActivarCliente(int idCliente, int idUsuario) => CambiarEstadoCliente(idCliente, idUsuario, "ACTIVAR_CLIENTE", false, true);
        // resuelve restaurar cliente con los datos recibidos y entrega el resultado al codigo que lo llama
        public void RestaurarCliente(int idCliente, int idUsuario) => CambiarEstadoCliente(idCliente, idUsuario, "RESTAURAR_CLIENTE", false, true);
        // coordina cambiar estado cliente dentro de una transaccion para confirmar o revertir sus escrituras juntas
        private static void CambiarEstadoCliente(int idCliente, int idUsuario, string accion, bool eliminado, bool activo)
        {
            // prepara la conexion configurada y la libera al terminar este alcance
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            conexion.Open();
            using SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable);
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand bloquearCliente = new SqlCommand("SELECT id_cliente FROM Cliente WITH (UPDLOCK, HOLDLOCK) WHERE id_cliente = @cliente;", conexion, transaccion);
                bloquearCliente.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                if (bloquearCliente.ExecuteScalar() == null)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El cliente no existe.");
                }

                if (accion == "BAJA_CLIENTE")
                {
                    // prepara la consulta y su conexion los valores variables se envian mediante parametros
                    using SqlCommand validarBaja = new SqlCommand(@"
                        SELECT
                            CASE WHEN EXISTS (
                                SELECT 1 FROM (" + CD_Venta.SaldosSql + @") saldos
                                WHERE saldos.id_cliente = @cliente AND saldos.saldo > 0
                            ) THEN 1 ELSE 0 END AS tiene_deuda,
                            CASE WHEN EXISTS (
                                SELECT 1
                                FROM Venta v
                                INNER JOIN Estado_Venta ev ON ev.id_estado_venta = v.id_estado_venta
                                INNER JOIN (
                                    SELECT id_venta, id_variante, SUM(cantidad) cantidad
                                    FROM Detalle_Venta GROUP BY id_venta, id_variante
                                ) dv ON dv.id_venta = v.id_venta
                                WHERE v.id_cliente = @cliente
                                    AND ev.nombre_estado_venta NOT LIKE N'Anulad%'
                                    AND ev.nombre_estado_venta NOT LIKE N'Cancelad%'
                                    AND dv.cantidad > (
                                        SELECT COALESCE(SUM(dr.cantidad), 0)
                                        FROM Remito r
                                        INNER JOIN Detalle_Remito dr ON dr.id_remito = r.id_remito
                                        WHERE r.id_venta = dv.id_venta AND dr.id_variante = dv.id_variante
                                          AND EXISTS (
                                              SELECT 1 FROM Envio e
                                              WHERE e.id_remito = r.id_remito AND e.entregado = 1
                                          )
                                    )
                            ) THEN 1 ELSE 0 END AS tiene_pedidos_pendientes;", conexion, transaccion);
                    validarBaja.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                    using SqlDataReader estadoBaja = validarBaja.ExecuteReader();
                    estadoBaja.Read();
                    bool tieneDeuda = estadoBaja.GetInt32(0) == 1;
                    bool tienePedidosPendientes = estadoBaja.GetInt32(1) == 1;
                    estadoBaja.Close();
                    if (tieneDeuda || tienePedidosPendientes)
                    {
                        string motivos;
                        if (tieneDeuda && tienePedidosPendientes)
                        {
                            motivos = "deuda y pedidos pendientes de entrega";
                        }
                        else
                        {
                            if (tieneDeuda)
                            {
                                motivos = "deuda pendiente";
                            }
                            else
                            {
                                motivos = "pedidos pendientes de entrega";
                            }
                        }
                        // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                        throw new InvalidOperationException($"No se puede dar de baja al cliente porque tiene {motivos}.");
                    }
                }

                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand actualizar = new SqlCommand(@"
                    UPDATE Cliente SET eliminado = @eliminado, activo = @activo
                    WHERE id_cliente = @cliente
                        AND (@restaurar = 0 OR eliminado = 1)
                        AND (@activar = 0 OR (eliminado = 0 AND activo = 0))
                        AND (@baja = 0 OR eliminado = 0);", conexion, transaccion);
                actualizar.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                actualizar.Parameters.Add("@eliminado", SqlDbType.Bit).Value = eliminado;
                actualizar.Parameters.Add("@activo", SqlDbType.Bit).Value = activo;
                actualizar.Parameters.Add("@restaurar", SqlDbType.Bit).Value = accion == "RESTAURAR_CLIENTE";
                actualizar.Parameters.Add("@activar", SqlDbType.Bit).Value = accion == "ACTIVAR_CLIENTE";
                actualizar.Parameters.Add("@baja", SqlDbType.Bit).Value = accion == "BAJA_CLIENTE";
                if (actualizar.ExecuteNonQuery() != 1)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El cliente ya no está en el estado esperado.");
                }

                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand historial = new SqlCommand(@"
                    INSERT INTO Log_Actividad(id_usuario, accion, entidad, id_entidad, detalle)
                    VALUES(@usuario, @accion, N'Cliente', @cliente, @detalle);", conexion, transaccion);
                historial.Parameters.Add("@usuario", SqlDbType.Int).Value = idUsuario;
                historial.Parameters.Add("@accion", SqlDbType.NVarChar, 100).Value = accion;
                historial.Parameters.Add("@cliente", SqlDbType.Int).Value = idCliente;
                historial.Parameters.Add("@detalle", SqlDbType.NVarChar, 1000).Value = accion switch
                {
                    "BAJA_CLIENTE" => "Cliente dado de baja logicamente",
                    "ACTIVAR_CLIENTE" => "Cliente activado",
                    _ => "Cliente restaurado"
                };
                historial.ExecuteNonQuery();
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

        // modifica los datos de un cliente y su localidad
        public bool ModificarCliente(Cliente cliente)
        {
            // crea la conexion con la base de datos
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // abre la conexion con la base de datos abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // inicia una transaccion para guardar todos los cambios juntos
            using SqlTransaction transaccion = conexion.BeginTransaction();
            // protege el acceso a los datos para atender los errores de la operacion
            try
            {
                // obtiene la clave de localidad dentro de la misma transaccion del cliente
                int? idLocalidad = CD_Localidad.Asegurar(conexion, transaccion, cliente.NombreLocalidad);
                string consulta = "UPDATE Cliente SET nombre_cliente = @Nombre, apellido_cliente = @Apellido, numero_documento = @DNI, tipo_documento = @TipoDocumento, telefono = @Telefono, direccion = @Direccion, id_localidad = @IdLocalidad WHERE id_cliente = @IdCliente AND eliminado = 0";
                // crea el comando que ejecutara la modificacion
                using SqlCommand comando = new SqlCommand(consulta, conexion, transaccion);
                // envia el identificador del cliente
                comando.Parameters.Add("@IdCliente", SqlDbType.Int).Value = cliente.IdCliente;
                // envia el nombre del cliente
                comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 100).Value = cliente.Nombre;
                // envia el apellido del cliente
                comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 100).Value = cliente.Apellido;
                // envia el dni del cliente
                comando.Parameters.Add("@DNI", SqlDbType.NVarChar, 11).Value = cliente.DNI;
                comando.Parameters.Add("@TipoDocumento", SqlDbType.NVarChar, 4).Value = cliente.TipoDocumento;
                // envia el telefono o un valor nulo si esta vacio
                if (string.IsNullOrWhiteSpace(cliente.Telefono))
                {
                    comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = DBNull.Value;
                }
                else
                {
                    comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 100).Value = cliente.Telefono;
                }
                // envia la direccion o un valor nulo si esta vacia
                if (string.IsNullOrWhiteSpace(cliente.Direccion))
                {
                    comando.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = DBNull.Value;
                }
                else
                {
                    comando.Parameters.Add("@Direccion", SqlDbType.NVarChar, 200).Value = cliente.Direccion;
                }
                // guarda la clave foranea o null si el cliente no tiene localidad
                comando.Parameters.Add("@IdLocalidad", SqlDbType.Int).Value = (object?)idLocalidad ?? DBNull.Value;
                // ejecuta la consulta y obtiene las filas modificadas
                int filasAfectadas = comando.ExecuteNonQuery();
                // confirma todos los cambios realizados
                transaccion.Commit();
                // devuelve verdadero si se modifico el cliente
                return filasAfectadas > 0;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch
            {
                // deshace todos los cambios si ocurre un error
                transaccion.Rollback();
                // envia el error a la capa que llamo al metodo interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw;
            }
        }

        // comprueba si el numero de documento pertenece a otro cliente
        public bool ExisteClientePorDNI(string dni, int idClienteExcluir)
        {
            // busca el numero de documento ignorando el cliente indicado
            string consulta = "SELECT COUNT(*) FROM Cliente WHERE numero_documento = @DNI AND id_cliente <> @IdClienteExcluir";
            // crea la conexion
            using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
            // crea el comando
            using SqlCommand comando = new SqlCommand(consulta, conexion);
            // envia el dni
            comando.Parameters.Add("@DNI", SqlDbType.NVarChar, 20).Value = dni;
            // envia el identificador que debe ignorarse
            comando.Parameters.Add("@IdClienteExcluir", SqlDbType.Int).Value = idClienteExcluir;
            // abre la conexion abre el recurso antes de usarlo; una conexion debe estar abierta para enviar consultas
            conexion.Open();
            // obtiene la cantidad de coincidencias
            int cantidadClientes = Convert.ToInt32(comando.ExecuteScalar());
            // devuelve verdadero si encontro otro cliente
            return cantidadClientes > 0;
        }
    }
}
