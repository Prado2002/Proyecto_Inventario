namespace Inventario.Domain.Entities;

public class Bodega
{
    public int BodegaID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ubicacion { get; set; } = string.Empty;

    public ICollection<StockBodegaLote> Stocks { get; set; } = new List<StockBodegaLote>();
}