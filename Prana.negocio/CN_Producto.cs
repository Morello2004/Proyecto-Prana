using Prana.negocio;
using Prana.Datos;
using Prana.Entidades;

namespace Prana.Negocio
{
    // agrupa los datos y operaciones de cn producto
    public class CN_Producto
    {
        // conserva datos producto entre las operaciones de esta clase
        private readonly CD_Producto datosProducto = new CD_Producto();
        // ofrece los colores existentes como sugerencias sin impedir escribir uno nuevo
        public List<string> ObtenerColores()
        {
            return datosProducto.ObtenerColores();
        }
        // recibe el texto ingresado en el buscador envia todos los filtros a la capa de datos
        public bool ActualizarProductoDesdeStock(int idVariante, string modelo, string nombreProducto, string descripcion, string color, string condicion, decimal costo, decimal precio, int stockFisico)
        {
            // normaliza los textos editados antes de comprobar sus reglas
            modelo = NormalizarTexto.Mayusculas(modelo);
            nombreProducto = NormalizarTexto.Mayusculas(nombreProducto);
            descripcion = NormalizarTexto.Mayusculas(descripcion);
            color = NormalizarTexto.Mayusculas(color);
            condicion = NormalizarTexto.Mayusculas(condicion);
            // comprueba que se haya seleccionado una variante
            if (idVariante <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione una variante válida.");
            }

            // verifica los textos obligatorios y sus limites
            if (string.IsNullOrWhiteSpace(nombreProducto))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese el nombre del producto.");
            }

            if (nombreProducto.Length > 150)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El nombre del producto admite hasta 150 caracteres.");
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(modelo))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese el modelo.");
            }

            if (modelo.Length > 15)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El modelo admite hasta 15 caracteres.");
            }

            if ((descripcion?.Length ?? 0) > 500)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La descripción admite hasta 500 caracteres.");
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(color))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese el color.");
            }

            if (color.Length > 50)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El color admite hasta 50 caracteres.");
            }

            // acepta las condiciones disponibles en el formulario
            if (condicion != "PRIMERA" && condicion != "OUTLET")
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La condición debe ser Primera u Outlet.");
            }

            // controla el precio segun la capacidad de la columna sql
            if (precio <= 0 || precio > 99999999.99m) // la m indica que es decimal ya que para comparalros deben ser del mismo tipo
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El precio debe ser mayor que cero y no superar 99.999.999,99.");
            }

            if (precio != decimal.Round(precio, 2))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El precio admite hasta dos decimales.");
            }

            // el costo usa el mismo limite y precision que el precio
            if (costo < 0 || costo > 99999999.99m)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El costo debe estar entre 0 y 99.999.999,99.");
            }

            if (costo != decimal.Round(costo, 2))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El costo admite hasta dos decimales.");
            }

            // el tipo int del parametro ya limita el maximo de stock
            if (stockFisico < 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El stock físico no puede ser negativo.");
            }

            return datosProducto.ActualizarProductoDesdeStock(idVariante, modelo, nombreProducto, descripcion, color, condicion, costo, precio, stockFisico);
        }

        // solicita la desactivacion de una variante
        public bool DesactivarVariante(int idVariante)
        {
            CD_Producto datosProducto = new CD_Producto();
            return datosProducto.DesactivarVariante(idVariante);
        }

        // resuelve buscar producto con los datos recibidos y entrega el resultado al codigo que lo llama
        public List<producto> buscarProducto(string nombreProducto, int? idCategoria, int? idMarca, string condicionProducto, string modelo, bool buscarPorPrefijo = false)
        {
            return datosProducto.buscarProducto(NormalizarTexto.Mayusculas(nombreProducto), idCategoria, idMarca, NormalizarTexto.Mayusculas(condicionProducto), NormalizarTexto.Mayusculas(modelo), buscarPorPrefijo);
        }

        // obtiene el producto completo de una variante
        public producto? obtenerProductoPorVariante(int idVariante)
        {
            return datosProducto.obtenerProductoPorVariante(idVariante);
        }

        // registra un ingreso de stock con sus detalles y actualiza el stock de las variantes
        public bool RegistrarIngreso(int idProveedor, DateTime fechaIngreso, string remitoProveedor, List<(int idVariante, int cantidad, decimal costoUnitario)> detalles)
        {
            ValidarIngreso(remitoProveedor, detalles);
            if (idProveedor <= 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Seleccione un proveedor.");
            }

            return datosProducto.RegistrarIngreso(idProveedor, fechaIngreso, remitoProveedor, detalles);
        }

        // resuelve registrar ingreso con los datos recibidos y entrega el resultado al codigo que lo llama
        public bool RegistrarIngreso(string nombreProveedor, DateTime fechaIngreso, string remitoProveedor, List<(int idVariante, int cantidad, decimal costoUnitario)> detalles)
        {
            nombreProveedor = NormalizarTexto.Mayusculas(nombreProveedor);
            if (nombreProveedor.Length == 0 || nombreProveedor.Length > 100)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese un proveedor de hasta 100 caracteres.");
            }

            ValidarIngreso(remitoProveedor, detalles);
            return datosProducto.RegistrarIngreso(0, fechaIngreso, remitoProveedor, detalles, nombreProveedor);
        }

        // comprueba validar ingreso; si no se cumple la regla interrumpe con un mensaje antes de continuar
        private static void ValidarIngreso(string remito, List<(int idVariante, int cantidad, decimal costoUnitario)> detalles)
        {
            // comprueba si hay elementos para continuar con esta operacion
            if (detalles == null || detalles.Count == 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No hay productos para ingresar.");
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(remito))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese el número o código del documento del proveedor (remito).");
            }

            if (remito.Trim().Length > 50)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El documento del proveedor admite hasta 50 caracteres.");
            }

            // procesa cada detalle de detalles sin cambiar el orden de la coleccion
            foreach ((int idVariante, int cantidad, decimal costoUnitario) detalle in detalles)
            {
                if (detalle.idVariante <= 0 || detalle.cantidad <= 0 || detalle.costoUnitario < 0)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("Revise los productos, las cantidades y los costos del ingreso.");
                }

                // evita desbordes o redondeos al guardar el costo del ingreso
                if (detalle.costoUnitario > 99999999.99m)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El costo del ingreso no puede superar 99.999.999,99.");
                }

                if (detalle.costoUnitario != decimal.Round(detalle.costoUnitario, 2))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException("El costo del ingreso admite hasta dos decimales.");
                }
            }
        }

        // desactiva la variante seleccionada
        public bool desactivarVariante(int idVariante)
        {
            return datosProducto.desactivarVariante(idVariante);
        }

        // valida los datos del producto antes de enviarlos a la capa de datos
        public bool NuevoProducto(producto producto)
        {
            // valida los textos antes de enviar parametros a la base para evitar recortes
            if (producto == null)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("No se recibieron los datos del producto.");
            }

            // normaliza los datos generales del alta antes de validar las longitudes
            producto.NombreProducto = NormalizarTexto.Mayusculas(producto.NombreProducto);
            producto.Modelo = NormalizarTexto.Mayusculas(producto.Modelo);
            producto.Marca = NormalizarTexto.Mayusculas(producto.Marca);
            producto.Categoria = NormalizarTexto.Mayusculas(producto.Categoria);
            producto.Descripcion = NormalizarTexto.Mayusculas(producto.Descripcion);
            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese el nombre del producto.");
            }

            if (producto.NombreProducto.Length > 150)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El nombre del producto admite hasta 150 caracteres.");
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(producto.Marca))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese la marca del producto.");
            }

            if (producto.Marca.Length > 100)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La marca admite hasta 100 caracteres.");
            }

            // distingue un dato vacio de un valor ingresado antes de continuar
            if (string.IsNullOrWhiteSpace(producto.Categoria))
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Ingrese la categoría del producto.");
            }

            if (producto.Categoria.Length > 100)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La categoría admite hasta 100 caracteres.");
            }

            if ((producto.Descripcion?.Length ?? 0) > 500)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("La descripción admite hasta 500 caracteres.");
            }

            if ((producto.Modelo?.Length ?? 0) > 15)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("El modelo admite hasta 15 caracteres.");
            }

            // comprueba si hay elementos para continuar con esta operacion
            if (producto.Variantes == null || producto.Variantes.Count == 0)
            {
                // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                throw new InvalidOperationException("Agregue al menos una variante antes de guardar.");
            }

            // identifica la variante que tiene un dato incompatible con las columnas de sql repite el bloque con un contador; la condicion y el incremento delimitan los elementos que se procesan
            for (int indice = 0; indice < producto.Variantes.Count; indice++)
            {
                VarianteProducto variante = producto.Variantes[indice];
                string referencia = $"Variante {indice + 1}: ";
                if (variante == null)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "no se recibieron sus datos.");
                }

                // normaliza el color y la condicion de cada variante
                variante.Color = NormalizarTexto.Mayusculas(variante.Color);
                variante.CondicionProducto = NormalizarTexto.Mayusculas(variante.CondicionProducto);
                // distingue un dato vacio de un valor ingresado antes de continuar
                if (string.IsNullOrWhiteSpace(variante.Color))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "ingrese el color.");
                }

                if (variante.Color.Length > 50)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "el color admite hasta 50 caracteres.");
                }

                // distingue un dato vacio de un valor ingresado antes de continuar
                if (string.IsNullOrWhiteSpace(variante.CondicionProducto))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "seleccione la condición del producto.");
                }

                if (variante.CondicionProducto.Length > 10)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "la condición admite hasta 10 caracteres.");
                }

                if (variante.CondicionProducto != "PRIMERA" && variante.CondicionProducto != "OUTLET")
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "la condición debe ser PRIMERA u OUTLET.");
                }

                if (variante.Precio <= 0 || variante.Precio > 99999999.99m)
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "el precio debe ser mayor que cero y no superar 99.999.999,99.");
                }

                if (variante.Precio != decimal.Round(variante.Precio, 2))
                {
                    // interrumpe el flujo con una excepcion; el nivel que la atiende decide como informar o revertir la operacion
                    throw new InvalidOperationException(referencia + "el precio admite hasta dos decimales.");
                }
            }

            return datosProducto.nuevoProducto(producto);
        }
    }
}
