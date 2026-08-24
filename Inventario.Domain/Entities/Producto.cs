namespace Inventario.Domain.Entities;

public class Producto
{
    public int ProductoID { get; set; }
    public string CodigoSKU { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal CostoPromedio { get; set; }
    public decimal PrecioVenta { get; set; }
    public int CategoriaID { get; set; }

    public Categoria? Categoria { get; set; }
    public ICollection<Lote> Lotes { get; set; } = new List<Lote>();
}