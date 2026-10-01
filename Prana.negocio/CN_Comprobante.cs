using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio;

public class CN_Comprobante
{
    public ComprobanteVenta Obtener(int venta)
    {
        CN_Pago.Verificar(Permisos.VerPanelPrincipal);
        return new CD_Comprobante().Obtener(venta);
    }
}
