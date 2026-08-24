using Microsoft.EntityFrameworkCore;
using Inventario.Domain.Entities;

namespace Inventario.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Tablas de la base de datos
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Bodega> Bodegas { get; set; }
    public DbSet<Lote> Lotes { get; set; }
    public DbSet<StockBodegaLote> StockBodegaLote { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Claves únicas
        modelBuilder.Entity<Producto>()
            .HasIndex(p => p.CodigoSKU)
            .IsUnique();

        modelBuilder.Entity<Lote>()
            .HasIndex(l => new { l.ProductoID, l.NumeroLote })
            .IsUnique();

        modelBuilder.Entity<StockBodegaLote>()
            .HasIndex(s => new { s.BodegaID, s.LoteID })
            .IsUnique();

        // Precisión para decimales de moneda
        modelBuilder.Entity<Producto>()
            .Property(p => p.CostoPromedio)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Producto>()
            .Property(p => p.PrecioVenta)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Lote>()
            .Property(l => l.CostoUnitarioLote)
            .HasPrecision(18, 4);
    }
}