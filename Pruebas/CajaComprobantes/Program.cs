using Microsoft.Data.SqlClient;
using Prana.Datos;
using Prana.Entidades;
using Prana.negocio;
using Prana.Presentacion;
using System.Text.RegularExpressions;
using System.Text.Json;

// agrupa los datos y operaciones de pruebas
class Pruebas
{
    // conserva base entre las operaciones de esta clase
    const string Base = "PranaCajaPruebas_20260923";
    // resuelve ok con los datos recibidos y entrega el resultado al codigo que lo llama
    static void Ok(bool valor, string mensaje)
    {
        if (!valor)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception(mensaje);
        }

        Console.WriteLine("OK: " + mensaje);
    }

    // resuelve rechaza con los datos recibidos y entrega el resultado al codigo que lo llama
    static void Rechaza(Action accion, string mensaje)
    {
        try
        {
            accion();
        }
        // atiende el error de esta operacion antes de informar o propagar el problema
        catch (Exception error) when (error is InvalidOperationException || error is ArgumentException || error is SqlException)
        {
            Console.WriteLine("OK: rechaza " + mensaje);
            return;
        }

        // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
        throw new Exception("No rechazó " + mensaje);
    }

    // ejecuta la consulta de sql y obtiene el primer valor del resultado
    static object? Sql(string texto)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        // prepara la consulta y su conexion los valores variables se envian mediante parametros
        using SqlCommand comando = new SqlCommand(texto, conexion);
        return comando.ExecuteScalar();
    }

    // envia los parametros de script y ejecuta la escritura en la base de datos
    static void Script(string archivo)
    {
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        string texto = File.ReadAllText(archivo);
        Ok(!Regex.IsMatch(texto, @"(?im)^\s*(USE|CREATE DATABASE)\b"), "Script aislado: " + Path.GetFileName(archivo));
        // procesa cada lote de regexsplit(texto, @"(?im)^\s*go\s*$") sin cambiar el orden de la coleccion
        foreach (string lote in Regex.Split(texto, @"(?im)^\s*GO\s*$"))
        {
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(lote))
            {
                continue;
            }

            // prepara la consulta y su conexion los valores variables se envian mediante parametros
            using SqlCommand comando = new SqlCommand(lote, conexion);
            comando.ExecuteNonQuery();
        }
        if (Path.GetFileName(archivo) == "CajaYComprobantes.sql")
        {
            Script("BaseDeDatos/CajasPorTurno.sql");
        }
    }

    // resuelve solicitud con los datos recibidos y entrega el resultado al codigo que lo llama
    static venta Solicitud(decimal pago = 400)
    {
        List<Pago> valorPagos;
        if (pago > 0)
        {
            valorPagos = new()
        {
            new()
            {
                IdMedioPago = 1,
                Monto = pago,
                Cuotas = 1

}
        };
        }
        else
        {
            valorPagos = new();
        }

        return new()
        {
            IdCliente = 1,
            IdUsuario = 1,
            FechaPago = DateTime.Today,
            FechaEntrega = DateTime.Today,
            HorarioSugerido = "14 a 18",
            ObservacionesEntrega = "Llamar antes",
            ObservacionPago = "Pago parcial registrado",
            TipoLogistica = "Flete",
            Total = 1000,
            CondicionPago = "Pago parcial",
            ModalidadEntrega = "Retiro en el local",
            Detalles = new()
        {
            new()
            {
                IdVariante = 1,
                Cantidad = 1,
                PrecioUnitario = 1000

}
        },
            Pagos = valorPagos
        };
    }

    // comprueba recuperacion de datos cierres atrasados y dos intentos simultaneos en una base aislada
    static void PruebasCierreAutomatico()
    {
        PruebasTurnosCaja();
    }

    // ejecuta los lotes de la migracion juntos como lo hace el inicializador real
    static void MigrarCaja()
    {
        using SqlConnection conexion = new SqlConnection(Conexion.ObtenerCadena());
        conexion.Open();
        using SqlTransaction transaccion = conexion.BeginTransaction();
        try
        {
            string texto = File.ReadAllText("BaseDeDatos/CajaYComprobantes.sql") + "\nGO\n" + File.ReadAllText("BaseDeDatos/CajasPorTurno.sql");
            foreach (string lote in Regex.Split(texto, @"(?im)^\s*GO\s*$"))
            {
                if (string.IsNullOrWhiteSpace(lote)) continue;
                using SqlCommand comando = new SqlCommand(lote, conexion, transaccion);
                comando.ExecuteNonQuery();
            }
            transaccion.Commit();
        }
        catch { transaccion.Rollback(); throw; }
    }

    // comprueba conservacion de datos idempotencia y cierre de la misma fila en una base exclusiva de pruebas
    static void PruebasCajaUnificada()
    {
        // reconstruye la estructura anterior solamente en la base aislada y vacia de esta prueba
        Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Caja")) == 0, "Migracion ensayada sobre cajas de prueba vacias");
        Sql(@"ALTER TABLE Pago DROP CONSTRAINT FK_Pago_Caja;
            ALTER TABLE Caja_Movimiento DROP CONSTRAINT FK_CajaMovimiento_Caja;
            DROP INDEX IX_Pago_Caja ON Pago;
            DROP INDEX IX_CajaMovimiento_Caja ON Caja_Movimiento;
            ALTER TABLE Pago ALTER COLUMN id_caja int NULL;
            ALTER TABLE Caja_Movimiento ALTER COLUMN id_caja int NULL;
            DROP TABLE Caja;
            CREATE TABLE Caja_Cierre(id int PRIMARY KEY,fecha date UNIQUE,id_usuario int NULL,fecha_cierre datetime2,datos nvarchar(max));
            INSERT Caja_Cierre VALUES(77,DATEADD(day,-3,CONVERT(date,SYSDATETIME())),1,SYSDATETIME(),N'{""SaldoInicial"":350,""Observaciones"":""CIERRE ANTERIOR""}');
            CREATE TABLE Caja_Abierta(fecha date PRIMARY KEY,datos nvarchar(max));
            INSERT Caja_Abierta VALUES(CONVERT(date,SYSDATETIME()),N'{""SaldoInicial"":125,""Observaciones"":""CAJA INICIAL""}'),
                (DATEADD(day,-2,CONVERT(date,SYSDATETIME())),N'{""SaldoInicial"":50}');
            INSERT Caja_Movimiento(fecha,concepto,tipo,medio,moneda,importe,cuenta,id_usuario)
                VALUES(DATEADD(day,-2,SYSDATETIME()),N'Anterior',N'Ingreso extra',N'Efectivo','ARS',15,N'',1);");
        MigrarCaja();
        CD_Caja datos = new CD_Caja();
        CierreCaja anterior = datos.Consultar(DateTime.Today.AddDays(-3));
        Ok(anterior.Cerrada && anterior.Numero == 77 && anterior.SaldoInicial == 350 && anterior.Observaciones == "CIERRE ANTERIOR", "Conserva los datos y el numero del cierre anterior");
        CierreCaja recuperada = datos.Consultar(DateTime.Today.AddDays(-2));
        Ok(recuperada.Cerrada && recuperada.Usuario == "CIERRE AUTOMÁTICO" && recuperada.BalanceArs == 65, "La migracion completa dias abiertos anteriores y conserva sus movimientos");
        Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM sys.tables WHERE name IN('Caja_Abierta','Caja_Cierre')")) == 0, "No conserva las tablas separadas anteriores");
        CierreCaja hoy = datos.Consultar(DateTime.Today);
        Ok(!hoy.Cerrada && hoy.SaldoInicial == 125 && hoy.IdCaja > 0, "Conserva la apertura actual con su saldo");
        datos.Cerrar(hoy, 1);
        CierreCaja otra = datos.Abrir(new CierreCaja { SaldoInicial = 200 }, 1);
        Ok(otra.IdCaja != hoy.IdCaja && datos.Listar(DateTime.Today).Count == 2, "La migracion permite otra apertura del mismo dia");
        MigrarCaja();
        Ok(datos.Listar(DateTime.Today).Count == 2 && datos.Consultar(DateTime.Today, otra.IdCaja).SaldoInicial == 200, "Repetir la migracion no duplica ni altera aperturas");
    }
    // comprueba dos cierres del mismo dia separacion de cobros y recuperacion de un turno nocturno
    static void PruebasTurnosCaja()
    {
        SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
        CD_Caja datos = new CD_Caja();
        Ok(datos.Listar(DateTime.Today).Count == 0, "Consultar no abre una caja automaticamente");
        Rechaza(() => CD_RegistroVenta.Guardar(Solicitud(), false), "vender antes de abrir la caja");
        CierreCaja primeraCaja = datos.Abrir(new CierreCaja { SaldoInicial = 100 }, 1);
        Rechaza(() => datos.Abrir(new CierreCaja(), 1), "abrir dos cajas simultaneas");
        int ventaCaja = CD_RegistroVenta.Guardar(Solicitud(), false);
        datos.Registrar(new MovimientoCaja { IdCaja = primeraCaja.IdCaja, Concepto = "Ingreso", Tipo = "Ingreso extra", Importe = 50 }, 1);
        datos.Registrar(new MovimientoCaja { IdCaja = primeraCaja.IdCaja, Concepto = "Gasto", Tipo = "Gasto", Importe = 10 }, 1);
        CierreCaja cierreUno = datos.Cerrar(datos.Consultar(DateTime.Today, primeraCaja.IdCaja), 1);
        Ok(cierreUno.Cerrada && cierreUno.BalanceArs == 540 && cierreUno.Movimientos.All(item => item.IdCaja == primeraCaja.IdCaja), "Primer cierre incluye solo su apertura");
        Rechaza(() => CD_RegistroVenta.Guardar(Solicitud(), false), "vender entre el cierre y la siguiente apertura");
        CierreCaja segundaCaja = datos.Abrir(new CierreCaja { SaldoInicial = 20 }, 1);
        Ok(segundaCaja.IdCaja != primeraCaja.IdCaja && segundaCaja.Movimientos.Count == 0 && segundaCaja.SaldoInicial == 20, "La segunda apertura tiene saldo y movimientos independientes");
        Rechaza(() => datos.GuardarCajaAbierta(primeraCaja), "guardar desde una pantalla del turno anterior");
        Rechaza(() => datos.Registrar(new MovimientoCaja { IdCaja = primeraCaja.IdCaja, Concepto = "Viejo", Importe = 1 }, 1), "agregar desde una seleccion desactualizada");
        int movimientoAnterior = cierreUno.Movimientos.First(item => !item.Venta.HasValue).Id;
        Rechaza(() => datos.Anular(movimientoAnterior, "Fuera de turno", 1), "anular movimientos del turno cerrado");
        Sql("UPDATE Medio_Pago SET recargo_medio_pago=10 WHERE id_medio_pago=1");
        new CD_Pago().Registrar(new Pago { IdVenta = ventaCaja, IdMedioPago = 1, Cuotas = 1, ImporteBase = 100, Porcentaje = 10, Recargo = 10 }, 1);
        datos.Registrar(new MovimientoCaja { IdCaja = segundaCaja.IdCaja, Concepto = "Ingreso", Tipo = "Ingreso extra", Importe = 30 }, 1);
        CierreCaja desactualizada = datos.Consultar(DateTime.Today, segundaCaja.IdCaja);
        datos.Registrar(new MovimientoCaja { IdCaja = segundaCaja.IdCaja, Concepto = "Gasto", Tipo = "Gasto", Importe = 5 }, 1);
        Rechaza(() => datos.Cerrar(desactualizada, 1), "cerrar con movimientos sin actualizar");
        CierreCaja cierreDos = datos.Cerrar(datos.Consultar(DateTime.Today, segundaCaja.IdCaja), 1);
        Ok(cierreDos.BalanceArs == 145 && cierreDos.Ingresos("ARS") == 130, "Segundo cierre suma la base del pago sin recargo");
        Ok(datos.Listar(DateTime.Today).Count == 2 && datos.Consultar(DateTime.Today, primeraCaja.IdCaja).BalanceArs == 540, "Los dos cierres se consultan sin mezclar importes");
        Ok(Convert.ToInt32(Sql($"SELECT id_caja FROM Pago WHERE id_venta={ventaCaja} AND monto=400")) == primeraCaja.IdCaja
            && Convert.ToInt32(Sql($"SELECT id_caja FROM Pago WHERE id_venta={ventaCaja} AND monto=100")) == segundaCaja.IdCaja, "Una venta puede cobrarse en aperturas diferentes");
        ResumenMensualCaja mensual = new CN_Caja().ConsultarMes(DateTime.Today);
        Ok(mensual.Totales.Single(item => item.Moneda == "ARS").Resultado == 565 && mensual.Movimientos.Count == 6, "Resumen mensual no duplica cierres ni suma saldos iniciales");
        // dos intentos de apertura concurrentes deben producir una sola caja vigente
        bool IntentarAbrir()
        {
            try { new CD_Caja().Abrir(new CierreCaja(), 1); return true; }
            catch (InvalidOperationException) { return false; }
        }
        Task<bool> intentoUno = Task.Run(IntentarAbrir);
        Task<bool> intentoDos = Task.Run(IntentarAbrir);
        Task.WaitAll(intentoUno, intentoDos);
        Ok(intentoUno.Result != intentoDos.Result, "Solo una apertura simultanea tiene exito");
        CierreCaja nocturna = datos.Listar(DateTime.Today).Single(item => !item.Cerrada);
        Sql($"UPDATE Caja SET fecha=DATEADD(day,-1,CONVERT(date,SYSDATETIME())),inicio=DATEADD(day,-1,SYSDATETIME()) WHERE id={nocturna.IdCaja}");
        datos.CerrarDiasAnteriores();
        CierreCaja automatica = datos.Consultar(DateTime.Today.AddDays(-1), nocturna.IdCaja);
        Ok(automatica.Cerrada && automatica.Usuario == "CIERRE AUTOMÁTICO" && automatica.Fin == DateTime.Today, "Cierra la apertura nocturna sin tocar otros turnos");
        Script("BaseDeDatos/CajaYComprobantes.sql");
        Ok(datos.Listar(DateTime.Today).Count == 2 && datos.Consultar(DateTime.Today, primeraCaja.IdCaja).BalanceArs == 540, "Repetir la migracion conserva los turnos y sus datos");
    }

    // punto de entrada: prepara el entorno y ejecuta el flujo inicial definido en este programa
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Contains("--turnos-validaciones"))
        {
            try
            {
                CN_Caja cajaNegocio = new CN_Caja();
                SesionActual.CerrarSesion();
                Rechaza(() => cajaNegocio.Abrir(new CierreCaja()), "abrir caja sin sesion");
                SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
                Rechaza(() => cajaNegocio.Abrir(new CierreCaja { SaldoInicial = -1 }), "abrir con saldo negativo antes de consultar sql");
                Rechaza(() => cajaNegocio.Abrir(new CierreCaja { Observaciones = new string('A', 1001) }), "abrir con observaciones demasiado largas");
                // presenta estados simulados sin cargar el formulario ni consultar la base
                using frmCaja pantalla = new frmCaja();
                System.Reflection.BindingFlags privados = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
                CierreCaja turno = new CierreCaja { IdCaja = 7, Fecha = DateTime.Today };
                typeof(frmCaja).GetField("actual", privados)!.SetValue(pantalla, turno);
                typeof(frmCaja).GetField("cajasDelDia", privados)!.SetValue(pantalla, new List<CierreCaja> { turno });
                System.Reflection.MethodInfo mostrar = typeof(frmCaja).GetMethod("MostrarCaja", privados)!;
                mostrar.Invoke(pantalla, null);
                Ok(pantalla.Controls.Find("registrar", true).Single().Enabled && !pantalla.Controls.Find("abrir", true).Single().Enabled, "Una caja abierta permite registrar y bloquea otra apertura");
                turno.Cerrada = true;
                turno.Numero = 7;
                turno.Fin = DateTime.Now;
                mostrar.Invoke(pantalla, null);
                Ok(!pantalla.Controls.Find("registrar", true).Single().Enabled && pantalla.Controls.Find("abrir", true).Single().Enabled, "Una caja cerrada permite preparar otro turno sin admitir movimientos");
                typeof(frmCaja).GetField("actual", privados)!.SetValue(pantalla, new CierreCaja());
                mostrar.Invoke(pantalla, null);
                Ok(pantalla.Controls.Find("saldo", true).Single().Enabled && !pantalla.Controls.Find("cerrar", true).Single().Enabled, "Antes de abrir se puede completar el saldo pero no cerrar");
                return;
            }
            finally { SesionActual.CerrarSesion(); }
        }
        if (args.Contains("--permisos-opciones-stock"))
        {
            try
            {
                CN_OpcionStock opcionesStock = new CN_OpcionStock();
                SesionActual.CerrarSesion();
                Rechaza(() => opcionesStock.Obtener(TipoOpcionStock.Marca), "consultar opciones sin sesion");
                SesionActual.IniciarSesion(new usuario { idUsuario = 2, IdRolUsuario = 2 });
                Ok(!Permisos.TienePermiso(Permisos.AdministrarOpcionesStock), "El vendedor no administra opciones de stock");
                Rechaza(() => opcionesStock.Obtener(TipoOpcionStock.Color), "consultar opciones como vendedor");
                Rechaza(() => opcionesStock.Guardar(TipoOpcionStock.Marca, new OpcionStock(), "MARCA", null, false), "modificar opciones como vendedor");
                Rechaza(() => opcionesStock.Guardar(TipoOpcionStock.Marca, new OpcionStock(), "", null, true), "eliminar opciones como vendedor");
                using (frmStock pantallaStock = new frmStock())
                {
                    Control botonOpciones = pantallaStock.Controls.Find("btnOpcionesStock", true).Single();
                    Ok(!botonOpciones.Visible, "El boton de administracion no se muestra para vendedores");
                }
                SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
                Ok(Permisos.TienePermiso(Permisos.AdministrarOpcionesStock), "El administrador puede gestionar opciones");
                Rechaza(() => opcionesStock.Guardar(TipoOpcionStock.Color, new OpcionStock(), new string('A', 51), null, false), "color demasiado largo antes de acceder a la base");
                using frmOpcionesStock formularioOpciones = new frmOpcionesStock();
                Ok(formularioOpciones.Controls.Count > 0, "La ventana se construye con controles nativos");
                return;
            }
            finally { SesionActual.CerrarSesion(); }
        }
        if (args.Contains("--permisos-entregas"))
        {
            try
            {
                CN_Entrega entregas = new CN_Entrega();
                SesionActual.CerrarSesion();
                Rechaza(() => entregas.Confirmar(0), "confirmar entrega sin sesion");
                SesionActual.IniciarSesion(new usuario { idUsuario = 2, IdRolUsuario = 2 });
                Ok(Permisos.TienePermiso(Permisos.ConsultarEntregas) && Permisos.TienePermiso(Permisos.GestionarEntregas),
                    "El vendedor puede consultar preparar y confirmar entregas");
                // procesa cada accion de new action[] { () => entregaspreparar(new producto por entregar(), 0, "", ""), () => entregasconfirmar(0) } sin cambiar el orden de la coleccion
                foreach (Action accion in new Action[]
                {
                    () => entregas.Preparar(new ProductoPorEntregar(), 0, "", ""),
                    () => entregas.Confirmar(0)
                })
                {
                    try
                    {
                        accion();
                        throw new Exception("Se aceptaron datos invalidos");
                    }
                    // atiende el error de esta operacion antes de informar o propagar el problema
                    catch (ArgumentException)
                    {
                        Ok(true, "El vendedor supera el permiso y llega a la validacion de datos de entrega");
                    }
                }
                Ok(!Permisos.TienePermiso(Permisos.AnularMovimientosCaja) && !Permisos.TienePermiso(Permisos.GestionarUsuarios)
                    && !Permisos.TienePermiso(Permisos.ModificarMediosPago), "Se mantienen las restricciones de administracion");
                SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
                Ok(Permisos.TienePermiso(Permisos.GestionarEntregas), "El administrador conserva la gestion de entregas");
            }
            // realiza la limpieza de salida tanto si la operacion termina bien como si falla
            finally
            {
                SesionActual.CerrarSesion();
            }
            return;
        }
        if (args.Contains("--permisos-caja"))
        {
            try
            {
                CN_Caja caja = new CN_Caja();
                SesionActual.CerrarSesion();
                Rechaza(() => caja.Anular(1, "Prueba"), "anular sin sesion");
                SesionActual.IniciarSesion(new usuario { idUsuario = 2, IdRolUsuario = 2 });
                Ok(!Permisos.TienePermiso(Permisos.AnularMovimientosCaja), "El vendedor no tiene permiso de anulacion");
                try
                {
                    caja.Anular(1, "Prueba");
                    throw new Exception("Se permitio anular al vendedor");
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch (InvalidOperationException error)
                {
                    Ok(error.Message == "No tiene permisos para esta operación.", "El negocio rechaza al vendedor antes de acceder a la base");
                }
                SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
                Ok(Permisos.TienePermiso(Permisos.AnularMovimientosCaja), "El administrador tiene permiso de anulacion");
                try
                {
                    caja.Anular(0, "Prueba");
                    throw new Exception("No se valido el identificador");
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch (ArgumentException)
                {
                    Ok(true, "El administrador supera el permiso y llega a la validacion del movimiento");
                }
            }
            // realiza la limpieza de salida tanto si la operacion termina bien como si falla
            finally
            {
                SesionActual.CerrarSesion();
            }
            return;
        }
        if (args.Contains("--presentacion"))
        {
            PruebasPresentacion.Ejecutar();
            return;
        }

        // prepara la conexion configurada y la libera al terminar este alcance
        SqlConnectionStringBuilder configuracionConexion = new SqlConnectionStringBuilder(Conexion.ObtenerCadena());
        Ok(configuracionConexion.InitialCatalog == Base, "Conexión exclusiva a base de pruebas");
        configuracionConexion.InitialCatalog = "master";
        // prepara la conexion configurada y la libera al terminar este alcance
        using SqlConnection master = new SqlConnection(configuracionConexion.ConnectionString);
        master.Open();
        bool creada = false;
        // protege el acceso a los datos para atender los errores de la operacion
        try
        {
            // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
            using (SqlCommand comando = new SqlCommand($"IF DB_ID('{Base}') IS NOT NULL THROW 50000,'La base de prueba ya existe; no se modifica.',1; CREATE DATABASE [{Base}]", master))
            {
                comando.ExecuteNonQuery();
                creada = true;
            }

            Script("BaseDeDatos/CrearEstructuraPrana_LocalDB.sql");
            Script("BaseDeDatos/ActualizarEstructuraPrana_LocalDB.sql");
            Script("BaseDeDatos/CajaYComprobantes.sql");
            Script("BaseDeDatos/CajaYComprobantes.sql");
            Sql(@"INSERT Categoria(nombre_categoria) VALUES(N'Prueba'); INSERT Marca(nombre_marca) VALUES(N'Prueba');
INSERT Producto(nombre_producto,id_marca,id_categoria,modelo) VALUES(N'Heladera <Prueba>',1,1,'TEST');
INSERT Variante_Producto(color,condicion_producto,precio,stock_disponible,id_producto,activo,costo) VALUES(N'Blanco',N'Nuevo',1000,20,1,1,50);
INSERT Cliente(nombre_cliente,apellido_cliente,dni,telefono,direccion,fecha_registro,saldo_a_favor) VALUES(N'Juana',N'Pérez','12345678','2323000000','Calle & Prueba 123',SYSDATETIME(),0);
INSERT Medio_Pago(nombre_medio_pago,recargo_medio_pago,recargo_3,recargo_6,recargo_9,recargo_12,activo,tipo_caja) VALUES(N'Efectivo',0,0,0,0,0,1,N'Efectivo'),(N'Transferencia',0,0,0,0,0,1,N'Transferencia'),(N'Tarjeta',10,20,30,40,50,1,N'Crédito');");
            Script("BaseDeDatos/ActualizarEstructuraPrana_LocalDB.sql");
            // llama a new cd_usuario()crearusuario para realizar este paso del flujo
            if (args.Contains("--turnos-caja"))
            {
                PruebasTurnosCaja();
                return;
            }
            if (args.Contains("--caja-unificada"))
            {
                PruebasCajaUnificada();
                return;
            }
            if (args.Contains("--cierre-automatico"))
            {
                PruebasTurnosCaja();
                return;
            }
            // las restantes pruebas de ventas necesitan ahora una apertura explicita
            new CD_Caja().Abrir(new CierreCaja(), 1);
            if (args.Contains("--opciones-stock-db"))
            {
                SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
                CN_OpcionStock opcionesStock = new CN_OpcionStock();
                OpcionStock marcaOrigen = opcionesStock.Obtener(TipoOpcionStock.Marca).Single(item => item.Id == 1);
                Rechaza(() => opcionesStock.Guardar(TipoOpcionStock.Marca, marcaOrigen, "", null, true), "eliminar marca utilizada por un producto");
                opcionesStock.Guardar(TipoOpcionStock.Marca, marcaOrigen, "CORREGIDA", null, false);
                Ok(Convert.ToString(Sql("SELECT nombre_marca FROM Marca WHERE id_marca=1")) == "CORREGIDA", "Corrige una marca sin borrar el producto");
                int marcaDestino = Convert.ToInt32(Sql("INSERT Marca(nombre_marca) OUTPUT INSERTED.id_marca VALUES(N'DESTINO')"));
                marcaOrigen = opcionesStock.Obtener(TipoOpcionStock.Marca).Single(item => item.Id == 1);
                opcionesStock.Guardar(TipoOpcionStock.Marca, marcaOrigen, "", marcaDestino, false);
                Ok(Convert.ToInt32(Sql("SELECT id_marca FROM Producto WHERE id_producto=1")) == marcaDestino
                    && Convert.ToInt32(Sql("SELECT COUNT(*) FROM Marca WHERE id_marca=1")) == 0, "Unifica la marca conservando el producto");
                OpcionStock colorOrigen = opcionesStock.Obtener(TipoOpcionStock.Color).Single();
                Rechaza(() => opcionesStock.Guardar(TipoOpcionStock.Color, colorOrigen, "", null, true), "eliminar un color en uso sin borrar variantes");
                opcionesStock.Guardar(TipoOpcionStock.Color, colorOrigen, "BLANCO CORREGIDO", null, false);
                Ok(Convert.ToString(Sql("SELECT color FROM Variante_Producto WHERE id_variante=1")) == "BLANCO CORREGIDO", "Corrige el texto del color en la variante");
                int marcaLibre = Convert.ToInt32(Sql("INSERT Marca(nombre_marca) OUTPUT INSERTED.id_marca VALUES(N'SIN USO')"));
                OpcionStock opcionLibre = opcionesStock.Obtener(TipoOpcionStock.Marca).Single(item => item.Id == marcaLibre);
                opcionesStock.Guardar(TipoOpcionStock.Marca, opcionLibre, "", null, true);
                Ok(Convert.ToInt32(Sql($"SELECT COUNT(*) FROM Marca WHERE id_marca={marcaLibre}")) == 0, "Elimina una opcion sin uso");
                Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion IN(N'MODIFICAR_OPCION_STOCK',N'UNIFICAR_OPCION_STOCK',N'ELIMINAR_OPCION_STOCK')")) == 4, "Registra solamente los cuatro cambios confirmados en historial");
                return;
            }
            if (args.Contains("--numeracion-ventas"))
            {
                int primerNumero = CD_RegistroVenta.Guardar(Solicitud(), false);
                Ok(primerNumero == 2500, "La primera venta comienza en 2500");
                venta fallida = Solicitud();
                fallida.Detalles[0].IdVariante = int.MaxValue;
                Rechaza(() => CD_RegistroVenta.Guardar(fallida, false), "venta con detalle invalido despues de insertar el encabezado");
                int segundoNumero = CD_RegistroVenta.Guardar(Solicitud(), false);
                Ok(segundoNumero == primerNumero + 1, "Una venta fallida no deja un numero vacio");
                Sql("DBCC CHECKIDENT ('dbo.Venta', RESEED, 3504)");
                int tercerNumero = CD_RegistroVenta.Guardar(Solicitud(), false);
                Ok(tercerNumero == segundoNumero + 1, "Un salto del contador identity no cambia el numero elegido");
                Task<int> simultaneaUno = Task.Run(() => CD_RegistroVenta.Guardar(Solicitud(), false));
                Task<int> simultaneaDos = Task.Run(() => CD_RegistroVenta.Guardar(Solicitud(), false));
                Task.WaitAll(simultaneaUno, simultaneaDos);
                Ok(Math.Min(simultaneaUno.Result, simultaneaDos.Result) == tercerNumero + 1
                    && Math.Max(simultaneaUno.Result, simultaneaDos.Result) == tercerNumero + 2,
                    "Dos ventas simultaneas reciben numeros consecutivos distintos");
                return;
            }

            if (args.Contains("--comprobante-venta"))
            {
                SesionActual.IniciarSesion(new usuario { idUsuario = 1, IdRolUsuario = 1 });
                int idVenta = CD_RegistroVenta.Guardar(Solicitud(), false);
                venta documentoVenta = new CN_Comprobante().Obtener(idVenta);
                Ok(documentoVenta.IdVenta == idVenta && documentoVenta.IdCliente == 1 && documentoVenta.Cliente.IdCliente == 1
                    && documentoVenta.IdUsuario == 1 && documentoVenta.NombreCompletoCliente == "Juana Pérez", "La consulta reconstruye venta y su cliente");
                Ok(documentoVenta.Total == 1000 && documentoVenta.Pagado == 400 && documentoVenta.SaldoPendiente == 600
                    && documentoVenta.Detalles.Count == 1 && documentoVenta.Pagos.Count == 1, "Conserva detalle pagos y saldo en venta");
                Ok(documentoVenta.FechaEntrega == DateTime.Today && documentoVenta.HorarioSugerido == "14 a 18"
                    && documentoVenta.ObservacionesEntrega == "Llamar antes", "Conserva la entrega dentro de venta");
                Sql($"DELETE Venta_Entrega WHERE id_venta={idVenta}");
                Ok(new CN_Comprobante().Obtener(idVenta).FechaEntrega == null, "Admite ventas anteriores sin fecha estimada");
                return;
            }
            new CD_Usuario().CrearUsuario("usuario_prueba_uno", "hash_prueba", 2);
            // llama a new cd_usuario()crearusuario para realizar este paso del flujo
            new CD_Usuario().CrearUsuario("usuario_prueba_dos", "hash_prueba", 2);
            Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Usuario WHERE nombre_usuario LIKE 'usuario_prueba_%'")) == 2, "Varios usuarios después de migrar dos veces");
            Rechaza(() => new CD_Usuario().CrearUsuario("usuario_prueba_uno", "hash_prueba", 2), "nombre de usuario activo duplicado");
            Sql("UPDATE Usuario SET activo=0 WHERE nombre_usuario='usuario_prueba_uno'");
            Rechaza(() => new CD_Usuario().CrearUsuario("usuario_prueba_uno", "hash_prueba", 2), "nombre de usuario dado de baja duplicado");
            Rechaza(() => new CD_Usuario().ModificarUsuario(Convert.ToInt32(Sql("SELECT id_usuario FROM Usuario WHERE nombre_usuario='usuario_prueba_dos'")), "usuario_prueba_uno", null, 2), "renombrar usando el nombre de una cuenta dada de baja");
            SesionActual.IniciarSesion(new usuario
            {
                idUsuario = 1,
                IdRolUsuario = 1

            });
            CN_Cliente clientes = new CN_Cliente();
            Cliente clientePostal = clientes.buscarCliente("12345678").Single();
            clientePostal.Localidad.NombreLocalidad = "Lujan";
            clientePostal.CodigoPostal = "B6700ABC";
            clientePostal.Direccion = "";
            Ok(clientes.NuevoCliente(new Cliente
            {
                Nombre = "Ana",
                Apellido = "Prueba",
                FechaRegistro = DateTime.Today,
                DNI = "87654321",
                Telefono = "2323000000",
                Direccion = "",
                Localidad = new Localidad { NombreLocalidad = "Lujan" }

            }), "Crear cliente sin dirección");
            Ok(clientes.ModificarCliente(clientePostal), "Guardar ciudad y codigo postal alfanumerico");
            Ok(clientes.buscarCliente("12345678").Single().CodigoPostal == "B6700ABC", "Codigo postal persistido");
            clientePostal.Localidad.NombreLocalidad = "";
            Rechaza(() => clientes.ModificarCliente(clientePostal), "ciudad vacia");
            clientePostal.Localidad.NombreLocalidad = "Lujan";
            clientePostal.CodigoPostal = "";
            Ok(clientes.ModificarCliente(clientePostal) && clientes.buscarCliente("12345678").Single().CodigoPostal == "", "Codigo postal opcional y se puede borrar");
            clientePostal.CodigoPostal = "6700";
            clientes.ModificarCliente(clientePostal);
            CN_RegistroVenta ventas = new CN_RegistroVenta();
            CN_Caja caja = new CN_Caja();
            CN_Pago pagos = new CN_Pago();
            Rechaza(() => ventas.Registrar(Solicitud(0)), "venta parcial sin pago inicial");
            int primera = ventas.Registrar(Solicitud());
            Ok(primera == 2500, "Primera venta = 2500 en base vacía, migración repetida");
            venta conDescuento = Solicitud(1);
            conDescuento.Descuento = 100;
            conDescuento.Total = 900;
            int segunda = ventas.Registrar(conDescuento);
            Ok(segunda == 2501, "Segunda venta = 2501");
            Script("BaseDeDatos/CajaYComprobantes.sql");
            int tercera = ventas.Registrar(Solicitud(1));
            Ok(tercera == 2502, "Reiniciar no retrocede ni repite números");
            Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='REGISTRAR_VENTA'")) == 3, "Historial registra cada venta sin duplicados");
            Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='APLICAR_DESCUENTO'")) == 1, "Historial registra solo descuentos aplicados");
            Sql($"DELETE FROM LogActividad WHERE id_entidad={segunda} AND entidad='Venta'");
            Script("BaseDeDatos/CajaYComprobantes.sql");
            Script("BaseDeDatos/CajaYComprobantes.sql");
            Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='REGISTRAR_VENTA'")) == 3 && Convert.ToInt32(Sql("SELECT COUNT(*) FROM LogActividad WHERE accion='APLICAR_DESCUENTO'")) == 1, "Recuperacion historica idempotente de ventas y descuentos");
            Ok(Sql("SELECT OBJECT_ID('dbo.Venta_Comprobante','U')") is DBNull, "La tabla eliminada no se recrea");
            venta documento = new CN_Comprobante().Obtener(primera);
            Ok(documento.IdVenta == 2500 && documento.Pagado == 400 && documento.SaldoPendiente == 600 && documento.Detalles.Count == 1, "Comprobantes reconstruidos con número común y saldo real");
            Ok(documento.Cliente.CodigoPostal == "6700" && documento.Cliente.NombreLocalidad == "Lujan", "Remito conserva ciudad y codigo postal");
            Ok(documento.HorarioSugerido == "14 a 18" && documento.ObservacionPago == "Pago parcial registrado" && documento.ObservacionesEntrega == "Llamar antes", "Comprobante consulta observaciones y horario");
            venta recuperado = new CN_Comprobante().Obtener(primera);
            Ok(recuperado.HorarioSugerido == "14 a 18" && recuperado.Total == documento.Total && recuperado.Pagado == documento.Pagado, "Comprobante consulta datos de entrega e importes");
            Sql("UPDATE Cliente SET nombre_cliente=N'Cambiado' WHERE id_cliente=1");
            Ok(new CN_Comprobante().Obtener(primera).NombreCompletoCliente == "Cambiado Pérez", "Reimpresión consulta los datos actuales del cliente");
            string carpeta = DocumentosPrana.GuardarVenta(documento, "obj/CajaComprobantes/muestras");
            string garantia = File.ReadAllText(Path.Combine(carpeta, "Garantia-2500.html"));
            string remito = File.ReadAllText(Path.Combine(carpeta, "Remito-2500.html"));
            Ok(garantia.Contains("GARANTÍA") && garantia.Contains("2500") && remito.Contains("2500") && garantia.Contains("Heladera &lt;Prueba&gt;"), "HTML generado, títulos, números y escape seguro");
            List<MedioPago> medios = new CN_MedioPago().ObtenerTodos();
            MedioPago efectivo = medios.Single(medioPago => medioPago.TipoCaja == "Efectivo");
            MedioPago transferencia = medios.Single(m => m.TipoCaja == "Transferencia");
            pagos.Registrar(primera, transferencia, 1, 600, "Lemon");
            Sql($"UPDATE Venta SET fecha_venta=DATEADD(day,-1,fecha_venta) WHERE id_venta={primera}");
            caja.Registrar(new MovimientoCaja
            {
                Concepto = "Flete",
                Importe = 100,
                Tipo = "Gasto"

            });
            caja.Registrar(new MovimientoCaja
            {
                Concepto = "Café",
                Importe = 50,
                Tipo = "Café"

            });
            caja.Registrar(new MovimientoCaja
            {
                Concepto = "Ingreso USD",
                Importe = 20,
                Tipo = "Ingreso extra",
                Moneda = "USD"

            });
            caja.Registrar(new MovimientoCaja
            {
                Concepto = "Gasto USD",
                Importe = 5,
                Tipo = "Gasto",
                Moneda = "USD"

            });
            caja.Registrar(new MovimientoCaja
            {
                Concepto = "Error",
                Importe = 999,
                Tipo = "Gasto"

            });
            CierreCaja consulta = caja.Consultar(DateTime.Today);
            caja.Anular(consulta.Movimientos.Single(movimiento => movimiento.Concepto == "Error").Id, "Error de carga");
            consulta = caja.Consultar(DateTime.Today);
            consulta.SaldoInicial = 200;
            consulta.SaldoInicialUsd = 10;
            consulta.Billetes = new()
            {
                new()
                {
                    Moneda = "ARS",
                    Denominacion = 100,
                    Cantidad = 5

},
                new()
                {
                    Moneda = "ARS",
                    Denominacion = 50,
                    Cantidad = 1

},
                new()
                {
                    Moneda = "ARS",
                    Denominacion = 1,
                    Cantidad = 2

},
                new()
                {
                    Moneda = "USD",
                    Denominacion = 5,
                    Cantidad = 5

}
            };
            Ok(consulta.Ingresos("ARS") == 1052 && consulta.Gastos("ARS") == 100 && consulta.Esperado("ARS") == 552 && consulta.BalanceArs == 1152 && consulta.Diferencia("ARS") == 0, "Caja: cobros de ventas anteriores, gastos, café, apertura y diferencia");
            Ok(consulta.Esperado("USD") == 25 && consulta.Diferencia("USD") == 0, "Dólares separados, sin mezclarse con pesos");
            Ok(consulta.Movimientos.Any(m => m.Cuenta == "Lemon"), "Cuenta destino de transferencias en cierre");
            CierreCaja obsoleto = caja.Consultar(DateTime.Today);
            caja.Registrar(new MovimientoCaja
            {
                Concepto = "Nuevo",
                Importe = 1,
                Tipo = "Ingreso extra"

            });
            Rechaza(() => caja.Cerrar(obsoleto), "cierre con movimientos desactualizados");
            caja.Anular(caja.Consultar(DateTime.Today).Movimientos.Single(m => m.Concepto == "Nuevo").Id, "Prueba");
            consulta.Movimientos = caja.Consultar(DateTime.Today).Movimientos;
            consulta.Inicio = DateTime.Today;
            CierreCaja cerrado = caja.Cerrar(consulta);
            Ok(cerrado.Numero > 0, "Cierre guardado");
            File.WriteAllText("obj/CajaComprobantes/muestras/Cierre.html", DocumentosPrana.CierreHtml(cerrado));
            Rechaza(() => caja.Cerrar(consulta), "segundo cierre del mismo día");
            Rechaza(() => caja.Registrar(new MovimientoCaja
            {
                Concepto = "Tarde",
                Importe = 1

            }), "gasto posterior al cierre");
            Rechaza(() => pagos.Registrar(segunda, efectivo, 1, 10), "cobro posterior al cierre");
            Rechaza(() => ventas.Registrar(Solicitud()), "venta con cobro posterior al cierre");
            Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Venta")) == 3, "Fallo no deja venta parcial");
            Sql("UPDATE Medio_Pago SET tipo_caja=N'Otros' WHERE id_medio_pago=1");
            CierreCaja guardado = caja.Consultar(DateTime.Today);
            Ok(guardado.Esperado("ARS") == 550 && guardado.Contado("USD") == 25, "Cierre conserva snapshot, billetes y totales");
            // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
            using (frmCaja formulario = new frmCaja())
            {
                // llama a typeof(frmcaja)getmethod("cargar", systemreflectionbindingflagsnonpublic | systemreflectionbindingflagsinstance)!invoke para realizar este paso del flujo
                typeof(frmCaja).GetMethod("Cargar", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.Invoke(formulario, null);
                // procesa cada ancho de new[] { 840, 1100, 780, 840 } sin cambiar el orden de la coleccion
                foreach (int ancho in new[]
                {
                    840,
                    1100,
                    780,
                    840
                }

                )
                {
                    formulario.ClientSize = new Size(ancho, 690);
                    formulario.PerformLayout();
                    Panel pagina = (Panel)formulario.Controls["paginaAdaptable"]!;
                    formulario.Controls.Remove(pagina);
                    pagina.Dock = DockStyle.None;
                    pagina.Size = new Size(ancho, 690);
                    pagina.PerformLayout();
                    // procesa cada control de paginacontrols sin cambiar el orden de la coleccion
                    foreach (Control control in pagina.Controls)
                    {
                        Ok(control.Right <= pagina.ClientSize.Width, "Caja: sección dentro del ancho " + ancho);
                    }

                    using Bitmap imagen = new Bitmap(ancho, 690);
                    pagina.DrawToBitmap(imagen, new Rectangle(0, 0, ancho, 690));
                    if (ancho == 840)
                    {
                        imagen.Save("obj/CajaComprobantes/caja-ui.png");
                    }

                    pagina.ScrollControlIntoView(pagina.Controls[pagina.Controls.Count - 1]);
                    Ok(pagina.Controls[pagina.Controls.Count - 1].Bottom <= pagina.ClientSize.Height + 1, "Caja: botón de cierre accesible al desplazar");
                    pagina.AutoScrollPosition = Point.Empty;
                    formulario.Controls.Add(pagina);
                    pagina.Dock = DockStyle.Fill;
                }
            }

            global::Prana.Negocio.CN_Producto stockNegocio = new Prana.Negocio.CN_Producto();
            List<(int idVariante, int cantidad, decimal costoUnitario)> detalleStock = new List<(int idVariante, int cantidad, decimal costoUnitario)>
            {
                (1, 2, 50m)
            };
            Ok(stockNegocio.RegistrarIngreso("  Proveedor nuevo prueba  ", DateTime.Today, "STOCK-1", detalleStock), "Ingreso crea proveedor nuevo");
            Ok(stockNegocio.RegistrarIngreso("proveedor NUEVO prueba", DateTime.Today, "STOCK-2", detalleStock), "Ingreso reutiliza nombre escrito sin distinguir mayúsculas");
            Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Proveedor WHERE nombre_proveedor='Proveedor nuevo prueba'")) == 1, "No duplica proveedor");
            Ok(Convert.ToInt32(Sql("SELECT COUNT(DISTINCT id_proveedor) FROM Ingreso_Stock WHERE remito_proveedor IN ('STOCK-1','STOCK-2')")) == 1, "Ambos ingresos usan el proveedor correcto");
            Rechaza(() => stockNegocio.RegistrarIngreso("Proveedor rollback", DateTime.Today, "STOCK-ERROR", new() { (int.MaxValue, 1, 10m) }), "Ingreso inválido informa la causa");
            Ok(Convert.ToInt32(Sql("SELECT COUNT(*) FROM Proveedor WHERE nombre_proveedor='Proveedor rollback'")) == 0, "Ingreso fallido revierte proveedor nuevo");
            Rechaza(() => stockNegocio.RegistrarIngreso("Proveedor inválido", DateTime.Today, "PRUEBA", new() { (1, 0, 10m) }), "ingreso sin cantidad positiva");
            try
            {
                stockNegocio.RegistrarIngreso("Proveedor sin documento", DateTime.Today, "   ", detalleStock);
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new Exception("Aceptó ingreso sin documento");
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (InvalidOperationException ex)
            {
                Ok(ex.Message == "Ingrese el número o código del documento del proveedor (remito).", "Documento vacío informa exactamente qué falta");
            }

            Rechaza(() => stockNegocio.RegistrarIngreso("Proveedor inválido", DateTime.Today, new string('X', 51), detalleStock), "documento supera longitud de la base");
            // limita la vida del recurso al bloque; al salir se ejecuta su liberacion incluso ante una excepcion
            using (frmStock stockForm = new frmStock())
            {
                object CampoStock(string nombre) => typeof(frmStock).GetField(nombre, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.GetValue(stockForm)!;
                ComboBox proveedorControl = (ComboBox)CampoStock("comboBoxSeleccionaProveedor");
                Ok(proveedorControl.DropDownStyle == ComboBoxStyle.DropDown && proveedorControl.AutoCompleteSource == AutoCompleteSource.ListItems, "Un proveedor editable con sugerencias");
                proveedorControl.Text = "Proveedor nuevo prueba";
                // ((textbox)campostock("txtdocumentoproveedor"))text: texto visible en el control; en un campo de entrada tambien representa su contenido
                ((TextBox)CampoStock("txtDocumentoProveedor")).Text = "STOCK-1";
                // ((datetimepicker)campostock("dtpfechaingresostock"))value: valor actual del control numerico o de fecha
                ((DateTimePicker)CampoStock("dtpFechaIngresoStock")).Value = DateTime.Today.AddDays(-1);
                // ((label)campostock("lbltotalingresodinero"))text: texto visible en el control; en un campo de entrada tambien representa su contenido
                ((Label)CampoStock("LblTotalIngresoDinero")).Text = "85000";
                // ((label)campostock("labelcantidadingresada"))text: texto visible en el control; en un campo de entrada tambien representa su contenido
                ((Label)CampoStock("labelCantidadIngresada")).Text = "5";
                // llama a typeof(frmstock)getmethod("limpiaringreso", systemreflectionbindingflagsinstance | systemreflectionbindingflagsnonpublic)!invoke para realizar este paso del flujo
                typeof(frmStock).GetMethod("LimpiarIngreso", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.Invoke(stockForm, null);
                Ok(proveedorControl.SelectedIndex == -1 && proveedorControl.Text == "" && ((TextBox)CampoStock("txtDocumentoProveedor")).Text == "", "Ingreso limpia proveedor y remito");
                Ok(((Label)CampoStock("labelCantidadIngresada")).Text == "0" && ((Label)CampoStock("LblTotalIngresoDinero")).Text == 0m.ToString("C2") && ((DateTimePicker)CampoStock("dtpFechaIngresoStock")).Value.Date == DateTime.Today, "Ingreso reinicia totales y fecha");
            }

            SesionActual.IniciarSesion(new usuario
            {
                idUsuario = 1,
                IdRolUsuario = 2

            });
            Ok(caja.Consultar(DateTime.Today) != null, "Caja se consulta sin restriccion por rol");
            Console.WriteLine("TODAS LAS PRUEBAS PASARON");
        }
        // realiza la limpieza de salida tanto si la operacion termina bien como si falla
        finally
        {
            if (creada)
            {
                SqlConnection.ClearAllPools();
                // prepara la consulta y su conexion los valores variables se envian mediante parametros
                using SqlCommand cmd = new SqlCommand($"ALTER DATABASE [{Base}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{Base}]", master);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
