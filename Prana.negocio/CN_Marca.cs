using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.negocio
{
    // agrupa los datos y operaciones de cn marca
    public class CN_Marca
    {
        // permite acceder a los metodos de la capa de datos
        private readonly CD_Marca cdMarca = new CD_Marca();
        // solicita todas las marcas registradas
        public List<Marca> obtenerMarcas()
        {
            return cdMarca.obtenerMarcas();
        }
    }
}
