using Prana.Datos;
using Prana.Entidades;

namespace Prana.negocio;

// exige administrador incluso si se intenta operar sin utilizar el boton de stock
public class CN_OpcionStock
{
    private readonly CD_OpcionStock datos = new CD_OpcionStock();
    // bloquea la consulta para cuentas sin permiso antes de abrir una conexion
    public List<OpcionStock> Obtener(TipoOpcionStock tipo)
    {
        Permisos.Exigir(Permisos.AdministrarOpcionesStock);
        return datos.Obtener(tipo);
    }
    // valida el nombre antes de enviarlo a la transaccion de datos
    public void Guardar(TipoOpcionStock tipo, OpcionStock opcion, string nombre, int? destino, bool eliminar)
    {
        Permisos.Exigir(Permisos.AdministrarOpcionesStock);
        nombre = NormalizarTexto.Mayusculas(nombre);
        int limite = 100;
        if (tipo == TipoOpcionStock.Color)
        {
            limite = 50;
        }
        if (!eliminar && !destino.HasValue && (string.IsNullOrWhiteSpace(nombre) || nombre.Length > limite))
        {
            throw new InvalidOperationException("Ingrese un nombre de hasta " + limite + " caracteres.");
        }
        datos.Guardar(tipo, opcion, nombre, destino, eliminar, SesionActual.Usuario!.idUsuario);
    }
}
