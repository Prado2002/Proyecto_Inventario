using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventario.Domain.Entities;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages.Bodegas;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Bodega> BodegasList { get; set; } = new();

    [BindProperty]
    public Bodega BodegaForm { get; set; } = new();

    public async Task OnGetAsync()
    {
        BodegasList = await _context.Bodegas.ToListAsync();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
        {
            BodegasList = await _context.Bodegas.ToListAsync();
            return Page();
        }

        _context.Bodegas.Add(BodegaForm);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _context.Bodegas.FindAsync(id);
        if (item != null)
        {
            _context.Bodegas.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}