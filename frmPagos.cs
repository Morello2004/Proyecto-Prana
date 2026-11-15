// guia del archivo: aqui se cobra una venta ya existente; no se crea otra venta importe a cancelar es base sin el recargo del nuevo cobro mostrarcobro calcula el adicional segun medio y cuotas cambiar medio o cuotas solo modifica la cotizacion; registrar confirma el pago mediante cn_pago el saldo futuro resta base cancelada, no todo el dinero recibido con recargo la garantia se consulta otra vez despues del cobro para incluir todos los pagos y su saldo vigente
using Prana.Entidades;
using Prana.negocio;
using System.Globalization;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de frm pagos
    public partial class frmPagos : Form
    {
        // conserva negocio entre las operaciones de esta clase
        private readonly CN_Pago negocio = new();
        // conserva cargando entre las operaciones de esta clase
        private bool cargando;
        // conserva btn garantia entre las operaciones de esta clase
        private Button btnGarantia;
        // dato seleccionada de tipo venta? se obtiene al consultar la propiedad mediante la expresion siguiente
        private venta? Seleccionada => dgvVentas.CurrentRow?.DataBoundItem as venta;

        // prepara frm pagos antes de que se utilice la instancia
        public frmPagos()
        {
            InitializeComponent();
            ConfigurarGrillas();
            lblImporte.Text = "Importe a cancelar (sin recargo)";
            cboCuotas.SelectedIndex = 0;
            ActualizarCuotas();
        }

        // define las columnas visibles para no mostrar todas las propiedades de las entidades
        private void ConfigurarGrillas()
        {
            dgvVentas.AutoGenerateColumns = false;
            dgvVentas.Columns.AddRange(new DataGridViewTextBoxColumn
            {
                Name = "Venta",
                HeaderText = "Venta",
                DataPropertyName = nameof(venta.IdVenta),
                FillWeight = 55

            }, new DataGridViewTextBoxColumn
            {
                Name = "Fecha",
                HeaderText = "Fecha",
                DataPropertyName = nameof(venta.Fecha),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }

            }, new DataGridViewTextBoxColumn
            {
                Name = "Cliente",
                HeaderText = "Cliente",
                DataPropertyName = nameof(venta.NombreCompletoCliente),
                FillWeight = 180

            }, new DataGridViewTextBoxColumn
            {
                Name = "Total",
                HeaderText = "Total",
                DataPropertyName = nameof(venta.Total),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }

            }, new DataGridViewTextBoxColumn
            {
                Name = "Pagado",
                HeaderText = "Pagado",
                DataPropertyName = nameof(venta.Pagado),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }

            }, new DataGridViewTextBoxColumn
            {
                Name = "Saldo",
                HeaderText = "Saldo pendiente",
                DataPropertyName = nameof(venta.SaldoPendiente),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }

            });
            dgvPagos.AutoGenerateColumns = false;
            dgvPagos.Columns.AddRange(new DataGridViewTextBoxColumn
            {
                Name = "Fecha",
                HeaderText = "Fecha",
                DataPropertyName = nameof(Pago.Fecha),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }

            }, new DataGridViewTextBoxColumn
            {
                Name = "Medio",
                HeaderText = "Medio de pago",
                DataPropertyName = nameof(Pago.Medio)

            }, new DataGridViewTextBoxColumn
            {
                Name = "Cuotas",
                HeaderText = "Cuotas",
                DataPropertyName = nameof(Pago.Cuotas),
                FillWeight = 50

            }, new DataGridViewTextBoxColumn
            {
                Name = "Base",
                HeaderText = "Aplicado a la venta",
                DataPropertyName = nameof(Pago.ImporteBase),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }

            }, new DataGridViewTextBoxColumn
            {
                Name = "Recargo",
                HeaderText = "Recargo",
                DataPropertyName = nameof(Pago.Recargo),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }

            }, new DataGridViewTextBoxColumn
            {
                Name = "Cobrado",
                HeaderText = "Total cobrado",
                DataPropertyName = nameof(Pago.Cobrado),
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }

            });
        }

        // resuelve buscar con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Buscar()
        {
            try
            {
                cargando = true;
                cboMedio.DisplayMember = nameof(MedioPago.NombreMedioPago);
                // orden de presentacion: efectivo primero y seleccionado al cargar cbomediodatasource: lista u objeto del que el control obtiene los registros que presenta
                cboMedio.DataSource = new CN_MedioPago().ObtenerTodos().OrderBy(medioPago => !medioPago.NombreMedioPago.Trim().Equals("Efectivo", StringComparison.OrdinalIgnoreCase)).ThenBy(m => m.NombreMedioPago).ToList();
                // comprueba si hay elementos para continuar con esta operacion
                if (cboMedio.Items.Count > 0)
                {
                    cboMedio.SelectedIndex = 0;
                }

                ActualizarCuotas();
                List<venta> ventas = negocio.Buscar(txtBuscar.Text, chkPendientes.Checked);
                dgvVentas.DataSource = ventas;
                // comprueba si hay elementos para continuar con esta operacion
                if (ventas.Count == 0)
                {
                    lblVentas.Text = "No se encontraron ventas";
                }
                else
                {
                    lblVentas.Text = $"Ventas · {ventas.Count} resultados";
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                dgvVentas.DataSource = null;
                Error(error);
            }
            // realiza la limpieza de salida tanto si la operacion termina bien como si falla
            finally
            {
                cargando = false;
                SeleccionarVenta();
            }
        }

        // resuelve seleccionar venta con los datos recibidos y entrega el resultado al codigo que lo llama
        private void SeleccionarVenta()
        {
            if (cargando)
            {
                return;
            }

            dgvPagos.DataSource = null;
            nudImporte.Value = 0;
            venta venta = Seleccionada;
            btnGarantia.Enabled = venta != null;
            if (venta == null)
            {
                lblSeleccion.Text = "Seleccione una venta para registrar un pago";
            }
            else
            {
                lblSeleccion.Text = $"Venta #{venta.IdVenta} · {venta.NombreCompletoCliente} · Saldo: {venta.SaldoPendiente.ToString("C2", CultureInfo.GetCultureInfo("es-AR"))}";
            }
            lblPagos.Text = "Pagos registrados de la venta seleccionada";
            if (venta != null)
            {
                try
                {
                    List<Pago> pagos = negocio.Listar(venta.IdVenta);
                    dgvPagos.DataSource = pagos;
                    lblPagos.Text = $"Pagos registrados · {pagos.Count}";
                    nudImporte.Value = Math.Min(nudImporte.Maximum, venta.SaldoPendiente);
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch (Exception error)
                {
                    Error(error);
                }
            }

            MostrarCobro();
        }

        // oculta las cuotas fuera de credito y evita conservar cuotas de una seleccion anterior
        private void ActualizarCuotas()
        {
            bool credito = cboMedio.SelectedItem is MedioPago medio && medio.TipoCaja == "Crédito";
            lblCuotas.Visible = cboCuotas.Visible = credito;
            cboCuotas.Enabled = credito;
            if (!credito)
            {
                cboCuotas.SelectedIndex = 0;
            }
        }

        // cotiza el importe a cancelar segun medio y cuotas y muestra recargo, total a cobrar y saldo; no confirma el pago
        private void MostrarCobro()
        {
            btnRegistrar.Enabled = false;
            lblCobro.Text = "Ingrese el importe y seleccione el medio de pago.";
            if (cargando || Seleccionada == null || cboMedio.SelectedItem is not MedioPago medio || cboCuotas.SelectedItem is not int cuotas || nudImporte.Value <= 0)
            {
                return;
            }

            try
            {
                // el calculo del recargo pertenece a negocio; el formulario solo presenta la cotizacion
                Pago cobro = negocio.Preparar(Seleccionada.IdVenta, medio, cuotas, nudImporte.Value);
                decimal saldoRestante = Seleccionada.SaldoPendiente - cobro.ImporteBase;
                string valorText;
                if (saldoRestante >= 0)
                {
                    valorText = $"Saldo pendiente después de registrar: {saldoRestante:C2} (sin recargo de futuros cobros).";
                }
                else
                {
                    valorText = "El importe a cancelar supera el saldo pendiente.";
                }
                lblCobro.Text = $"Importe a cancelar: {cobro.ImporteBase:C2}    |    Recargo ({cobro.Porcentaje:0.##}%): {cobro.Recargo:C2}\n" + $"Total a cobrar con {medio.NombreMedioPago}: {cobro.Cobrado:C2}\n" + (valorText);
                btnRegistrar.Enabled = nudImporte.Value <= Seleccionada.SaldoPendiente && SesionActual.Usuario != null && Permisos.TienePermiso(Permisos.RegistrarPagos);
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                lblCobro.Text = error.Message;
            }
        }

        // resuelve registrar con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Registrar(object? sender, EventArgs e)
        {
            if (Seleccionada == null || cboMedio.SelectedItem is not MedioPago medio || cboCuotas.SelectedItem is not int cuotas)
            {
                return;
            }

            try
            {
                btnRegistrar.Enabled = false;
                int numeroVenta = Seleccionada.IdVenta;
                negocio.Registrar(numeroVenta, medio, cuotas, nudImporte.Value, txtCuentaDestino.Text);
                txtCuentaDestino.Clear();
                Buscar();
                if (MessageBox.Show(this, "Pago registrado correctamente. ¿Desea emitir la garantía con el saldo actualizado?", "Pagos", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    EmitirGarantia(numeroVenta);
                }
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                Error(error);
                MostrarCobro();
            }
        }

        // resuelve error con los datos recibidos y entrega el resultado al codigo que lo llama
        private void Error(Exception ex) => MessageBox.Show(this, ex.Message, "Pagos", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // ejecuta la accion de btn garantia cuando el usuario pulsa el control
        private void btnGarantia_Click(object? sender, EventArgs e)
        {
            if (Seleccionada is venta seleccionada)
            {
                EmitirGarantia(seleccionada.IdVenta);
            }
        }

        // consulta los datos actuales de la venta y sus cobros y abre una garantia actualizada, sin registrar nuevamente el pago
        private void EmitirGarantia(int numeroVenta)
        {
            try
            {
                // consulta nuevamente para incluir todos los cobros y el saldo vigente
                venta documento = new CN_Comprobante().Obtener(numeroVenta);
                string carpeta = DocumentosPrana.GuardarVenta(documento);
                DocumentosPrana.Abrir(Path.Combine(carpeta, $"Garantia-{numeroVenta}.html"));
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                MessageBox.Show(this, $"No se pudo abrir la garantía de la venta {numeroVenta}: {error.Message}\n" + "Si acaba de registrar un pago, no lo repita. Puede emitir la garantía nuevamente desde esta pantalla.", "Garantía", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ejecuta la accion de btn buscar cuando el usuario pulsa el control
        private void btnBuscar_Click(object? sender, EventArgs e)
        {
            Buscar();
        }

        // interpreta la tecla pulsada y ejecuta la accion prevista por el formulario
        private void txtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Buscar();
            }
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void dgvVentas_SelectionChanged(object? sender, EventArgs e)
        {
            SeleccionarVenta();
        }

        // resuelve nud importe value changed con los datos recibidos y entrega el resultado al codigo que lo llama
        private void nudImporte_ValueChanged(object? sender, EventArgs e)
        {
            MostrarCobro();
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboMedio_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ActualizarCuotas();
            MostrarCobro();
        }

        // actualiza los controles relacionados cuando cambia la seleccion puede dispararse tambien al cargar los datos
        private void cboCuotas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            MostrarCobro();
        }

        // prepara la informacion inicial cuando se carga la ventana
        private void frmPagos_Load(object? sender, EventArgs e) => Buscar();
    }
}
