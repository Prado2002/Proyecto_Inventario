using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventario.Domain.Entities;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages.Categorias;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<CategoriaViewModel> CategoriasList { get; set; } = new();

    [BindProperty]
    public CategoriaFormModel CategoriaForm { get; set; } = new();

    public class CategoriaViewModel
    {
        public int CategoriaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int TotalProductos { get; set; }
    }

    public class CategoriaFormModel
    {
        public int CategoriaID { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public async Task OnGetAsync()
    {
        await CargarDatosAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var categoria = new Categoria
        {
            Nombre = CategoriaForm.Nombre,
            Descripcion = CategoriaForm.Descripcion
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        var categoria = await _context.Categorias.FindAsync(CategoriaForm.CategoriaID);
        if (categoria != null)
        {
            categoria.Nombre = CategoriaForm.Nombre;
            categoria.Descripcion = CategoriaForm.Descripcion;
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria != null)
        {
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    private async Task CargarDatosAsync()
    {
        CategoriasList = await _context.Categorias
            .Include(c => c.Productos)
            .AsNoTracking()
            .Select(c => new CategoriaViewModel
            {
                CategoriaID = c.CategoriaID,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                TotalProductos = c.Productos.Count
            })
            .ToListAsync();
    }
}