using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio
{
    // Autoriza y coordina la generacion de datos de prueba.
    public class CN_DatosPrueba
    {
        // Accede a la capa de datos de los lotes de prueba.
        private readonly CD_DatosPrueba datos = new CD_DatosPrueba();

        // Obtiene el lote activo para actualizar las acciones disponibles.
        public LoteDatosPrueba? ObtenerActivo()
        {
            ExigirAdministrador();
            return datos.ObtenerActivo();
        }

        // Crea un lote completo para probar los principales flujos del sistema.
        public LoteDatosPrueba Cargar()
        {
            ExigirAdministrador();
            return datos.Cargar(SesionActual.Usuario!.idUsuario);
        }

        // Elimina el lote activo y conserva sus registros de auditoria.
        public void EliminarActivo()
        {
            ExigirAdministrador();
            datos.EliminarActivo(SesionActual.Usuario!.idUsuario);
        }

        // Limita estas operaciones a un administrador con sesion vigente.
        private static void ExigirAdministrador()
        {
            CN_Pago.Verificar(Permisos.GestionarUsuarios);
        }
    }
}
