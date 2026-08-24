using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventario.Domain.Entities;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages.Productos;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ProductoViewModel> ProductosList { get; set; } = new();
    public List<SelectListItem> CategoriasDrop { get; set; } = new();

    [BindProperty]
    public ProductoFormModel ProductoForm { get; set; } = new();

    public class ProductoViewModel
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoSKU { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public decimal CostoPromedio { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public int? CategoriaID { get; set; }
    }

    public class ProductoFormModel
    {
        public int ProductoID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CodigoSKU { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioVenta { get; set; }
        public decimal CostoPromedio { get; set; }
        public int CategoriaID { get; set; }
    }

    public async Task OnGetAsync()
    {
        await CargarDatosAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var producto = new Producto
        {
            Nombre = ProductoForm.Nombre,
            CodigoSKU = ProductoForm.CodigoSKU,
            Descripcion = ProductoForm.Descripcion,
            PrecioVenta = ProductoForm.PrecioVenta,
            CostoPromedio = ProductoForm.CostoPromedio,
            CategoriaID = ProductoForm.CategoriaID
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        var producto = await _context.Productos.FindAsync(ProductoForm.ProductoID);
        if (producto != null)
        {
            producto.Nombre = ProductoForm.Nombre;
            producto.CodigoSKU = ProductoForm.CodigoSKU;
            producto.Descripcion = ProductoForm.Descripcion;
            producto.PrecioVenta = ProductoForm.PrecioVenta;
            producto.CostoPromedio = ProductoForm.CostoPromedio;
            producto.CategoriaID = ProductoForm.CategoriaID;

            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto != null)
        {
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    private async Task CargarDatosAsync()
    {
        ProductosList = await _context.Productos
            .Include(p => p.Categoria)
            .AsNoTracking()
            .Select(p => new ProductoViewModel
            {
                ProductoID = p.ProductoID,
                Nombre = p.Nombre,
                CodigoSKU = p.CodigoSKU,
                Descripcion = p.Descripcion ?? string.Empty,
                PrecioVenta = p.PrecioVenta,
                CostoPromedio = p.CostoPromedio,
                CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : "Sin Categoría",
                CategoriaID = p.CategoriaID
            })
            .ToListAsync();

        CategoriasDrop = await _context.Categorias
            .AsNoTracking()
            .Select(c => new SelectListItem
            {
                Value = c.CategoriaID.ToString(),
                Text = c.Nombre
            })
            .ToListAsync();
    }
}