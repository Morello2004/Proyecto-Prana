using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio
{
    // Coordina el registro de actividades de los usuarios.
    public class CN_LogActividad
    {
        // Accede a la tabla de logs mediante la capa de datos.
        private readonly CD_LogActividad datosLog = new CD_LogActividad();

        // Registra una acción realizada por el usuario autenticado.
        public void Registrar(string accion, string entidad, int? idEntidad, string detalle)
        {
            // Obtiene el usuario actualmente autenticado.
            int idUsuario = SesionActual.Usuario!.idUsuario;

            // Guarda la actividad en la base de datos.
            datosLog.Registrar(idUsuario, accion, entidad, idEntidad, detalle);
        }

        // Obtiene el historial de actividades para mostrarlo en pantalla.
        public List<LogActividad> ObtenerTodos()
        {
            // Solicita el historial a la capa de datos.
            return datosLog.ObtenerTodos();
        }

        // Obtiene actividades filtradas por fecha y cantidad máxima.
        public List<LogActividad> ObtenerFiltrado(DateTime desde, DateTime hasta, int limite)
        {
            // Solicita los registros filtrados a la capa de datos.
            return datosLog.ObtenerFiltrado(desde, hasta, limite);
        }
    }
}
