namespace Prana.Entidades;

// limita los tipos administrables a las opciones utilizadas por stock
public enum TipoOpcionStock { Proveedor, Categoria, Marca, Color }

// conserva la opcion y la cantidad de registros que la utilizan
public class OpcionStock
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Usos { get; set; }
}
