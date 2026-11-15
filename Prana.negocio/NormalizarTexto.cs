namespace Prana.negocio;
// agrupa los datos y operaciones de normalizar texto
public static class NormalizarTexto
{
    // conserva un formato unico antes de validar y guardar los textos de negocio
    public static string Mayusculas(string? texto)
    {
        return (texto ?? string.Empty).Trim().ToUpperInvariant();
    }
}
