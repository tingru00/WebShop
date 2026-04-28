using WebShop.Domain.Entities;

namespace WebShop.Domain.Interfaces;
// Detta interface beskriver vad vi kan göra med Product

public interface IProductRepository
{
    // Hämtar alla produkter från databasen
    Task<List<Product>> GetAllAsync();

    // Hämtar en specifik produkt baserat på Id
    Task<Product?> GetByIdAsync(int id);

    // Lägger till en ny produkt i databasen
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}