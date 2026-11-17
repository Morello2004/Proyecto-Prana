using System;
using System.Text.RegularExpressions;

namespace Prana.negocio;

public static class Direcciones
{
    public static string NormalizarLocalidad(string? valor)
    {
        string localidad = NormalizarTexto.Mayusculas(valor ?? string.Empty);
        if (localidad.Length > 25)
            throw new InvalidOperationException("La localidad admite hasta 25 caracteres.");
        return localidad;
    }

    public static string ValidarCalleNumero(string? valor, bool obligatoria)
    {
        string direccion = NormalizarTexto.Mayusculas(valor ?? string.Empty);
        if (direccion.Length == 0 && !obligatoria) return direccion;
        Match partes = Regex.Match(direccion, @"\A(.+)\s+(?:[0-9]+|S/N)\z");
        if (direccion.Length > 200 || !partes.Success || !Regex.IsMatch(partes.Groups[1].Value, @"\p{L}"))
            throw new InvalidOperationException("Ingrese calle y número, o S/N si no tiene numeración. Ejemplos: SAN MARTÍN 123 / SAN MARTÍN S/N (máximo 200 caracteres).");
        return direccion;
    }
}
