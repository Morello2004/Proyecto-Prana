using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion;

public partial class frmCaja : Form
{
    private readonly CN_Caja negocio = new();
    private CierreCaja actual = new();
    private bool cargando;

    public frmCaja()
    {
        InitializeComponent();
        inicio.Value = DateTime.Today.AddHours(9);
        EstiloGestion.Grilla(movimientos);
        foreach(var (prop,tituloCol,peso) in new[]{("Fecha","Hora",65),("Venta","Remito",70),("Tipo","Tipo",90),
            ("Concepto","Cliente / motivo",180),("Medio","Medio",90),("Moneda","Moneda",60),("Importe","Importe",90),("Cuenta","Cuenta",100)})
            EstiloGestion.Columna(movimientos,prop,tituloCol,peso,prop=="Importe"?"N2":prop=="Fecha"?"HH:mm":"");
    }

    private void movimientos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e) {
            if(movimientos.Columns[e.ColumnIndex].DataPropertyName=="Venta" && e.Value is int numero)
            { e.Value=$"RTO {numero}"; e.FormattingApplied=true; }
        }

    private void saldo_ValueChanged(object? sender, EventArgs e) { ActualizarResumen(); }

    private void saldoUsd_ValueChanged(object? sender, EventArgs e) { ActualizarResumen(); }

    private void resumenMes_Click(object? sender, EventArgs e)
    {
        using var pantalla = new frmResumenMensualCaja(fecha.Value);
        pantalla.ShowDialog(this);
    }

    private void actualizar_Click(object? sender, EventArgs e) { Ejecutar(Cargar); }

    private void fecha_ValueChanged(object? sender, EventArgs e) { Ejecutar(Cargar); }

    private void registrar_Click(object? sender, EventArgs e) { Ejecutar(()=> {
            negocio.Registrar(new MovimientoCaja { Concepto=motivo.Text, Tipo=tipo.Text,Medio=medio.Text,Moneda=moneda.Text,Importe=importe.Value,Cuenta=cuenta.Text });
            motivo.Clear(); cuenta.Clear(); importe.Value=0; Cargar();
        }); }

    private void cerrar_Click(object? sender, EventArgs e) { Ejecutar(Cerrar); }

    private void imprimir_Click(object? sender, EventArgs e) { Ejecutar(()=>DocumentosPrana.Abrir(DocumentosPrana.GuardarCierre(actual))); }

    private void anular_Click(object? sender, EventArgs e) { Ejecutar(()=> {
            if(movimientos.CurrentRow?.DataBoundItem is not MovimientoCaja m || m.Venta.HasValue) throw new InvalidOperationException("Seleccione un gasto o ingreso manual. Los cobros de ventas no se anulan desde aquí.");
            using var dialogo = new frmMotivoAnulacion();
            if(dialogo.ShowDialog(this)!=DialogResult.OK)return;
            negocio.Anular(m.Id,dialogo.Motivo); Cargar();
        }); }

    private void frmCaja_Load(object? sender, EventArgs e) => Ejecutar(Cargar);

    private void Cargar()
    {
        var consultado = negocio.Consultar(fecha.Value.Date);
        bool mismoDia = actual.Fecha.Date==consultado.Fecha.Date;
        cargando=true;
        try
        {
            actual=consultado;
            if(actual.Numero>0 || !mismoDia) { saldo.Value=actual.SaldoInicial; saldoUsd.Value=actual.SaldoInicialUsd; inicio.Value=actual.Inicio; observaciones.Text=actual.Observaciones; }
            movimientos.DataSource=actual.Movimientos;
            bool abierta=actual.Numero==0;
            registrar.Enabled=abierta && fecha.Value.Date==DateTime.Today;
            cerrar.Enabled=abierta && fecha.Value.Date<=DateTime.Today;
            imprimir.Enabled=!abierta; saldo.Enabled=saldoUsd.Enabled=inicio.Enabled=observaciones.Enabled=abierta;
            estado.Text=abierta ? "Caja sin cerrar. Los cobros se incluyen por su fecha de pago, aunque la venta sea de otro día."
                : $"Cierre N.º {actual.Numero} guardado el {actual.Fin:dd/MM/yyyy HH:mm}. Usuario: {actual.Usuario}.";
        }
        finally { cargando=false; }
        ActualizarResumen();
    }

    private void ActualizarResumen()
    {
        if(cargando)return;
        if(actual.Numero==0) { actual.SaldoInicial=saldo.Value; actual.SaldoInicialUsd=saldoUsd.Value; }
        resumen.Text=$"Cobros e ingresos ARS: {actual.Ingresos("ARS"):N2}    Gastos ARS: {actual.Gastos("ARS"):N2}    Balance ARS: {actual.BalanceArs:N2}\n"+
            $"Ingresos por café ARS (incluidos en los ingresos): {actual.CafeArs:N2}\n"+
            $"Ingresos USD: {actual.Ingresos("USD"):N2}    Gastos USD: {actual.Gastos("USD"):N2}    Balance USD: {actual.SaldoInicialUsd+actual.Ingresos("USD")-actual.Gastos("USD"):N2}\n"+
            string.Join("    ",TiposCaja.Todos.Select(m=>$"{m} ARS: {actual.Ingresos("ARS",m):N2}"))+"\n"+
            "Los ingresos se suman desde los pagos registrados. Cada cobro identifica su remito. USD separado de ARS.";
    }

    private void Cerrar()
    {
        if(!Validate())return;
        ActualizarResumen(); actual.Inicio=fecha.Value.Date+inicio.Value.TimeOfDay; actual.Observaciones=observaciones.Text.Trim();
        if(MessageBox.Show(this,$"{resumen.Text}\n\n¿Guardar el cierre del {fecha.Value:dd/MM/yyyy}? Después no se podrán agregar cobros ni movimientos a ese día.",
            "Cerrar caja",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)return;
        actual=negocio.Cerrar(actual); Cargar();
        MessageBox.Show(this,$"Cierre N.º {actual.Numero} guardado. Puede imprimirlo o consultarlo nuevamente por fecha.","Caja");
    }

    private void Ejecutar(Action accion) { try { accion(); } catch(Exception ex) { MessageBox.Show(this,ex.Message,"Caja",MessageBoxButtons.OK,MessageBoxIcon.Warning); } }
}
