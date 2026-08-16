using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    public class CN_Marca
    {
        // Permite acceder a los métodos de la capa de Datos
        private readonly CD_Marca cdMarca = new CD_Marca();

        // Solicita todas las marcas registradas
        public List<Marca> obtenerMarcas()
        {
            return cdMarca.obtenerMarcas();
        }
    }
}
