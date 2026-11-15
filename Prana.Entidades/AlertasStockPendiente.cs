using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades;
// agrupa los datos y operaciones de alerta stock pendiente
public class AlertaStockPendiente
{
    // identificador de la presentacion concreta del producto; distingue color y condicion
    public int IdVariante
    {
        get; set;
    }
    // conserva producto como string para intercambiar este dato entre las capas
    public string Producto { get; set; } = "";
    // conserva pendientes como int para intercambiar este dato entre las capas
    public int Pendientes
    {
        get; set;
    }
    // conserva disponible como int para intercambiar este dato entre las capas
    public int Disponible
    {
        get; set;
    }
    // conserva faltantes como int para intercambiar este dato entre las capas
    public int Faltantes
    {
        get; set;
    }
}
