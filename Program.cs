// Permite utilizar la clase que prepara la base de datos.
using Prana.Datos;

// Permite utilizar el formulario de inicio de sesión.
using Prana.Presentacion;

namespace PranaElectronica
{
    // Contiene el punto de inicio de la aplicación.
    internal static class Program
    {
        // Indica que la aplicación utiliza un único subproceso
        // para los componentes visuales de Windows Forms.
        [STAThread]
        static void Main()
        {
            // Configura Windows Forms antes de abrir cualquier formulario.
            ApplicationConfiguration.Initialize();

            try
            {
                // Comprueba si PranaElectronica existe en LocalDB.
                // Si no existe, crea la base, las tablas y el usuario admin.
                InicializadorBaseDatos.AsegurarBaseDeDatos();
            }
            catch (Exception ex)
            {
                // Muestra el motivo si no se pudo preparar la base.
                MessageBox.Show(
                    "No se pudo preparar la base de datos local.\n\n" +
                    ex.Message,
                    "Error de inicialización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                // Cierra la aplicación porque no puede funcionar sin la base.
                return;
            }

            // Abre el login después de comprobar que la base funciona.
            Application.Run(new login());
        }
    }
}