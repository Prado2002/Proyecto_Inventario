using System.ComponentModel.DataAnnotations;

namespace Inventario.Web.Models;

public class CrearProductoViewModel
{
    // --- DATOS DEL PRODUCTO ---
    [Required(ErrorMessage = "El código SKU es obligatorio")]
    public string CodigoSKU { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El costo es obligatorio")]
    public decimal CostoPromedio { get; set; }

    [Required(ErrorMessage = "El precio de venta es obligatorio")]
    public decimal PrecioVenta { get; set; }

    [Required(ErrorMessage = "Seleccione una categoría")]
    public int CategoriaID { get; set; }

    // --- DATOS DEL LOTE UNIFICADO ---
    [Required(ErrorMessage = "El número de lote es obligatorio")]
    public string NumeroLote { get; set; } = string.Empty;

    [Required(ErrorMessage = "La cantidad inicial es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int CantidadInicial { get; set; }

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
    [DataType(DataType.Date)]
    public DateTime FechaVencimiento { get; set; } = DateTime.Now.AddMonths(6);
}