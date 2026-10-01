using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio
{
    public class CN_InformeVentas
    {
        private readonly CD_InformeVentas datos = new();

        public Dictionary<string, List<string>> ObtenerOpciones() => datos.ObtenerOpciones();

        public List<InformeVenta> Buscar(FiltroInformeVentas filtro) => datos.Buscar(filtro);

        public List<AlertaPagoPendiente> ObtenerAlertas() => datos.ObtenerAlertas(DateTime.Now);
    }
}
