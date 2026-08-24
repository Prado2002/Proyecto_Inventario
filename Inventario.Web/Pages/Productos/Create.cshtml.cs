using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Inventario.Domain.Entities;
using Inventario.Web.Models;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages.Productos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CrearProductoViewModel Input { get; set; } = new();

        public SelectList Categorias { get; set; } = null!;

        public void OnGet()
        {
            Categorias = new SelectList(_context.Categorias, "CategoriaID", "Nombre");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categorias = new SelectList(_context.Categorias, "CategoriaID", "Nombre");
                return Page();
            }

            var nuevoProducto = new Producto
            {
                CodigoSKU = Input.CodigoSKU,
                Nombre = Input.Nombre,
                Descripcion = Input.Descripcion,
                CostoPromedio = Input.CostoPromedio,
                PrecioVenta = Input.PrecioVenta,
                CategoriaID = Input.CategoriaID,
                Lotes = new List<Lote>
                {
                    new Lote
                    {
                        NumeroLote = Input.NumeroLote,
                        CantidadInicial = Input.CantidadInicial,
                        CantidadDisponible = Input.CantidadInicial,
                        FechaVencimiento = Input.FechaVencimiento,
                        FechaIngreso = DateTime.Now
                    }
                }
            };

            _context.Productos.Add(nuevoProducto);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}