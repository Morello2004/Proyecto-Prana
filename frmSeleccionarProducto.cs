using Prana.Entidades;
using Prana.Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace Prana.Presentacion
{
    // agrupa los datos y operaciones de button seleccionar producto
    public partial class buttonSeleccionarProducto : Form
    {
        // se crea el evento que le dice a c# que transportara un objeto varianteproducto de este frm al grid del frmstock
        public event Action<VarianteProducto, producto> OnVarianteSeleccionada;
        // permite utilizar los metodos de productos de la capa de negocio
        private readonly CN_Producto negocioProducto = new CN_Producto();
        // conserva los productos obtenidos de la base de datos
        private List<producto> productosEncontrados = new List<producto>();
        // devuelve el producto seleccionado
        public producto? ProductoSeleccionado
        {
            get; private set;
        }
        // devuelve la variante seleccionada
        public VarianteProducto? VarianteSeleccionada
        {
            get; private set;
        }

        // carga todos los productos al abrirse el formulario
        public buttonSeleccionarProducto()
        {
            InitializeComponent();
        }

        // se ejecuta cuando se abre el formulario
        private void frmSeleccionarProducto_Load(object sender, EventArgs e)
        {
            // envia un texto vacio para mostrar todos los productos
            CargarProductos(string.Empty);
        }

        // se ejecuta cuando cambia el contenido del buscador responde al cambio del texto; revisa el estado de busqueda o de seleccion antes de continuar
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        // se ejecuta cuando el usuario presiona el boton buscar atiende la pulsacion de buttonbuscarproducto y ejecuta la accion asociada el evento no significa que la operacion ya se haya guardado
        private void buttonBuscarProducto_Click(object sender, EventArgs e)
        {
            // busca utilizando el nombre o modelo escrito
            CargarProductos(textBox1.Text.Trim());
        }

        // busca los productos y carga sus variantes en la grilla
        private void CargarProductos(string textoBusqueda)
        {
            // solicita los productos encontrados a la capa de negocio los demas filtros se envian vacios porque este formulario solamente busca por nombre o modelo
            List<producto> productosEncontrados = negocioProducto.buscarProducto(textoBusqueda, null, null, string.Empty, string.Empty);
            // elimina las filas de la busqueda anterior vacia la coleccion o campo indicado revisar si es una carga temporal antes de reutilizarlo
            dgvProductos.Rows.Clear();
            // recorre todos los productos encontrados recorre cada productoencontrado de la coleccion; el bloque se aplica a un elemento por vez
            foreach (producto productoEncontrado in productosEncontrados)
            {
                // recorre todas las variantes pertenecientes al producto recorre cada variante de la coleccion; el bloque se aplica a un elemento por vez
                foreach (VarianteProducto variante in productoEncontrado.Variantes)
                {
                    // agrega una fila por cada variante el orden coincide con las columnas del datagridview anade una fila visible; el orden de los valores debe coincidir con el de las columnas
                    dgvProductos.Rows.Add(variante.IdVariante, productoEncontrado.NombreProducto, productoEncontrado.Modelo, variante.Color, variante.CondicionProducto, variante.StockFisico, variante.Costo);
                }
            }
        }

        // ejecuta la accion de button cancelar cuando el usuario pulsa el control
        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // identifica la fila y la columna pulsadas antes de ejecutar la accion de la celda
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // evita errores si hacen clic en los encabezados de columna (-1) o fuera de la grilla
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string nombreColumna = dgvProductos.Columns[e.ColumnIndex].Name;
            // comparacion insensible a mayusculas para evitar problemas con el nombre desde el disenador
            if (nombreColumna.Equals("colAgregar", StringComparison.OrdinalIgnoreCase))
            {
                CN_Producto cnProducto = new CN_Producto();
                int idVariante = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["colIdVariante"].Value);
                // obtiene el producto completo a partir de la variante
                producto? primerProducto = cnProducto.obtenerProductoPorVariante(idVariante);
                if (primerProducto != null)
                {
                    VarianteProducto? varianteSeleccionada = primerProducto.Variantes.FirstOrDefault(varianteActual => varianteActual.IdVariante == idVariante);
                    if (varianteSeleccionada != null)
                    {
                        // se manda la informacion al otro formulario
                        OnVarianteSeleccionada?.Invoke(varianteSeleccionada, primerProducto);
                    }
                }
            }
        }

        // interpreta la columna y fila pulsadas para ejecutar la accion de esa celda; los encabezados no son registros
        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // comprueba que el clic corresponda a una fila de datos y no al encabezado
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
        }
    }
}
