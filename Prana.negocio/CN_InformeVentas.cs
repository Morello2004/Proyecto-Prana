using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn informe ventas
    public class CN_InformeVentas
    {
        // conserva datos entre las operaciones de esta clase
        private readonly CD_InformeVentas datos = new();
        // resuelve obtener opciones con los datos recibidos y entrega el resultado al codigo que lo llama
        public Dictionary<string, List<string>> ObtenerOpciones() => datos.ObtenerOpciones();
        // resuelve obtener categoria marca producto con los datos recibidos y entrega el resultado al codigo que lo llama
        public (string Categoria, string Marca)? ObtenerCategoriaMarcaProducto(string nombreProducto) => datos.ObtenerCategoriaMarcaProducto(nombreProducto);
        // resuelve buscar con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<InformeVenta> Buscar(FiltroInformeVentas filtro) => datos.Buscar(filtro);
        // resuelve obtener alertas con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<AlertaPagoPendiente> ObtenerAlertas() => datos.ObtenerAlertas(DateTime.Now);
    }
}
