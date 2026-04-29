using Microsoft.EntityFrameworkCore;
using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;
using WebShop.Infrastructure.Data;

namespace WebShop.Infrastructure.Repositories;

// Här bestämms hur data sparas/hämtas

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    // Hämtar alla produkter, inkluderar category(relationen)
    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .ToListAsync();
    }

    // Hämtar en produkt baserat på id
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Lägger till en produkt i databasen
    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);

        // Sparar till databasen
        await _context.SaveChangesAsync();
    }

    // Uppdaterar produkt
    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    // Tar bort produkt
    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}