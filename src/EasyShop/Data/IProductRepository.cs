using EasyShop.Models;

namespace EasyShop.Data;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync();
    Task<Product> InsertAsync(Product product);
    Task<bool> DeleteAsync(int id);
}
