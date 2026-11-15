using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm panel principal
    public partial class frmPanelPrincipal : Form
    {
        // conserva negocio ventas entre las operaciones de esta clase
        private readonly CN_Venta negocioVentas = new();
        // prepara frm panel principal antes de que se utilice la instancia
        public frmPanelPrincipal()
        {
            InitializeComponent();
            // muestra solamente las columnas definidas en esta pantalla
            dgvUltimasVentas.AutoGenerateColumns = false;
            colTotal.DefaultCellStyle.Format = "$ #,##0.00";
            colTotal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        

        // prepara la informacion inicial cuando se carga la ventana
        private void frmPanelPrincipal_Load(object? sender, EventArgs e) => CargarVentas();
        // presentacion solo consulta negocio y enlaza resultados; no calcula deudas
        private void CargarVentas()
        {
            try
            {
            
                // una busqueda vacia devuelve las ultimas diez ventas y con texto devuelve las coincidencias
                List<venta> ventas = negocioVentas.BuscarVentas(txtBuscarVenta.Text);
                dgvUltimasVentas.DataSource = ventas;
                // comprueba si hay elementos para continuar con esta operacion
                if (ventas.Count == 0)
                {
                    lblResultados.Text = "No se encontraron ventas";
                }
                else
                {
                    // distingue un dato vacio de un valor ingresado antes de continuar
                    if (string.IsNullOrWhiteSpace(txtBuscarVenta.Text))
                    {
                        lblResultados.Text = $"Últimas 10 ventas · {ventas.Count} registradas";
                    }
                    else
                    {
                        lblResultados.Text = $"Resultados de búsqueda · {ventas.Count} ventas";
                    }
                }
                // muestra el resumen general de ventas del dia y deudas pendientes sin depender del filtro de la grilla
                ResumenVentas resumen = negocioVentas.ObtenerResumen();
                lblTotalHoy.Text = "$ " + resumen.TotalHoy.ToString("N2");
                lblCantidadHoy.Text = $"{resumen.CantidadHoy} operaciones registradas hoy";
                lblTotalPendiente.Text = "$ " + resumen.SaldoPendiente.ToString("N2");
                lblCantidadPendientes.Text = resumen.VentasPendientes.ToString();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                // limpia los resultados para no presentar informacion anterior como si la consulta hubiera terminado bien
                dgvUltimasVentas.DataSource = null;
                lblResultados.Text = "No se pudieron cargar las ventas. Presione Actualizar para reintentar.";
                lblTotalHoy.Text = lblTotalPendiente.Text = lblCantidadPendientes.Text = "—";
                lblCantidadHoy.Text = "Sin datos disponibles";
                MessageBox.Show(this, error.Message, "Consulta de ventas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        // ejecuta la accion de btn buscar cuando el usuario pulsa el control
        private void btnBuscar_Click(object? sender, EventArgs e) => CargarVentas();
        // ejecuta la accion de btn ultimas cuando el usuario pulsa el control
        private void btnUltimas_Click(object? sender, EventArgs e)
        {
            txtBuscarVenta.Clear();
            CargarVentas();
        }

      

        // ejecuta la accion de btn nueva venta cuando el usuario pulsa el control
        private void btnNuevaVenta_Click(object? sender, EventArgs e)
        {// parent es el contenedor inmediato del formulario es pnlcontenido porque ahi se puso el panel principal
            if (Parent?.FindForm() is Interfaz interfaz)
            {
                interfaz.AbrirFormulario(new frmNuevaVenta());
            }
        }

        // ejecuta la accion de btn stock cuando el usuario pulsa el control
        private void btnStock_Click(object? sender, EventArgs e)
        {
            if (Parent?.FindForm() is Interfaz interfaz)
            {
                interfaz.AbrirFormulario(new frmStock());
            }
        }

        // se ejecuta cuando cambia la fila seleccionada y tambien puede ejecutarse al cargar la grilla
        private void dgvUltimasVentas_SelectionChanged(object? sender, EventArgs e)
        {
            // currentrow obtiene la fila actual y databounditem obtiene su objeto asociado el signo ? evita acceder al objeto si no hay fila y is guarda la venta encontrada en seleccionada
            if (dgvUltimasVentas.CurrentRow?.DataBoundItem is venta seleccionada)
            {
                // muestra el numero y los productos de la venta seleccionada el signo $ permite insertar los valores entre llaves dentro del texto
                lblDetalle.Text = $"Venta #{seleccionada.IdVenta} · Productos: {seleccionada.Productos}";
            }
            else
            {
                // si no hay una venta seleccionada muestra una indicacion en lugar del detalle anterior
                lblDetalle.Text = "Seleccione una venta para ver sus productos.";
            }
        }

        // este evento permite cambiar como se muestra una celda pero actualmente esta vacio y no realiza ninguna accion
        private void dgvUltimasVentas_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        // se ejecuta al presionar el boton de documentos para consultar los comprobantes de la venta seleccionada
        private void documentos_Click(object? sender, EventArgs e)
        {
            // obtiene el objeto de la fila actual si no es una venta entra al bloque de aviso si es una venta la guarda en seleccionada para usarla mas abajo
            if (dgvUltimasVentas.CurrentRow?.DataBoundItem is not venta seleccionada)
            {
                // avisa que falta seleccionar una venta y return termina el metodo para no intentar mostrar documentos sin una venta
                MessageBox.Show("Seleccione una venta del listado.");
                return;
            }

            // intenta mostrar los documentos y permite atender un error mediante el catch
            try
            {
                // this identifica esta ventana y idventa indica de que venta se deben mostrar los documentos
                DocumentosPrana.MostrarVenta(this, seleccionada.IdVenta);
            }
            // guarda en error la excepcion que se produzca dentro del try
            catch (Exception error)
            {
                // muestra el mensaje del error en un cuadro con titulo comprobantes boton aceptar e icono de advertencia
                MessageBox.Show(this, error.Message, "Comprobantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // se ejecuta al presionar el boton de caja
        private void caja_Click(object? sender, EventArgs e)
        {
            // parent es el contenedor de este formulario findform busca la ventana que lo contiene y si es de tipo interfaz la guarda en la variable interfaz el signo ? evita la llamada si no hay contenedor
            if (Parent?.FindForm() is Interfaz interfaz)
            {
                // crea el formulario de caja y pide a la ventana interfaz que lo muestre en su panel de contenido
                interfaz.AbrirFormulario(new frmCaja());
            }
        }

        // este evento corresponde al clic sobre el total de hoy pero esta vacio y no realiza ninguna accion
        private void lblTotalHoy_Click(object sender, EventArgs e)
        {

        }
    }
}
