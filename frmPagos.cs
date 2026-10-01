using Prana.Entidades;
using Prana.negocio;
using System.Globalization;

namespace Prana.Presentacion
{
    public partial class frmPagos : Form
    {
        private readonly CN_Pago negocio=new();
        private bool cargando;
        private readonly TextBox txtCuentaDestino = new() { MaxLength=100 };
        private venta? Seleccionada => dgvVentas.CurrentRow?.DataBoundItem as venta;
        public frmPagos()
        {
            InitializeComponent();
            ConfigurarDistribucion();
            lblImporte.Text = "Importe a cancelar";
            EstiloGestion.Grilla(dgvVentas); EstiloGestion.Grilla(dgvPagos);
            EstiloGestion.Columna(dgvVentas,"IdVenta","Venta",50);
            EstiloGestion.Columna(dgvVentas,"NombreCompletoCliente","Cliente",160);
            EstiloGestion.Columna(dgvVentas,"Fecha","Fecha",100,"dd/MM/yyyy");
            EstiloGestion.Columna(dgvVentas,"Total","Total",100,"C2");
            EstiloGestion.Columna(dgvVentas,"Pagado","Pagado",100,"C2");
            EstiloGestion.Columna(dgvVentas,"SaldoPendiente","Saldo pendiente",110,"C2");
            EstiloGestion.Columna(dgvVentas,"Estado","Estado",85);
            EstiloGestion.Columna(dgvPagos,"IdPago","Pago",50);
            EstiloGestion.Columna(dgvPagos,"Fecha","Fecha",120,"dd/MM/yyyy HH:mm");
            EstiloGestion.Columna(dgvPagos,"Medio","Medio de pago",130);
            EstiloGestion.Columna(dgvPagos,"Cuotas","Cuotas",60);
            EstiloGestion.Columna(dgvPagos,"Cobrado","Total cobrado",120,"C2");
            cboCuotas.SelectedIndex=0;
            ActualizarCuotas();
            Load+=(_,_)=>Buscar();
            btnBuscar.Click+=(_,_)=>Buscar();
            txtBuscar.KeyDown+=(_,e)=>{if(e.KeyCode==Keys.Enter){e.SuppressKeyPress=true;Buscar();}};
            dgvVentas.SelectionChanged+=(_,_)=>SeleccionarVenta();
            nudImporte.ValueChanged+=(_,_)=>MostrarCobro();
            cboMedio.SelectedIndexChanged+=(_,_)=>{ActualizarCuotas();MostrarCobro();};
            cboCuotas.SelectedIndexChanged+=(_,_)=>MostrarCobro();
            btnRegistrar.Click+=Registrar;
        }
        private void Buscar()
        {
            try
            {
                cargando=true;
                cboMedio.DisplayMember=nameof(MedioPago.NombreMedioPago);
                // Orden de presentación: Efectivo primero y seleccionado al cargar.
                cboMedio.DataSource=new CN_MedioPago().ObtenerTodos()
                    .OrderBy(m=>!m.NombreMedioPago.Trim().Equals("Efectivo",StringComparison.OrdinalIgnoreCase))
                    .ThenBy(m=>m.NombreMedioPago).ToList();
                if(cboMedio.Items.Count>0)cboMedio.SelectedIndex=0;
                ActualizarCuotas();
                var ventas=negocio.Buscar(txtBuscar.Text,chkPendientes.Checked);
                dgvVentas.DataSource=ventas;
                lblVentas.Text=ventas.Count==0?"No se encontraron ventas":$"Ventas · {ventas.Count} resultados";
            }
            catch(Exception ex){dgvVentas.DataSource=null;Error(ex);}
            finally{cargando=false;SeleccionarVenta();}
        }
        private void SeleccionarVenta()
        {
            if(cargando)return;
            dgvPagos.DataSource=null;
            nudImporte.Value=0;
            var venta=Seleccionada;
            lblSeleccion.Text=venta==null?"Seleccione una venta para registrar un pago":$"Venta #{venta.IdVenta} · {venta.NombreCompletoCliente} · Saldo: {venta.SaldoPendiente.ToString("C2",CultureInfo.GetCultureInfo("es-AR"))}";
            lblPagos.Text="Pagos registrados de la venta seleccionada";
            if(venta!=null)
            {
                try
                {
                    var pagos=negocio.Listar(venta.IdVenta);
                    dgvPagos.DataSource=pagos;
                    lblPagos.Text=$"Pagos registrados · {pagos.Count}";
                    nudImporte.Value=Math.Min(nudImporte.Maximum,venta.SaldoPendiente);
                }
                catch(Exception ex){Error(ex);}
            }
            MostrarCobro();
        }
        // Oculta las cuotas fuera de Crédito y evita conservar cuotas de una selección anterior.
        private void ActualizarCuotas()
        {
            bool credito=cboMedio.SelectedItem is MedioPago medio &&
                CultureInfo.GetCultureInfo("es-AR").CompareInfo.IndexOf(medio.NombreMedioPago,"credito",
                    CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace)>=0;
            lblCuotas.Visible=cboCuotas.Visible=credito;
            cboCuotas.Enabled=credito;
            if(!credito)cboCuotas.SelectedIndex=0;
        }

        private void MostrarCobro()
        {
            btnRegistrar.Enabled=false;
            lblCobro.Text="Ingrese el importe y seleccione el medio de pago.";
            if(cargando || Seleccionada==null || cboMedio.SelectedItem is not MedioPago medio || cboCuotas.SelectedItem is not int cuotas || nudImporte.Value<=0)return;
            try
            {
                // El cálculo del recargo pertenece a negocio; el formulario solo presenta la cotización.
                var cobro=negocio.Preparar(Seleccionada.IdVenta,medio,cuotas,nudImporte.Value);
                lblCobro.Text=$"Total a cobrar: {cobro.ImporteBase+cobro.Recargo:C2}";
                btnRegistrar.Enabled=nudImporte.Value<=Seleccionada.SaldoPendiente && SesionActual.Usuario!=null && Permisos.TienePermiso(Permisos.RegistrarPagos);
            }
            catch(Exception ex){lblCobro.Text=ex.Message;}
        }
        private void Registrar(object? sender,EventArgs e)
        {
            if(Seleccionada==null || cboMedio.SelectedItem is not MedioPago medio || cboCuotas.SelectedItem is not int cuotas)return;
            try
            {
                btnRegistrar.Enabled=false;
                negocio.Registrar(Seleccionada.IdVenta,medio,cuotas,nudImporte.Value,txtCuentaDestino.Text);
                txtCuentaDestino.Clear();
                MessageBox.Show(this,"Pago registrado correctamente.","Pagos",MessageBoxButtons.OK,MessageBoxIcon.Information);
                Buscar();
            }
            catch(Exception ex){Error(ex);MostrarCobro();}
        }
        private void Error(Exception ex)=>MessageBox.Show(this,ex.Message,"Pagos",MessageBoxButtons.OK,MessageBoxIcon.Error);
    }
}
