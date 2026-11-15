using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio
{
    // coordina el registro de actividades de los usuarios
    public class CN_LogActividad
    {
        // accede a la tabla de logs mediante la capa de datos
        private readonly CD_LogActividad datosLog = new CD_LogActividad();
        // registra una accion realizada por el usuario autenticado
        public void Registrar(string accion, string entidad, int? idEntidad, string detalle)
        {
            // obtiene el usuario actualmente autenticado
            int idUsuario = SesionActual.Usuario!.idUsuario;
            // guarda la actividad en la base de datos
            datosLog.Registrar(idUsuario, accion, entidad, idEntidad, detalle);
        }

        // obtiene el historial de actividades para mostrarlo en pantalla
        public List<LogActividad> ObtenerTodos()
        {
            // solicita el historial a la capa de datos
            return datosLog.ObtenerTodos();
        }

        // obtiene actividades filtradas por fecha y cantidad maxima
        public List<LogActividad> ObtenerFiltrado(DateTime desde, DateTime hasta, int limite, int? remito = null, string accion = "", string usuario = "")
        {
            // solicita los registros filtrados a la capa de datos
            return datosLog.ObtenerFiltrado(desde, hasta, limite, remito, accion, usuario);
        }
    }
}
