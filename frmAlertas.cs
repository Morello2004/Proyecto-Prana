using Prana.Entidades;
using Prana.negocio;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm alertas
    public partial class frmAlertas : Form
    {
        // conserva negocio entre las operaciones de esta clase
        private readonly CN_InformeVentas negocio = new();
        // conserva negocio entregas entre las operaciones de esta clase
        private readonly CN_Entrega negocioEntregas = new();
        // prepara frm alertas antes de que se utilice la instancia
        public frmAlertas()
        {
            InitializeComponent();
            grilla.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        // prepara la informacion inicial cuando se carga la ventana
        private async void frmAlertas_Load(object? sender, EventArgs e) => await CargarAsync();
        // obtiene y presenta async para actualizar los controles de la pantalla
        private async Task CargarAsync()
        {
            string etapa = "pagos pendientes";
            try
            {
                estado.Text = "Consultando pagos pendientes...";
                List<AlertaPagoPendiente> alertas = await Task.Run(negocio.ObtenerAlertas);
                // comprueba si hay elementos para continuar con esta operacion
                if (alertas.Count == 0)
                {
                    estado.Text = "No hay pagos pendientes vencidos.";
                }
                else
                {
                    estado.Text = $"{alertas.Count} venta(s) requieren seguimiento.";
                }
                etapa = "faltantes de stock";
                lblStock.Text = "Consultando faltantes de stock...";
                List<AlertaStockPendiente> alertasStock = await Task.Run(negocioEntregas.ObtenerAlertasStock);
                grillaStock.DataSource = alertasStock;
                // grillastockcolumns[nameof(alertastockpendienteidvariante)]visible: determina si el control ocupa una zona visible de la pantalla
                grillaStock.Columns[nameof(AlertaStockPendiente.IdVariante)].Visible = false;
                // grillastockcolumns[nameof(alertastockpendienteproducto)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grillaStock.Columns[nameof(AlertaStockPendiente.Producto)].HeaderText = "Producto / variante";
                // grillastockcolumns[nameof(alertastockpendientependientes)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grillaStock.Columns[nameof(AlertaStockPendiente.Pendientes)].HeaderText = "Pendientes de preparar";
                // grillastockcolumns[nameof(alertastockpendientedisponible)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grillaStock.Columns[nameof(AlertaStockPendiente.Disponible)].HeaderText = "Stock disponible";
                // grillastockcolumns[nameof(alertastockpendientefaltantes)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grillaStock.Columns[nameof(AlertaStockPendiente.Faltantes)].HeaderText = "Unidades faltantes";
                // comprueba si hay elementos para continuar con esta operacion
                if (alertasStock.Count == 0)
                {
                    lblStock.Text = "No hay faltantes para las ventas pendientes.";
                }
                else
                {
                    lblStock.Text = $"{alertasStock.Count} variante(s) necesitan ingreso de stock.";
                }
                grilla.DataSource = alertas;
                // comprueba si hay elementos para continuar con esta operacion
                if (grilla.Columns.Count == 0)
                {
                    return;
                }

                // grillacolumns[nameof(alertapagopendienteidventa)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.IdVenta)].HeaderText = "Venta";
                // grillacolumns[nameof(alertapagopendientecliente)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.Cliente)].HeaderText = "Cliente";
                // grillacolumns[nameof(alertapagopendienteproductos)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.Productos)].HeaderText = "Productos";
                // grillacolumns[nameof(alertapagopendienteprimerpago)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.PrimerPago)].HeaderText = "Primer pago";
                // grillacolumns[nameof(alertapagopendientetotal)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.Total)].HeaderText = "Total";
                // grillacolumns[nameof(alertapagopendientepagado)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.Pagado)].HeaderText = "Pagado";
                // grillacolumns[nameof(alertapagopendientesaldo)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.Saldo)].HeaderText = "Saldo";
                // grillacolumns[nameof(alertapagopendientediasvencida)]headertext: titulo de la columna; no es el nombre de la propiedad enlazada
                grilla.Columns[nameof(AlertaPagoPendiente.DiasVencida)].HeaderText = "Días vencida";
                // grillacolumns[nameof(alertapagopendienteprimerpago)]defaultcellstyleformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
                grilla.Columns[nameof(AlertaPagoPendiente.PrimerPago)].DefaultCellStyle.Format = "dd/MM/yyyy";
                // procesa cada nombre de new[] { nameof(alerta pago pendientetotal), nameof(alerta pago pendientepagado), nameof(alerta pago pendientesaldo) } sin cambiar el orden de la coleccion
                foreach (string nombre in new[]
                {
                    nameof(AlertaPagoPendiente.Total),
                    nameof(AlertaPagoPendiente.Pagado),
                    nameof(AlertaPagoPendiente.Saldo)
                }

                )
                {
                    // grillacolumns[nombre]defaultcellstyleformat: formato de presentacion, por ejemplo moneda o fecha; no modifica el valor original
                    grilla.Columns[nombre].DefaultCellStyle.Format = "C2";
                }

                grilla.Columns[nameof(AlertaPagoPendiente.Productos)].Width = 280;
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                if (etapa == "pagos pendientes")
                {
                    estado.Text = "Error al consultar pagos pendientes.";
                }
                else
                {
                    lblStock.Text = "Error al consultar faltantes de stock.";
                }

                MessageBox.Show(this, error.Message, "Error al cargar alertas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // interpreta la columna y fila pulsadas para ejecutar la accion de esa celda; los encabezados no son registros
        private void grilla_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}
