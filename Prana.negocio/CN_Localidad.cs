using Prana.Datos;
using Prana.Entidades;
using System;
using System.Collections.Generic;

namespace Prana.negocio;

public sealed class CN_Localidad
{
    private readonly CD_Localidad datos = new();

    // entrega las opciones activas que puede elegir el usuario
    public List<string> Obtener() => datos.Obtener();

    // entrega todas las opciones para la pantalla de administracion
    public List<Localidad> ObtenerTodas() => datos.ObtenerTodas();

    // valida permisos y normaliza el nombre antes de agregarlo
    public void Agregar(string? nombre)
    {
        Permisos.Exigir(Permisos.AdministrarLocalidades);
        datos.Guardar(null, Normalizar(nombre));
    }

    // valida permisos y normaliza ambos nombres antes de renombrar
    public void Renombrar(string nombreActual, string? nombreNuevo)
    {
        Permisos.Exigir(Permisos.AdministrarLocalidades);
        datos.Guardar(Normalizar(nombreActual), Normalizar(nombreNuevo));
    }

    // limita el cambio de estado a usuarios autorizados
    public void CambiarEstado(string? nombre, bool activa)
    {
        Permisos.Exigir(Permisos.AdministrarLocalidades);
        datos.CambiarEstado(Normalizar(nombre), activa);
    }

    // convierte el nombre a mayusculas y rechaza valores vacios
    private static string Normalizar(string? nombre)
    {
        string localidad = Direcciones.NormalizarLocalidad(nombre);
        if (localidad.Length == 0)
            throw new InvalidOperationException("Ingrese el nombre de la localidad.");
        return localidad;
    }
}
