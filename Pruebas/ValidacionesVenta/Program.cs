using System;
using System.Collections.Generic;
using Prana.Entidades;
using Prana.negocio;

// agrupa los datos y operaciones de program
internal static class Program
{
    // conserva negocio entre las operaciones de esta clase
    private static readonly CN_RegistroVenta Negocio = new();
    // conserva comprobaciones entre las operaciones de esta clase
    private static int comprobaciones;
    // resuelve venta con los datos recibidos y entrega el resultado al codigo que lo llama
    private static venta Venta() => new()
    {
        IdCliente = 1,
        IdUsuario = 1,
        FechaPago = DateTime.Today,
        ModalidadEntrega = "Retiro en el local",
        CondicionPago = "Pago total",
        Total = 100,
        Detalles = [new()
        {
            IdVariante = 1,
            Cantidad = 1,
            PrecioUnitario = 100

}

        ],
        Pagos = [new()
        {
            IdMedioPago = 1,
            Monto = 100,
            Cuotas = 1

}

        ]

    };
    // resuelve rechaza con los datos recibidos y entrega el resultado al codigo que lo llama
    private static void Rechaza(Action accion, string mensaje)
    {
        try
        {
            accion();
        }
        // atiende el error de esta operacion antes de informar o propagar el problema
        catch (Exception error) when (error is InvalidOperationException || error is ArgumentException)
        {
            if (!error.Message.Contains(mensaje))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new Exception($"Error inesperado: {error.Message}");
            }

            comprobaciones++;
            return;
        }

        // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
        throw new Exception($"No rechazó: {mensaje}");
    }

    // resuelve invalida con los datos recibidos y entrega el resultado al codigo que lo llama
    private static void Invalida(Action<venta> cambiar, string mensaje)
    {
        venta venta = Venta();
        cambiar(venta);
        // registrar debe rechazar la solicitud antes de abrir una conexion a datos
        Rechaza(() => Negocio.Registrar(venta), mensaje);
    }

    // punto de entrada: prepara el entorno y ejecuta el flujo inicial definido en este programa
    private static void Main()
    {
        if (Environment.GetCommandLineArgs().Contains("--telefonos-clientes"))
        {
            // valida formatos sin acceder a sql y comprueba las dos operaciones que guardan clientes
            foreach (string telefono in new[] { "2323516501", "1145678901", "3414567890" })
            {
                CN_Cliente.ValidarTelefono(telefono);
                comprobaciones++;
            }
            CN_Cliente clientes = new CN_Cliente();
            foreach (string telefono in new[] { "549888 555", "2323 516501", "2323-516501", "(2323)516501", "+5492323516501", "5492323516501", "02323516501", "1551650100", "516501", "23235165010", "2323516501\n", "", "２３２３５１６５０１" })
            {
                Rechaza(() => CN_Cliente.ValidarTelefono(telefono), "10 dígitos");
                Cliente cliente = new Cliente { IdCliente = 1, Nombre = "FEDERICO", Apellido = "SCARDULLA", TipoDocumento = "DNI", DNI = "20123456", Telefono = telefono };
                Rechaza(() => clientes.NuevoCliente(cliente), "10 dígitos");
                Rechaza(() => clientes.ModificarCliente(cliente), "10 dígitos");
            }
            Rechaza(() => CN_Cliente.ValidarTelefono(null), "10 dígitos");
            Console.WriteLine($"Comprobaciones de telefonos: {comprobaciones}");
            return;
        }
        CN_Pago pagos = new CN_Pago();
        MedioPago credito = new MedioPago
        {
            IdMedioPago = 1,
            Activo = true,
            TipoCaja = "Crédito",
            RecargoMedioPago = 10,
            Recargo3 = 20

        };
        venta saldo = new venta
        {
            SaldoPendiente = 70000
        };
        Pago cotizacion = pagos.Preparar(1, credito, 1, 30000);
        if (cotizacion.ImporteBase != 30000 || cotizacion.Recargo != 3000 || cotizacion.Cobrado != 33000 || saldo.SaldoPendiente - cotizacion.ImporteBase != 40000)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception("Cotizacion incorrecta con recargo general.");
        }

        comprobaciones++;
        cotizacion = pagos.Preparar(1, credito, 3, 30000);
        if (cotizacion.Recargo != 6000 || cotizacion.Cobrado != 36000)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception("Recargo de cuotas incorrecto.");
        }

        comprobaciones++;
        cotizacion = pagos.Preparar(1, new MedioPago
        {
            IdMedioPago = 2,
            Activo = true,
            TipoCaja = "Efectivo"

        }, 1, 30000);
        if (cotizacion.Recargo != 0 || cotizacion.Cobrado != 30000 || saldo.SaldoPendiente != 70000)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception("Cambiar el medio acumula recargos o modifica la deuda.");
        }

        comprobaciones++;
        if (Environment.GetCommandLineArgs().Contains("--recargos"))
        {
            Console.WriteLine("OK: recargo general, recargo por cuotas y cambio de medio sin modificar la deuda.");
            return;
        }

        Negocio.Validar(Venta());
        comprobaciones++;
        Invalida(ventaActual => ventaActual.IdCliente = 0, "cliente");
        Invalida(v => v.IdUsuario = 0, "usuario");
        Invalida(v => v.Detalles.Clear(), "producto");
        Invalida(v => v.Detalles[0].Cantidad = 0, "productos válidos");
        Invalida(v => v.Descuento = -1, "negativo");
        Invalida(v => v.Descuento = 101, "50%");
        Invalida(v => v.Descuento = 50.01m, "50%");
        venta descuentoMaximo = Venta();
        descuentoMaximo.Descuento = 50;
        descuentoMaximo.Total = 50;
        descuentoMaximo.Pagos[0].Monto = 50;
        Negocio.Validar(descuentoMaximo);
        comprobaciones++;
        venta descuentoConEnvio = Venta();
        descuentoConEnvio.CostoEnvio = 100;
        descuentoConEnvio.Descuento = 60;
        Rechaza(() => Negocio.Validar(descuentoConEnvio), "50%");
        Invalida(v => v.ModalidadEntrega = "", "forma de entrega");
        Invalida(v =>
        {
            v.ModalidadEntrega = "Envío a domicilio";
            v.TipoLogistica = "Propia";
        }, "domicilio y logística");
        Invalida(v =>
        {
            v.ModalidadEntrega = "Envío a domicilio";
            v.DomicilioEntrega = "Calle 1";
        }, "domicilio y logística");
        Invalida(v => v.CostoEnvio = -1, "negativo");
        Invalida(v => v.CondicionPago = "", "condición");
        Invalida(v => v.CondicionPago = "inexistente", "condición");
        Invalida(v => v.FechaPago = DateTime.Today.AddDays(-1), "fecha actual");
        Invalida(v => v.FechaPago = DateTime.Today.AddDays(1), "fecha actual");
        Invalida(v => v.Pagos[0].Monto = -1, "importes");
        Invalida(v => v.Pagos[0].CuentaDestino = new string('x', 101), "100 caracteres");
        Invalida(v => v.Pagos[0].Monto = 101, "superan el total");
        Invalida(v => v.Pagos[0].Monto = 99, "cubrir la venta");
        // procesa cada condicion de new[] { "pago parcial" } sin cambiar el orden de la coleccion
        foreach (string condicion in new[]
        {
            "Pago parcial"
        }

        )
        {
            Invalida(v =>
            {
                v.CondicionPago = condicion;
                v.Pagos.Clear();
            }, "pago inicial parcial");
            Invalida(v => v.CondicionPago = condicion, "pago inicial parcial");
            venta parcial = Venta();
            parcial.CondicionPago = condicion;
            parcial.Pagos[0].Monto = 40;
            Negocio.Validar(parcial);
            comprobaciones++;
        }

        Invalida(v => v.Total = 101, "no coincide");
        venta envio = Venta();
        envio.ModalidadEntrega = "Envío a domicilio";
        envio.DomicilioEntrega = "Calle 1";
        envio.TipoLogistica = "Propia";
        envio.CostoEnvio = 10;
        envio.Total = 115;
        envio.Pagos[0].Monto = 110;
        envio.Pagos[0].Recargo = 5;
        Negocio.Validar(envio);
        comprobaciones++;
        producto producto = new producto
        {
            NombreProducto = "Heladera"
        };
        VarianteProducto variante = new VarianteProducto
        {
            IdVariante = 1,
            Precio = 100,
            StockFisico = 5,
            StockComprometido = 2

        };
        Rechaza(() => Negocio.PrepararProducto(null, null, 1, []), "Seleccione un producto");
        DetalleNuevaVenta sinDisponible = Negocio.PrepararProducto(producto, variante, 100, []);
        if (sinDisponible.Cantidad != 100)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception("Debe permitir registrar una venta de 100 unidades aunque no haya stock disponible");
        }

        variante.StockFisico = 0;
        DetalleNuevaVenta sinStock = Negocio.PrepararProducto(producto, variante, 2, [new() {
IdVariante = 1,
Cantidad = 2

}]);
        if (sinStock.Cantidad != 2)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception("Debe permitir agregar productos sin stock físico");
        }

        comprobaciones += 2;
        Rechaza(() => Negocio.PrepararProducto(producto, variante, 0, []), "no son válidos");
        DetalleNuevaVenta detalle = Negocio.PrepararProducto(producto, variante, 3, []);
        if (detalle.Cantidad != 3 || detalle.PrecioUnitario != 100)
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception("Detalle incorrecto");
        }

        comprobaciones++;
        MedioPago medio = new MedioPago
        {
            IdMedioPago = 1,
            Activo = true,
            RecargoMedioPago = 10

        };
        Rechaza(() => Negocio.PrepararPago(null, 1, 10, 100, [], ""), "medio activo");
        Rechaza(() => Negocio.PrepararPago(medio, 1, 111, 100, [], ""), "supera el saldo");
        Rechaza(() => Negocio.PrepararPago(medio, 4, 10, 100, [], ""), "cuotas válida");
        Pago pago = Negocio.PrepararPago(medio, 1, 110, 100, [], " referencia ");
        if (pago.Monto != 100 || pago.Recargo != 10 || pago.CuentaDestino != "referencia")
        {
            // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
            throw new Exception("Cobro incorrecto");
        }

        comprobaciones++;
        // verifica que el alta rechace los datos invalidos antes de acceder a sql
        global::Prana.Negocio.CN_Producto productos = new Prana.Negocio.CN_Producto();
        void ProductoInvalido(Action<producto> cambiar, string mensaje)
        {
            producto candidato = new producto
            {
                NombreProducto = "Heladera",
                Marca = "Marca",
                Categoria = "Categoría",
                Modelo = "ABC",
                Descripcion = "Descripción",
                Variantes = [new()
                {
                    Color = "Blanco",
                    CondicionProducto = "Nuevo",
                    Precio = 100

}

                ]

            };
            cambiar(candidato);
            Rechaza(() => productos.NuevoProducto(candidato), mensaje);
        }

        ProductoInvalido(productoActual => productoActual.NombreProducto = new string('a', 151), "150 caracteres");
        ProductoInvalido(p => p.NombreProducto = " ", "nombre");
        ProductoInvalido(p => p.Marca = new string('a', 101), "100 caracteres");
        ProductoInvalido(p => p.Categoria = new string('a', 101), "100 caracteres");
        ProductoInvalido(p => p.Descripcion = new string('a', 501), "500 caracteres");
        ProductoInvalido(p => p.Modelo = new string('a', 16), "15 caracteres");
        ProductoInvalido(p => p.Variantes.Clear(), "variante");
        ProductoInvalido(p => p.Variantes[0].Color = " ", "color");
        ProductoInvalido(p => p.Variantes[0].Color = new string('a', 51), "50 caracteres");
        ProductoInvalido(p => p.Variantes[0].CondicionProducto = new string('a', 11), "10 caracteres");
        ProductoInvalido(p => p.Variantes[0].Precio = 0, "mayor que cero");
        ProductoInvalido(p => p.Variantes[0].Precio = 100000000m, "99.999.999,99");
        ProductoInvalido(p => p.Variantes[0].Precio = 100.001m, "dos decimales");
        Console.WriteLine($"OK: {comprobaciones} comprobaciones de negocio, sin controles ni acceso a base de datos.");
    }
}
