using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion
{
    public partial class frmEntregas : Form
    {
        private readonly CN_Entrega negocio=new();
        private ProductoPorEntregar? Producto=>dgvPendientes.CurrentRow?.DataBoundItem as ProductoPorEntregar;
        public frmEntregas()
        {
            InitializeComponent();
            ConfigurarDistribucion();
            EstiloGestion.Grilla(dgvPendientes); EstiloGestion.Grilla(dgvEntregas);
            EstiloGestion.Columna(dgvPendientes,"IdVenta","Venta",50);
            EstiloGestion.Columna(dgvPendientes,"Cliente","Cliente",120);
            EstiloGestion.Columna(dgvPendientes,"Producto","Producto",200);
            EstiloGestion.Columna(dgvPendientes,"Modalidad","Modalidad",110);
            EstiloGestion.Columna(dgvPendientes,"Vendidos","Vendidos",65);
            EstiloGestion.Columna(dgvPendientes,"Preparados","Preparados",70);
            EstiloGestion.Columna(dgvPendientes,"Pendientes","Por preparar",70);
            EstiloGestion.Columna(dgvEntregas,"IdVenta","Venta",50);
            EstiloGestion.Columna(dgvEntregas,"NumeroRemito","Remito",50);
            EstiloGestion.Columna(dgvEntregas,"Cliente","Cliente",100);
            EstiloGestion.Columna(dgvEntregas,"Productos","Productos",170);
            EstiloGestion.Columna(dgvEntregas,"Direccion","Destino",130);
            EstiloGestion.Columna(dgvEntregas,"Estado","Estado",80);
            EstiloGestion.Columna(dgvEntregas,"FechaEntrega","Entregado el",105,"dd/MM/yyyy HH:mm");
            Load+=(_,_)=>Buscar(); btnBuscar.Click+=(_,_)=>Buscar();
            txtBuscar.KeyDown+=(_,e)=>{if(e.KeyCode==Keys.Enter){e.SuppressKeyPress=true;Buscar();}};
            dgvPendientes.SelectionChanged+=(_,_)=>Seleccionar();
            dgvEntregas.SelectionChanged+=(_,_)=>ActualizarConfirmacion();
            btnPreparar.Click+=Preparar; btnConfirmar.Click+=Confirmar;
        }
        private void Buscar()
        {
            try
            {
                var pendientes=negocio.Pendientes(txtBuscar.Text);
                var entregas=negocio.Listar(txtBuscar.Text,chkPendientes.Checked);
                dgvPendientes.DataSource=pendientes; dgvEntregas.DataSource=entregas;
                lblPendientes.Text=$"Productos por preparar · {pendientes.Count}";
                lblEntregas.Text=$"Entregas preparadas · {entregas.Count}";
            }
            catch(Exception ex){dgvPendientes.DataSource=null;dgvEntregas.DataSource=null;Error(ex);}
            Seleccionar(); ActualizarConfirmacion();
        }
        private void Seleccionar()
        {
            var producto=Producto;
            lblSeleccion.Text=producto==null?"Seleccione un producto para preparar su entrega":$"Venta #{producto.IdVenta} · {producto.Cliente} · {producto.Producto}";
            nudCantidad.Value=1;
            txtDireccion.Text=producto?.Direccion??"";
            txtHorario.Clear();
            bool retiro=producto?.Modalidad.StartsWith("Retiro",StringComparison.OrdinalIgnoreCase)==true;
            if(retiro)txtDireccion.Text="Retiro en local";
            txtDireccion.ReadOnly=retiro;
            btnPreparar.Enabled=producto!=null && PuedeGestionar();
        }
        private bool PuedeGestionar()=>SesionActual.Usuario!=null && Permisos.TienePermiso(Permisos.GestionarEntregas);
        private void ActualizarConfirmacion()=>btnConfirmar.Enabled=dgvEntregas.CurrentRow?.DataBoundItem is EntregaRegistrada entrega && entrega.Estado=="Pendiente" && PuedeGestionar();
        private void Preparar(object? sender,EventArgs e)
        {
            if(Producto==null)return;
            try
            {
                btnPreparar.Enabled=false;
                // Negocio valida; datos guarda remito, envío, stock y actividad en una transacción.
                negocio.Preparar(Producto,(int)nudCantidad.Value,txtDireccion.Text,txtHorario.Text);
                Buscar();
                MessageBox.Show(this,"Entrega preparada y stock reservado. Selecciónela abajo para confirmar cuando se entregue.","Entregas");
            }
            catch(Exception ex){Error(ex);btnPreparar.Enabled=PuedeGestionar();}
        }
        private void Confirmar(object? sender,EventArgs e)
        {
            if(dgvEntregas.CurrentRow?.DataBoundItem is not EntregaRegistrada entrega)return;
            try
            {
                btnConfirmar.Enabled=false;
                negocio.Confirmar(entrega.IdEnvio);
                Buscar();
                MessageBox.Show(this,"Entrega confirmada correctamente.","Entregas");
            }
            catch(Exception ex){Error(ex);ActualizarConfirmacion();}
        }
        private void Error(Exception ex)=>MessageBox.Show(this,ex.Message,"Entregas",MessageBoxButtons.OK,MessageBoxIcon.Error);
    }
}
