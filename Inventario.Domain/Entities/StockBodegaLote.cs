namespace Inventario.Domain.Entities;

public class StockBodegaLote
{
    public int StockBodegaLoteID { get; set; }

    public int BodegaID { get; set; }
    public Bodega? Bodega { get; set; }

    public int LoteID { get; set; }
    public Lote? Lote { get; set; }

    public int Cantidad { get; set; }
}