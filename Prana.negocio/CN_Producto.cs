using Prana.Datos;
using Prana.Entidades;

namespace Prana.Negocio
{
    public class CN_Producto
    {
        private readonly CD_Producto datosProducto = new CD_Producto();

        // Recibe el texto ingresado en el buscador
        // Envía todos los filtros a la capa de Datos



        public bool ActualizarProductoDesdeStock(int idVariante, string modelo,string nombreProducto,string descripcion, string color,string condicion,decimal costo,decimal precio,int stockDisponible)
        {
            // Envía los datos modificados a la capa de Datos
            return datosProducto.ActualizarProductoDesdeStock( idVariante,modelo,nombreProducto,descripcion, color,condicion,costo, precio,stockDisponible);
        }



        // Solicita la desactivación de una variante
        public bool DesactivarVariante(int idVariante)
        {
            CD_Producto datosProducto = new CD_Producto();

            return datosProducto.DesactivarVariante(idVariante);
        }



        public List<producto> buscarProducto(string nombreProducto,int? idCategoria,int? idMarca,string condicionProducto, string modelo)
        {
            return datosProducto.buscarProducto( nombreProducto, idCategoria,idMarca,condicionProducto,modelo);
        }

        // Obtiene el producto completo de una variante
        public producto? obtenerProductoPorVariante(int idVariante)
        {
            return datosProducto.obtenerProductoPorVariante(idVariante);
        }

        // Registra un ingreso de stock con sus detalles y actualiza el stock de las variantes
        public bool RegistrarIngreso(int idProveedor, DateTime fechaIngreso, string remitoProveedor, List<(int idVariante, int cantidad, decimal costoUnitario)> detalles)
        {
            return datosProducto.RegistrarIngreso(idProveedor, fechaIngreso, remitoProveedor, detalles);
        }


        // Desactiva la variante seleccionada
        public bool desactivarVariante(int idVariante)
        {
            return datosProducto.desactivarVariante( idVariante);
        }




        // Valida los datos del producto antes de enviarlos a la capa de Datos
        public bool NuevoProducto(producto producto)
        {
            if (producto == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(producto.Marca))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(producto.Categoria))
            {
                return false;
            }

            if (producto.Variantes == null || producto.Variantes.Count == 0)
            {
                return false;
            }

            foreach (VarianteProducto variante in producto.Variantes)
            {
                
                if (string.IsNullOrWhiteSpace(variante.CondicionProducto))
                {
                    return false;
                }

                if (variante.Precio <= 0)
                {
                    return false;
                }
            }

            return datosProducto.nuevoProducto(producto);
        }
    }
}