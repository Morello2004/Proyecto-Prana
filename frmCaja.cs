using Prana.Entidades;
using Prana.negocio;

namespace Prana.Presentacion;

public class frmCaja : Form
{
    private readonly CN_Caja negocio = new();
    private readonly DateTimePicker fecha = new() { Format = DateTimePickerFormat.Short, Width = 130 };
    private readonly DateTimePicker inicio = new() { Format=DateTimePickerFormat.Time, ShowUpDown=true, Width=130, Value=DateTime.Today.AddHours(9) };
    private readonly NumericUpDown saldo = Dinero(), saldoUsd = Dinero(), importe = Dinero();
    private readonly ComboBox tipo = Lista("Gasto", "Ingreso extra", "Café"), medio = Lista(TiposCaja.Todos), moneda = Lista("ARS","USD");
    private readonly TextBox motivo = new() { MaxLength=200, Width=310 }, cuenta = new() { MaxLength=100, Width=210 };
    private readonly TextBox observaciones = new() { Multiline=true, Height=60, MaxLength=1000 };
    private readonly Label estado = new() { Height=36 }, resumen = new() { Height=110 };
    private readonly DataGridView movimientos = new() { Height=240 };
    private readonly Button registrar = Boton("Registrar movimiento"), cerrar = Boton("Cerrar caja del día"), imprimir = Boton("Ver / imprimir cierre");
    private CierreCaja actual = new();
    private bool cargando;

    public frmCaja()
    {
        Text="Caja y gastos"; ForeColor=Color.White; BackColor=EstiloGestion.Fondo; ClientSize=new Size(900,700);
        var titulo = new Label { Text="Caja y gastos", Font=new Font("Segoe UI",18,FontStyle.Bold), Height=42 };
        var actualizar = Boton("Consultar / actualizar");
        var filtros = DistribucionAdaptable.Fila(Campo("Fecha",fecha), actualizar);
        var nuevo = DistribucionAdaptable.Fila(Campo("Tipo",tipo),Campo("Motivo",motivo,310),Campo("Medio",medio),
            Campo("Moneda",moneda,90),Campo("Importe",importe),Campo("Cuenta / referencia",cuenta,210),registrar);
        var apertura = DistribucionAdaptable.Fila(Campo("Hora de inicio",inicio),Campo("Saldo inicial ARS",saldo),Campo("Saldo inicial USD",saldoUsd));
        var anular = Boton("Anular movimiento seleccionado"); anular.Width=250;
        var acciones = DistribucionAdaptable.Fila(cerrar,imprimir);
        DistribucionAdaptable.Pagina(this,titulo,filtros,estado,Titulo("Registrar gasto u otro ingreso (hoy)"),nuevo,
            Titulo("Cobros y movimientos del día"),movimientos,DistribucionAdaptable.Fila(anular),
            Titulo("Resumen y cierre"),apertura,resumen,Titulo("Observaciones del cierre"),observaciones,acciones);
        EstiloGestion.Grilla(movimientos);
        foreach(var (prop,tituloCol,peso) in new[]{("Fecha","Hora",65),("Venta","Remito",70),("Tipo","Tipo",90),
            ("Concepto","Cliente / motivo",180),("Medio","Medio",90),("Moneda","Moneda",60),("Importe","Importe",90),("Cuenta","Cuenta",100)})
            EstiloGestion.Columna(movimientos,prop,tituloCol,peso,prop=="Importe"?"N2":prop=="Fecha"?"HH:mm":"");
        movimientos.CellFormatting += (_,e)=> {
            if(movimientos.Columns[e.ColumnIndex].DataPropertyName=="Venta" && e.Value is int numero)
            { e.Value=$"RTO {numero}"; e.FormattingApplied=true; }
        };
        saldo.ValueChanged += (_,_)=>ActualizarResumen(); saldoUsd.ValueChanged += (_,_)=>ActualizarResumen();
        actualizar.Click += (_,_)=>Ejecutar(Cargar); fecha.ValueChanged += (_,_)=>Ejecutar(Cargar);
        registrar.Click += (_,_)=>Ejecutar(()=> {
            negocio.Registrar(new MovimientoCaja { Concepto=motivo.Text, Tipo=tipo.Text,Medio=medio.Text,Moneda=moneda.Text,Importe=importe.Value,Cuenta=cuenta.Text });
            motivo.Clear(); cuenta.Clear(); importe.Value=0; Cargar();
        });
        cerrar.Click += (_,_)=>Ejecutar(Cerrar);
        imprimir.Click += (_,_)=>Ejecutar(()=>DocumentosPrana.Abrir(DocumentosPrana.GuardarCierre(actual)));
        anular.Click += (_,_)=>Ejecutar(()=> {
            if(movimientos.CurrentRow?.DataBoundItem is not MovimientoCaja m || m.Venta.HasValue) throw new InvalidOperationException("Seleccione un gasto o ingreso manual. Los cobros de ventas no se anulan desde aquí.");
            using var dialogo = new Form { Text="Motivo de anulación",ClientSize=new Size(440,130),StartPosition=FormStartPosition.CenterParent };
            var texto = new TextBox { Left=16,Top=16,Width=405,MaxLength=200 };
            var aceptar = new Button { Text="Anular",Left=310,Top=65,Width=110,DialogResult=DialogResult.OK };
            dialogo.Controls.AddRange(new Control[]{texto,aceptar}); dialogo.AcceptButton=aceptar;
            if(dialogo.ShowDialog(this)!=DialogResult.OK)return;
            negocio.Anular(m.Id,texto.Text); Cargar();
        });
        Load += (_,_)=>Ejecutar(Cargar);
    }

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
    private static Label Titulo(string texto)=>new(){Text=texto,Height=30,Font=new Font("Segoe UI",11,FontStyle.Bold)};
    private static Panel Campo(string texto,Control entrada,int ancho=170)=>DistribucionAdaptable.Campo(new Label{Text=texto},entrada,ancho);
    private static NumericUpDown Dinero()=>new(){DecimalPlaces=2,Maximum=999999999,ThousandsSeparator=true,Width=160};
    private static ComboBox Lista(params string[] opciones) { var c=new ComboBox { DropDownStyle=ComboBoxStyle.DropDownList,Width=170 }; c.Items.AddRange(opciones); c.SelectedIndex=0; return c; }
    private static Button Boton(string texto)=>new(){Text=texto,Width=180,Height=36,BackColor=Color.SteelBlue,ForeColor=Color.White,FlatStyle=FlatStyle.Flat};
}
