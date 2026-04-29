using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities;

namespace WebShop.Infrastructure.Data;

//Här defineras vilka tabeller som finns
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Dessa är tabeller i databasen
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    // Extra tydlighet för databasen hur tabellerna hänger ihop
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }
}