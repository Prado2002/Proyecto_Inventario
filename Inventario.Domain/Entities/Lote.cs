namespace Inventario.Domain.Entities;

public class Lote
{
    public int LoteID { get; set; }
    public string NumeroLote { get; set; } = string.Empty;
    public int CantidadInicial { get; set; }
    public int CantidadDisponible { get; set; } // Aquí se irán restando los 30 de los 200
    public DateTime FechaVencimiento { get; set; }
    public DateTime FechaIngreso { get; set; } = DateTime.Now;

    public int ProductoID { get; set; }
    public Producto? Producto { get; set; }
}