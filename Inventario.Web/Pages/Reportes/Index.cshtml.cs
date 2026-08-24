using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventario.Infrastructure;

namespace Inventario.Web.Pages.Reportes;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public int TotalProductos { get; set; }
    public int TotalStock { get; set; }
    public decimal ValorTotalInventario { get; set; }
    public int ProductosPorVencer { get; set; }

    public List<StockPorBodegaReporte> StockPorBodega { get; set; } = new();
    public List<LoteVencimientoReporte> ProximosVencimientos { get; set; } = new();

    public class StockPorBodegaReporte
    {
        public string Bodega { get; set; } = string.Empty;
        public int CantidadTotal { get; set; }
    }

    public class LoteVencimientoReporte
    {
        public string Producto { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public DateTime FechaVencimiento { get; set; }
        public int Cantidad { get; set; }
    }

    public async Task OnGetAsync()
    {
        var stocks = await _context.StockBodegaLote
            .Include(s => s.Bodega)
            .Include(s => s.Lote)
                .ThenInclude(l => l!.Producto)
            .AsNoTracking()
            .ToListAsync();

        TotalProductos = await _context.Productos.CountAsync();
        TotalStock = stocks.Sum(s => s.Cantidad);
        ValorTotalInventario = stocks.Sum(s => s.Cantidad * (s.Lote?.Producto?.PrecioVenta ?? 0));

        var fechaLimite = DateTime.Now.AddDays(30);
        ProductosPorVencer = stocks.Count(s => s.Lote != null && s.Lote.FechaVencimiento <= fechaLimite);

        StockPorBodega = stocks
            .GroupBy(s => s.Bodega?.Nombre ?? "Sin Bodega")
            .Select(g => new StockPorBodegaReporte
            {
                Bodega = g.Key,
                CantidadTotal = g.Sum(x => x.Cantidad)
            })
            .ToList();

        ProximosVencimientos = stocks
            .Where(s => s.Lote != null && s.Lote.FechaVencimiento <= fechaLimite)
            .OrderBy(s => s.Lote!.FechaVencimiento)
            .Select(s => new LoteVencimientoReporte
            {
                Producto = s.Lote?.Producto?.Nombre ?? "Sin Producto",
                Lote = s.Lote?.NumeroLote ?? string.Empty,
                FechaVencimiento = s.Lote?.FechaVencimiento ?? DateTime.MinValue,
                Cantidad = s.Cantidad
            })
            .ToList();
    }
}