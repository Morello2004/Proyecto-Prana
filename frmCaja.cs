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
    // conserva las aperturas visibles para saber si ya existe un turno abierto en la fecha
    private List<CierreCaja> cajasDelDia = new List<CierreCaja>();
    // prepara frm caja antes de que se utilice la instancia
    public frmCaja()
    {
        InitializeComponent();
        anular.Enabled = SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.AnularMovimientosCaja);
        // el inicio es la hora real de apertura no un horario editable para mover cobros entre turnos
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

    // guarda cambios solo despues de abrir la caja los datos de una apertura nueva se confirman con su boton
    private void DatosCierreCambiados(object? sender, EventArgs e)
    {
        if (cargando || actual.IdCaja == 0 || actual.Cerrada || actual.Fecha.Date != fecha.Value.Date || fecha.Value.Date != DateTime.Today)
        {
            return;
        }
        Ejecutar(GuardarDatosAbiertos);
    }

    // resuelve guardar datos abiertos con los datos recibidos y entrega el resultado al codigo que lo llama
    private void GuardarDatosAbiertos()
    {
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
    // vuelve a consultar las aperturas y conserva la seleccion si todavia pertenece a la fecha elegida
    private void Cargar()
    {
        negocio.CerrarDiasAnteriores();
        int? seleccionada = null;
        if (cboCaja.SelectedItem is CierreCaja elegida && elegida.Fecha.Date == fecha.Value.Date)
        {
            seleccionada = elegida.IdCaja;
        }
        cargando = true;
        try
        {
            cajasDelDia = negocio.Listar(fecha.Value.Date);
            cboCaja.DataSource = cajasDelDia;
            CierreCaja? opcion = cajasDelDia.FirstOrDefault(caja => caja.IdCaja == seleccionada);
            if (opcion == null)
            {
                opcion = cajasDelDia.FirstOrDefault();
            }
            if (opcion != null)
            {
                cboCaja.SelectedItem = opcion;
                actual = negocio.Consultar(fecha.Value.Date, opcion.IdCaja);
            }
            else
            {
                actual = new CierreCaja { Fecha = fecha.Value.Date, Inicio = DateTime.Now };
            }
            MostrarCaja();
        }
        // realiza la limpieza de salida tanto si la operacion termina bien como si falla
        finally
        {
            cargando = false;
        }

        ActualizarResumen();
    }

    // presenta un turno o prepara los campos de una nueva apertura sin guardarla automaticamente
    private void MostrarCaja()
    {
        observaciones.Text = actual.Observaciones;
        movimientos.DataSource = actual.Movimientos;
        if (movimientos.Columns.Contains(nameof(MovimientoCaja.IdCaja)))
        {
            movimientos.Columns[nameof(MovimientoCaja.IdCaja)].Visible = false;
        }
        bool hoy = fecha.Value.Date == DateTime.Today;
        bool abierta = actual.IdCaja > 0 && !actual.Cerrada && hoy;
        bool puedeAbrir = hoy && !cajasDelDia.Any(caja => !caja.Cerrada);
        registrar.Enabled = cerrar.Enabled = abierta;
        imprimir.Enabled = actual.Cerrada && actual.Numero > 0;
        observaciones.Enabled = abierta || (puedeAbrir && actual.IdCaja == 0);
        abrir.Enabled = puedeAbrir;
        abrir.Text = "Abrir caja";
        if (actual.IdCaja > 0)
        {
            abrir.Text = "Nueva caja";
        }
        anular.Enabled = abierta && SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.AnularMovimientosCaja);
        if (actual.Cerrada)
        {
            estado.Text = $"Cierre N.º {actual.Numero} del {actual.Fin:dd/MM/yyyy HH:mm}. Usuario: {actual.Usuario}.";
            if (puedeAbrir)
            {
                estado.Text += " Puede preparar otra apertura con Nueva caja.";
            }
        }
        else if (abierta)
        {
            estado.Text = $"Caja N.º {actual.IdCaja} abierta desde {actual.Inicio:HH:mm}. Los movimientos corresponden solamente a esta apertura.";
        }
        else
        {
            estado.Text = "No hay cajas registradas para esta fecha.";
            if (hoy)
            {
                estado.Text = "Presione Abrir caja antes de registrar operaciones.";
            }
        }
    }

    // cambiar la seleccion consulta por identificador para no juntar cierres de la misma fecha
    private void cboCaja_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!cargando)
        {
            Ejecutar(Cargar);
        }
    }

    // el primer paso prepara datos vacios y el segundo confirma la apertura sin copiar el balance anterior
    private void abrir_Click(object? sender, EventArgs e)
    {
        Ejecutar(() =>
        {
            if (actual.IdCaja > 0)
            {
                cargando = true;
                try
                {
                    cboCaja.SelectedIndex = -1;
                    actual = new CierreCaja();
                    MostrarCaja();
                }
                finally { cargando = false; }
                ActualizarResumen();
                observaciones.Focus();
                return;
            }
            CierreCaja nueva = negocio.Abrir(new CierreCaja
            {
                Observaciones = observaciones.Text.Trim()
            });
            Cargar();
            MessageBox.Show(this, $"Caja N.º {nueva.IdCaja} abierta correctamente.", "Caja");
        });
    }

    // resuelve actualizar resumen con los datos recibidos y entrega el resultado al codigo que lo llama
    private void ActualizarResumen()
    {
        if (cargando)
        {
            return;
        }



        resumen.Text = $"Cobros e ingresos ARS: {actual.Ingresos("ARS"):N2}    Gastos ARS: {actual.Gastos("ARS"):N2}    Balance ARS: {actual.BalanceArs:N2}\n" +
            $"Ingresos por café ARS (incluidos en los ingresos): {actual.CafeArs:N2}\n" +
            $"Ingresos USD: {actual.Ingresos("USD"):N2}    Gastos USD: {actual.Gastos("USD"):N2}    Balance USD: {actual.Ingresos("USD") - actual.Gastos("USD"):N2}\n" +
            string.Join("    ", TiposCaja.Todos.Select(m => $"{m} ARS: {actual.Ingresos("ARS", m):N2}"));
    }

    // resuelve cerrar con los datos recibidos y entrega el resultado al codigo que lo llama
    private void Cerrar()
    {
        if (!Validate())
        {
            return;
        }

        ActualizarResumen();
        actual.Observaciones = observaciones.Text.Trim();
        // conserva la caja confirmada aunque el reloj cambie de dia mientras el mensaje esta abierto
        CierreCaja cierreConfirmado = actual;
        if (MessageBox.Show(this,
            $"{resumen.Text}\n\nATENCIÓN: REVISE TODOS LOS COBROS, GASTOS Y SALDOS ANTES DE CERRAR LA CAJA DEL {fecha.Value:dd/MM/yyyy}.\n\n" +
            "UNA VEZ CERRADA, NO SE PODRÁN AGREGAR OPERACIONES A ESTA CAJA. PODRÁ ABRIR OTRA PARA CONTINUAR EN EL MISMO DÍA.\n\n" +
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
            IdCaja = actual.IdCaja,
            Concepto = motivo.Text,
            Tipo = tipo.Text,
            Medio = medio.Text,
            Moneda = moneda.Text,
            Importe = importe.Value,
            Cuenta = cuenta.Text

        };
    }
}
