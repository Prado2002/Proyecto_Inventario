namespace Inventario.Domain.Entities;

public class Lote
{
    public int LoteID { get; set; }
    public string NumeroLote { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public decimal CostoUnitarioLote { get; set; }

    public int ProductoID { get; set; }
    public Producto? Producto { get; set; }
}