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
    public partial class buttonSeleccionarProducto : Form


    {

        // SE CREA EL EVENTO que le dice a C# que transportará un objeto VarianteProducto de este frm al grid del frmstock

        public event Action<VarianteProducto, producto> OnVarianteSeleccionada;


        // Permite utilizar los métodos de productos de la capa de Negocio.
        private readonly CN_Producto negocioProducto = new CN_Producto();

        // Conserva los productos obtenidos de la base de datos.
        private List<producto> productosEncontrados = new List<producto>();

        // Devuelve el producto seleccionado.
        public producto? ProductoSeleccionado { get; private set; }

        // Devuelve la variante seleccionada.
        public VarianteProducto? VarianteSeleccionada { get; private set; }

        // Carga todos los productos al abrirse el formulario
        public buttonSeleccionarProducto()
        {
            InitializeComponent();

            CargarProductos(string.Empty);
        }

        // Se ejecuta cuando se abre el formulario.
        private void frmSeleccionarProducto_Load(object sender, EventArgs e)
        {
            // Envía un texto vacío para mostrar todos los productos.
            CargarProductos(string.Empty);

        }

        // Se ejecuta cuando cambia el contenido del buscador.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        // Se ejecuta cuando el usuario presiona el botón Buscar.
        private void buttonBuscarProducto_Click(object sender, EventArgs e)
        {
            // Busca utilizando el nombre o modelo escrito.
            CargarProductos(textBox1.Text.Trim());
        }



        // Busca los productos y carga sus variantes en la grilla.
        private void CargarProductos(string textoBusqueda)
        {
            // Solicita los productos encontrados a la capa de Negocio
            // Los demás filtros se envían vacíos porque este formulario solamente busca por nombre o modelo.
            List<producto> productosEncontrados = negocioProducto.buscarProducto(textoBusqueda, null,null,string.Empty,string.Empty);

            // Elimina las filas de la búsqueda anterior.
            dgvProductos.Rows.Clear();

            // Recorre todos los productos encontrados.
            foreach (producto productoEncontrado in productosEncontrados)
            {
                // Recorre todas las variantes pertenecientes al producto.
                foreach (VarianteProducto variante in productoEncontrado.Variantes)
                {
                    // Agrega una fila por cada variante.
                    // El orden coincide con las columnas del DataGridView.
                    dgvProductos.Rows.Add(
                        variante.IdVariante,
                        productoEncontrado.NombreProducto,
                        productoEncontrado.Modelo,
                        variante.Color,
                        variante.CondicionProducto,
                        variante.StockFisico,
                        variante.Costo
                    );
                }
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; this.Close();
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evita errores si hacen clic en los encabezados de columna (-1) o fuera de la grilla
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

           

            string nombreColumna = dgvProductos.Columns[e.ColumnIndex].Name;

            // Comparación insensible a mayúsculas para evitar problemas con el nombre desde el diseñador
            if (nombreColumna.Equals("colAgregar", StringComparison.OrdinalIgnoreCase))
            {
                CN_Producto cnProducto = new CN_Producto();

                int idVariante = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["colIdVariante"].Value);
                // Obtiene el producto completo a partir de la variante 
                producto? primerProducto = cnProducto.obtenerProductoPorVariante(idVariante);

                if (primerProducto != null)
                {
                    VarianteProducto? varianteSeleccionada = primerProducto.Variantes.FirstOrDefault(v => v.IdVariante == idVariante);

                    if (varianteSeleccionada != null)
                    {
                        // se manda la información al otro formulario
                        OnVarianteSeleccionada?.Invoke(varianteSeleccionada, primerProducto);
                    }
                }
            }
        }



        private void dgvProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
           

           
        }
    }
}