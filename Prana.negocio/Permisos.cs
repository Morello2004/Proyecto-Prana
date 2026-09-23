using Prana.Entidades;

namespace Prana.negocio
{
    // Centraliza los permisos fijos del sistema.
    public static class Permisos
    {
        // Identifica el rol Administrador.
        private const int RolAdministrador = 1;

        // Identifica el rol Vendedor.
        private const int RolVendedor = 2;

        // Permite consultar el panel principal.
        public const string VerPanelPrincipal = "VER_PANEL_PRINCIPAL";

        // Permite gestionar clientes.
        public const string GestionarClientes = "GESTIONAR_CLIENTES";

        // Permite consultar productos.
        public const string ConsultarProductos = "CONSULTAR_PRODUCTOS";

        // Permite crear productos.
        public const string CrearProductos = "CREAR_PRODUCTOS";

        // Permite modificar productos.
        public const string ModificarProductos = "MODIFICAR_PRODUCTOS";

        // Permite modificar precios.
        public const string ModificarPrecios = "MODIFICAR_PRECIOS";

        // Permite consultar el stock.
        public const string ConsultarStock = "CONSULTAR_STOCK";

        // Permite gestionar el stock.
        public const string GestionarStock = "GESTIONAR_STOCK";

        // Permite registrar ingresos de stock.
        public const string RegistrarIngresoStock = "REGISTRAR_INGRESO_STOCK";

        // Permite registrar ventas.
        public const string RegistrarVentas = "REGISTRAR_VENTAS";

        // Permite consultar pagos.
        public const string ConsultarPagos = "CONSULTAR_PAGOS";

        // Permite registrar pagos.
        public const string RegistrarPagos = "REGISTRAR_PAGOS";

        // Permite consultar entregas.
        public const string ConsultarEntregas = "CONSULTAR_ENTREGAS";

        // Permite gestionar entregas.
        public const string GestionarEntregas = "GESTIONAR_ENTREGAS";

        // Permite consultar alertas.
        public const string ConsultarAlertas = "CONSULTAR_ALERTAS";

        // Permite modificar medios de pago.
        public const string ModificarMediosPago = "MODIFICAR_MEDIOS_PAGO";

        // Permite gestionar usuarios.
        public const string GestionarUsuarios = "GESTIONAR_USUARIOS";

        // Permite consultar el historial de actividades.
        public const string ConsultarHistorial = "CONSULTAR_HISTORIAL";

        // Comprueba si el usuario actual tiene el permiso solicitado.
        // Comprueba los permisos del rol del usuario actual.
        public static bool TienePermiso(string permiso)
        {
            // Obtiene el usuario guardado en la sesión.
            usuario usuarioActual = SesionActual.Usuario!;

            // El Administrador tiene acceso completo.
            if (usuarioActual.IdRolUsuario == RolAdministrador)
            {
                return true;
            }

            // Comprueba los permisos específicos del Vendedor.
            if (usuarioActual.IdRolUsuario == RolVendedor)
            {
                return permiso == VerPanelPrincipal
                    || permiso == GestionarClientes
                    || permiso == ConsultarProductos
                    || permiso == ConsultarStock
                    || permiso == RegistrarVentas
                    || permiso == ConsultarPagos
                    || permiso == RegistrarPagos
                    || permiso == ConsultarEntregas
                    || permiso == ConsultarAlertas;
            }

            // Deniega roles no definidos.
            return false;
        }
    }
}
