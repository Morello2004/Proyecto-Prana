// permite utilizar la clase que prepara la base de datos
using Prana.Datos;
// permite utilizar el formulario de inicio de sesion
using Prana.Presentacion;
using Prana.negocio;

namespace PranaElectronica
{
    // contiene el punto de inicio de la aplicacion
    internal static class Program
    {
        // establece el modo de subproceso requerido por los controles de windows antes de iniciar la aplicacion
        [STAThread]
        static void Main()
        {
            // configura windows forms antes de abrir cualquier formulario
            ApplicationConfiguration.Initialize();
            // utiliza el servicio de caja para recuperar dias pendientes y detectar el cambio de fecha
            CN_Caja caja = new CN_Caja();
            DateTime diaProcesado;
            try
            {
                // comprueba si pranaelectronica existe en localdb si no existe, crea la base, las tablas y el usuario admin
                InicializadorBaseDatos.AsegurarBaseDeDatos();
                // recupera los cierres pendientes antes de permitir nuevas operaciones
                diaProcesado = caja.CerrarDiasAnteriores();
            }
            // atiende el error de esta operacion antes de informar o propagar el problema
            catch (Exception error)
            {
                // muestra el motivo si no se pudo preparar la base
                MessageBox.Show("No se pudo preparar la base de datos local.\n\n" + error.Message, "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // cierra la aplicacion porque no puede funcionar sin la base
                return;
            }

            // el temporizador vive durante toda la aplicacion aunque la pantalla de caja no este abierta
            using System.Windows.Forms.Timer relojCaja = new System.Windows.Forms.Timer();
            // comprueba el cambio de dia una vez por segundo sin cerrar una caja en cada tick
            relojCaja.Interval = 1000;
            // limita los reintentos cuando no se pudo completar un cierre automatico
            DateTime proximoIntento = DateTime.MinValue;
            bool errorInformado = false;
            relojCaja.Tick += (sender, e) =>
            {
                if (DateTime.Today == diaProcesado || DateTime.Now < proximoIntento)
                {
                    return;
                }
                // evita reentradas si aparece un mensaje mientras se procesa el cambio de dia
                relojCaja.Stop();
                try
                {
                    DateTime hoy = caja.CerrarDiasAnteriores();
                    // procesa cada pantalla de applicationopen formsof type<frm caja>()to array() sin cambiar el orden de la coleccion
                    foreach (frmCaja pantalla in Application.OpenForms.OfType<frmCaja>().ToArray())
                    {
                        pantalla.ActualizarCambioDeDia(diaProcesado, hoy);
                    }
                    diaProcesado = hoy;
                    errorInformado = false;
                }
                // atiende el error de esta operacion antes de informar o propagar el problema
                catch (Exception error)
                {
                    proximoIntento = DateTime.Now.AddSeconds(30);
                    if (!errorInformado)
                    {
                        errorInformado = true;
                        MessageBox.Show("No se pudo completar el cierre automático. Se volverá a intentar.\n\n" + error.Message,
                            "Cierre de caja", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                // realiza la limpieza de salida tanto si la operacion termina bien como si falla
                finally
                {
                    relojCaja.Start();
                }
            };
            relojCaja.Start();
            // abre el login despues de comprobar que la base funciona
            Application.Run(new login());
        }
    }
}
