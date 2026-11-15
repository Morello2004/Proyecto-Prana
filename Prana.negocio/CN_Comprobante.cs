using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio;
// agrupa los datos y operaciones de cn comprobante
public class CN_Comprobante
{
    // resuelve obtener con los datos recibidos y entrega el resultado al codigo que lo llama
    public venta Obtener(int idVenta)
    {
        Permisos.Exigir(Permisos.VerPanelPrincipal);
        return new CD_Comprobante().Obtener(idVenta);
    }
}
