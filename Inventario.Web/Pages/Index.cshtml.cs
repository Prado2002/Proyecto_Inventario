using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventario.Domain.Entities;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ProductoItemDto> InventarioList { get; set; } = new();

    [BindProperty]
    public ProductoItemDto ItemForm { get; set; } = new();

    public class ProductoItemDto
    {
        public int Id { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string CodigoBodega { get; set; } = string.Empty;
        public int BodegaId { get; set; }
        public string Lote { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }

    public async Task OnGetAsync()
    {
        var stocks = await _context.StockBodegaLote
            .Include(s => s.Bodega)
            .Include(s => s.Lote)
                .ThenInclude(l => l!.Producto)
            .AsNoTracking()
            .ToListAsync();

        InventarioList = stocks.Select(s => new ProductoItemDto
        {
            Id = s.StockBodegaLoteID,
            NombreProducto = s.Lote?.Producto?.Nombre ?? string.Empty,
            CodigoBodega = s.Bodega?.Nombre ?? string.Empty,
            BodegaId = s.BodegaID,
            Lote = s.Lote?.NumeroLote ?? string.Empty,
            Cantidad = s.Cantidad,
            Precio = s.Lote?.Producto?.PrecioVenta ?? 0
        }).ToList();
    }

    public async Task<IActionResult> OnPostCreateAsync()
    {
        var sku = "PROD-" + Guid.NewGuid().ToString()[..8].ToUpper();

        var producto = new Producto
        {
            Nombre = ItemForm.NombreProducto,
            CodigoSKU = sku,
            PrecioVenta = ItemForm.Precio,
            CostoPromedio = ItemForm.Precio
        };
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        var lote = new Lote
        {
            ProductoID = producto.ProductoID,
            NumeroLote = string.IsNullOrWhiteSpace(ItemForm.Lote) ? "LOTE-001" : ItemForm.Lote,
            FechaVencimiento = DateTime.Today.AddMonths(6),
            CostoUnitarioLote = ItemForm.Precio
        };
        _context.Lotes.Add(lote);
        await _context.SaveChangesAsync();

        var bodega = await _context.Bodegas.FirstOrDefaultAsync(b => b.Nombre == ItemForm.CodigoBodega);
        int bodegaId = bodega?.BodegaID ?? ItemForm.BodegaId;

        if (bodegaId == 0)
        {
            var nuevaBodega = new Bodega { Nombre = string.IsNullOrWhiteSpace(ItemForm.CodigoBodega) ? "Principal" : ItemForm.CodigoBodega };
            _context.Bodegas.Add(nuevaBodega);
            await _context.SaveChangesAsync();
            bodegaId = nuevaBodega.BodegaID;
        }

        var stock = new StockBodegaLote
        {
            BodegaID = bodegaId,
            LoteID = lote.LoteID,
            Cantidad = ItemForm.Cantidad
        };
        _context.StockBodegaLote.Add(stock);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        var stock = await _context.StockBodegaLote
            .Include(s => s.Lote)
                .ThenInclude(l => l!.Producto)
            .FirstOrDefaultAsync(s => s.StockBodegaLoteID == ItemForm.Id);

        if (stock != null)
        {
            stock.Cantidad = ItemForm.Cantidad;

            if (stock.Lote != null)
            {
                stock.Lote.NumeroLote = ItemForm.Lote;
                
                var producto = await _context.Productos.FindAsync(stock.Lote.ProductoID);
                if (producto != null)
                {
                    producto.Nombre = ItemForm.NombreProducto;
                    producto.PrecioVenta = ItemForm.Precio;
                }
            }

            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var stock = await _context.StockBodegaLote.FindAsync(id);
        if (stock != null)
        {
            _context.StockBodegaLote.Remove(stock);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}