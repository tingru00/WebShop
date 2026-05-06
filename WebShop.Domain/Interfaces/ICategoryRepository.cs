using WebShop.Domain.Entities;

public interface ICategoryRepository
{
    // Hämtar alla kategorier från databasen
    Task<List<Category>> GetAllAsync();
    // Lägger till en ny kategori
    Task AddAsync(Category category);
}