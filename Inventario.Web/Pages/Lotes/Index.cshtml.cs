using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventario.Domain.Entities;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages.Lotes;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<LoteViewModel> LotesList { get; set; } = new();
    public List<SelectListItem> ProductosDrop { get; set; } = new();

    [BindProperty]
    public LoteFormModel LoteForm { get; set; } = new();

    public class LoteViewModel
    {
        public int LoteID { get; set; }
        public string NumeroLote { get; set; } = string.Empty;
        public string ProductoNombre { get; set; } = string.Empty;
        public string CodigoSKU { get; set; } = string.Empty;
        public decimal CostoUnitarioLote { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }

    public class LoteFormModel
    {
        public int LoteID { get; set; }
        public int ProductoID { get; set; }
        public string NumeroLote { get; set; } = string.Empty;
        public decimal CostoUnitarioLote { get; set; }
        public DateTime FechaVencimiento { get; set; } = DateTime.Now.AddMonths(6);
    }

    public async Task OnGetAsync()
    {
        await CargarDatosAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var lote = new Lote
        {
            ProductoID = LoteForm.ProductoID,
            NumeroLote = LoteForm.NumeroLote,
            CostoUnitarioLote = LoteForm.CostoUnitarioLote,
            FechaVencimiento = LoteForm.FechaVencimiento
        };

        _context.Lotes.Add(lote);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var lote = await _context.Lotes.FindAsync(id);
        if (lote != null)
        {
            _context.Lotes.Remove(lote);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    private async Task CargarDatosAsync()
    {
        LotesList = await _context.Lotes
            .Include(l => l.Producto)
            .AsNoTracking()
            .Select(l => new LoteViewModel
            {
                LoteID = l.LoteID,
                NumeroLote = l.NumeroLote,
                ProductoNombre = l.Producto != null ? l.Producto.Nombre : "Sin Producto",
                CodigoSKU = l.Producto != null ? l.Producto.CodigoSKU : "-",
                CostoUnitarioLote = l.CostoUnitarioLote,
                FechaVencimiento = l.FechaVencimiento
            })
            .OrderBy(l => l.FechaVencimiento)
            .ToListAsync();

        ProductosDrop = await _context.Productos
            .AsNoTracking()
            .Select(p => new SelectListItem
            {
                Value = p.ProductoID.ToString(),
                Text = $"{p.Nombre} ({p.CodigoSKU})"
            })
            .ToListAsync();
    }
}