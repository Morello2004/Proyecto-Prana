using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.Entidades;

public class AlertaStockPendiente
{
    public int IdVariante { get; set; }
    public string Producto { get; set; } = "";
    public int Pendientes { get; set; }
    public int Disponible { get; set; }
    public int Faltantes { get; set; }
}