using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion;
// agrupa los datos y operaciones de frm caja
public partial class frmCaja : Form
{
    // conserva negocio entre las operaciones de esta clase
    private readonly CN_Caja negocio = new();
    // conserva actual entre las operaciones de esta clase
    private CierreCaja actual = new();
    // conserva cargando entre las operaciones de esta clase
    private bool cargando = true;
    // prepara frm caja antes de que se utilice la instancia
    public frmCaja()
    {
        InitializeComponent();
        inicio.Value = DateTime.Now;
        anular.Enabled = SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.AnularMovimientosCaja);
        inicio.ValueChanged += DatosCierreCambiados;
        observaciones.TextChanged += DatosCierreCambiados;
    }

    // ajusta la presentacion de una celda sin convertir ese formato en una escritura a la base
    private void movimientos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (movimientos.Columns[e.ColumnIndex].DataPropertyName == "Venta" && e.Value is int numero)
        {
            e.Value = $"RTO {numero}";
            e.FormattingApplied = true;
        }
    }

    // resuelve saldo value changed con los datos recibidos y entrega el resultado al codigo que lo llama
    private void saldo_ValueChanged(object? sender, EventArgs e)
    {
        ActualizarResumen();
        DatosCierreCambiados(sender, e);
    }

    // resuelve saldo usd value changed con los datos recibidos y entrega el resultado al codigo que lo llama
    private void saldoUsd_ValueChanged(object? sender, EventArgs e)
    {
        ActualizarResumen();
        DatosCierreCambiados(sender, e);
    }

    // cada cambio queda guardado antes de abandonar la pantalla o llegar a medianoche
    private void DatosCierreCambiados(object? sender, EventArgs e)
    {
        if (cargando || actual.Numero != 0 || actual.Fecha.Date != fecha.Value.Date || fecha.Value.Date != DateTime.Today)
        {
            return;
        }
        Ejecutar(GuardarDatosAbiertos);
    }

    // resuelve guardar datos abiertos con los datos recibidos y entrega el resultado al codigo que lo llama
    private void GuardarDatosAbiertos()
    {
        actual.SaldoInicial = saldo.Value;
        actual.SaldoInicialUsd = saldoUsd.Value;
        actual.Inicio = actual.Fecha.Date + inicio.Value.TimeOfDay;
        actual.Observaciones = observaciones.Text.Trim();
        negocio.GuardarCajaAbierta(actual);
    }

    // sigue el nuevo dia si se estaba mirando la caja actual conserva las consultas de otras fechas
    internal void ActualizarCambioDeDia(DateTime diaAnterior, DateTime hoy)
    {
        if (fecha.Value.Date == diaAnterior)
        {
            fecha.Value = hoy;
        }
        else
        {
            Ejecutar(Cargar);
        }
    }

    // ejecuta la accion de resumen mes cuando el usuario pulsa el control
    private void resumenMes_Click(object? sender, EventArgs e)
    {
        using frmResumenMensualCaja pantalla = new frmResumenMensualCaja(fecha.Value);
        pantalla.ShowDialog(this);
    }

    // ejecuta la accion de actualizar cuando el usuario pulsa el control
    private void actualizar_Click(object? sender, EventArgs e)
    {
        Ejecutar(Cargar);
    }

    // resuelve fecha value changed con los datos recibidos y entrega el resultado al codigo que lo llama
    private void fecha_ValueChanged(object? sender, EventArgs e)
    {
        Ejecutar(Cargar);
    }

    // ejecuta la accion de registrar cuando el usuario pulsa el control
    private void registrar_Click(object? sender, EventArgs e)
    {
        Ejecutar(() =>
        {
            negocio.Registrar(ObtenerMovimientoDelFormulario());
            motivo.Clear();
            cuenta.Clear();
            importe.Value = 0;
            Cargar();
        });
    }

    // ejecuta la accion de cerrar cuando el usuario pulsa el control
    private void cerrar_Click(object? sender, EventArgs e)
    {
        Ejecutar(Cerrar);
    }

    // ejecuta la accion de imprimir cuando el usuario pulsa el control
    private void imprimir_Click(object? sender, EventArgs e)
    {
        Ejecutar(() => DocumentosPrana.Abrir(DocumentosPrana.GuardarCierre(actual)));
    }

    // ejecuta la accion de anular cuando el usuario pulsa el control
    private void anular_Click(object? sender, EventArgs e)
    {
        Ejecutar(() =>
        {
            // evita abrir la solicitud de anulacion si la sesion no es de administrador
            Permisos.Exigir(Permisos.AnularMovimientosCaja);
            // comprueba si se recibio el valor opcional antes de utilizarlo
            if (movimientos.CurrentRow?.DataBoundItem is not MovimientoCaja m || m.Venta.HasValue)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione un gasto o ingreso manual. Los cobros de ventas no se anulan desde aquí.");
            }

            using frmMotivoAnulacion dialogo = new frmMotivoAnulacion();
            if (dialogo.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            negocio.Anular(m.Id, dialogo.Motivo);
            Cargar();
        });
    }

    // prepara la informacion inicial cuando se carga la ventana
    private void frmCaja_Load(object? sender, EventArgs e) => Ejecutar(Cargar);
    // obtiene y presenta para actualizar los controles de la pantalla
    private void Cargar()
    {
        CierreCaja consultado = negocio.Consultar(fecha.Value.Date);
        cargando = true;
        try
        {
            actual = consultado;
            saldo.Value = actual.SaldoInicial;
            saldoUsd.Value = actual.SaldoInicialUsd;
            inicio.Value = actual.Inicio;
            observaciones.Text = actual.Observaciones;

            movimientos.DataSource = actual.Movimientos;
            bool abierta = actual.Numero == 0;
            registrar.Enabled = abierta && fecha.Value.Date == DateTime.Today;
            cerrar.Enabled = abierta && fecha.Value.Date <= DateTime.Today;
            imprimir.Enabled = !abierta;
            saldo.Enabled = saldoUsd.Enabled = inicio.Enabled = observaciones.Enabled = abierta && fecha.Value.Date == DateTime.Today;
            if (abierta)
            {
                estado.Text = "Caja sin cerrar. Los datos se guardan automáticamente y el día se cierra a medianoche. Los cobros se incluyen por su fecha de pago.";
            }
            else
            {
                estado.Text = $"Cierre N.º {actual.Numero} guardado el {actual.Fin:dd/MM/yyyy HH:mm}. Usuario: {actual.Usuario}.";
            }
        }
        // realiza la limpieza de salida tanto si la operacion termina bien como si falla
        finally
        {
            cargando = false;
        }

        ActualizarResumen();
        if (actual.Numero == 0 && actual.Fecha.Date == DateTime.Today)
        {
            GuardarDatosAbiertos();
        }
    }

    // resuelve actualizar resumen con los datos recibidos y entrega el resultado al codigo que lo llama
    private void ActualizarResumen()
    {
        if (cargando)
        {
            return;
        }

        if (actual.Numero == 0)
        {
            actual.SaldoInicial = saldo.Value;
            actual.SaldoInicialUsd = saldoUsd.Value;
        }

        resumen.Text = $"Cobros e ingresos ARS: {actual.Ingresos("ARS"):N2}    Gastos ARS: {actual.Gastos("ARS"):N2}    Balance ARS: {actual.BalanceArs:N2}\n" + $"Ingresos por café ARS (incluidos en los ingresos): {actual.CafeArs:N2}\n" + $"Ingresos USD: {actual.Ingresos("USD"):N2}    Gastos USD: {actual.Gastos("USD"):N2}    Balance USD: {actual.SaldoInicialUsd + actual.Ingresos("USD") - actual.Gastos("USD"):N2}\n" + string.Join("    ", TiposCaja.Todos.Select(m => $"{m} ARS: {actual.Ingresos("ARS", m):N2}")) + "\n" + "Los ingresos se suman desde los pagos registrados. Cada cobro identifica su remito. USD separado de ARS.";
    }

    // resuelve cerrar con los datos recibidos y entrega el resultado al codigo que lo llama
    private void Cerrar()
    {
        if (!Validate())
        {
            return;
        }

        ActualizarResumen();
        actual.Inicio = fecha.Value.Date + inicio.Value.TimeOfDay;
        actual.Observaciones = observaciones.Text.Trim();
        // conserva la caja confirmada aunque el reloj cambie de dia mientras el mensaje esta abierto
        CierreCaja cierreConfirmado = actual;
        if (MessageBox.Show(this,
            $"{resumen.Text}\n\nATENCIÓN: REVISE TODOS LOS COBROS, GASTOS Y SALDOS ANTES DE CERRAR LA CAJA DEL {fecha.Value:dd/MM/yyyy}.\n\n" +
            "UNA VEZ CERRADA, NO SE PODRÁN REGISTRAR MÁS COBROS NI MOVIMIENTOS DE CAJA PARA ESA FECHA.\n\n" +
            "¿CONFIRMA QUE REVISÓ TODO Y DESEA CERRAR LA CAJA?",
            "CONFIRMAR CIERRE DE CAJA", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        {
            return;
        }

        CierreCaja cerrado = negocio.Cerrar(cierreConfirmado);
        Cargar();
        MessageBox.Show(this, $"Cierre N.º {cerrado.Numero} guardado. Puede imprimirlo o consultarlo nuevamente por fecha.", "Caja");
    }

    // resuelve ejecutar con los datos recibidos y entrega el resultado al codigo que lo llama
    private void Ejecutar(Action accion)
    {
        try
        {
            accion();
        }
        // atiende el error de esta operacion antes de informar o propagar el problema
        catch (Exception error)
        {
            MessageBox.Show(this, error.Message, "Caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    // lee los campos actuales y construye la solicitud; este metodo no guarda cambios ni modifica la seleccion
    private MovimientoCaja ObtenerMovimientoDelFormulario()
    {
        return new MovimientoCaja
        {
            Concepto = motivo.Text,
            Tipo = tipo.Text,
            Medio = medio.Text,
            Moneda = moneda.Text,
            Importe = importe.Value,
            Cuenta = cuenta.Text

        };
    }
}
