using Prana.Entidades;

namespace Prana.negocio
{
    // centraliza los permisos fijos del sistema
    public static class Permisos
    {
        // identifica el rol administrador
        private const int RolAdministrador = 1;
        // identifica el rol vendedor
        private const int RolVendedor = 2;
        // permite consultar el panel principal
        public const string VerPanelPrincipal = "VER_PANEL_PRINCIPAL";
        // permite gestionar clientes
        public const string GestionarClientes = "GESTIONAR_CLIENTES";
        // solo el administrador puede dar de baja clientes
        public const string DarDeBajaClientes = "DAR_DE_BAJA_CLIENTES";
        // permite consultar productos
        public const string ConsultarProductos = "CONSULTAR_PRODUCTOS";
        // permite consultar el stock
        public const string ConsultarStock = "CONSULTAR_STOCK";
        // permite registrar ventas
        public const string RegistrarVentas = "REGISTRAR_VENTAS";
        // permite consultar pagos
        public const string ConsultarPagos = "CONSULTAR_PAGOS";
        // permite registrar pagos
        public const string RegistrarPagos = "REGISTRAR_PAGOS";
        // permite consultar entregas
        public const string ConsultarEntregas = "CONSULTAR_ENTREGAS";
        // permite al administrador y al vendedor preparar y confirmar entregas
        public const string GestionarEntregas = "GESTIONAR_ENTREGAS";
        // permite consultar alertas
        public const string ConsultarAlertas = "CONSULTAR_ALERTAS";
        // permite modificar medios de pago
        public const string ModificarMediosPago = "MODIFICAR_MEDIOS_PAGO";
        // permite gestionar usuarios
        public const string GestionarUsuarios = "GESTIONAR_USUARIOS";
        // permite consultar el historial de actividades
        public const string ConsultarHistorial = "CONSULTAR_HISTORIAL";
        // conserva consultar informes entre las operaciones de esta clase
        public const string ConsultarInformes = "CONSULTAR_INFORMES";
        // solo el administrador puede anular ingresos y gastos manuales de caja
        public const string AnularMovimientosCaja = "ANULAR_MOVIMIENTOS_CAJA";
        // solo el administrador puede corregir unificar o eliminar las opciones compartidas de stock
        public const string AdministrarOpcionesStock = "ADMINISTRAR_OPCIONES_STOCK";
        public const string AdministrarLocalidades = "ADMINISTRAR_LOCALIDADES";
        // exige una sesion iniciada y el permiso necesario antes de continuar comprueba exigir; si no se cumple la regla interrumpe con un mensaje antes de continuar
        public static void Exigir(string permiso)
        {
            // comprueba la autorizacion de la cuenta antes de permitir esta accion
            if (SesionActual.Usuario == null || !TienePermiso(permiso))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No tiene permisos para esta operación.");
            }
        }

        // comprueba si el usuario actual tiene el permiso solicitado comprueba los permisos del rol del usuario actual
        public static bool TienePermiso(string permiso)
        {
            // obtiene el usuario guardado en la sesion
            usuario usuarioActual = SesionActual.Usuario!;
            // el administrador tiene acceso completo
            if (usuarioActual.IdRolUsuario == RolAdministrador)
            {
                return true;
            }

            // comprueba los permisos especificos del vendedor
            if (usuarioActual.IdRolUsuario == RolVendedor)
            {
                return permiso == VerPanelPrincipal || permiso == GestionarClientes || permiso == ConsultarProductos || permiso == ConsultarStock || permiso == RegistrarVentas || permiso == ConsultarPagos || permiso == RegistrarPagos || permiso == ConsultarEntregas || permiso == GestionarEntregas || permiso == ConsultarAlertas;
            }

            // deniega roles no definidos
            return false;
        }
    }
}
