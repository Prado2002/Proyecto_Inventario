using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages.Facturacion;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ProductoDto> ProductosDisponibles { get; set; } = new();

    public class ProductoDto
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
    }

    public async Task OnGetAsync()
    {
        ProductosDisponibles = await _context.Productos
            .AsNoTracking()
            .Select(p => new ProductoDto
            {
                ProductoID = p.ProductoID,
                Nombre = p.Nombre,
                PrecioVenta = p.PrecioVenta
            })
            .ToListAsync();
    }
}