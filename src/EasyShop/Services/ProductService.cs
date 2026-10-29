using EasyShop.Data;
using EasyShop.Models;

namespace EasyShop.Services;

/// <summary>Regras de negócio do catálogo de produtos (persistido no NoSQL).</summary>
public class ProductService(IProductRepository repository)
{
    public Task<IReadOnlyList<Product>> GetProductsAsync() => repository.GetAllAsync();

    public Task<Product> AddProductAsync(string name, decimal price, string? category,
        IEnumerable<KeyValuePair<string, string>> attributes)
    {
        name = name?.Trim() ?? string.Empty;
        if (name.Length == 0)
            throw new ValidationException("Informe o nome do produto.");
        if (price <= 0)
            throw new ValidationException("O preço deve ser maior que zero.");

        var normalized = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, value) in attributes)
        {
            var k = key?.Trim() ?? string.Empty;
            var v = value?.Trim() ?? string.Empty;
            if (k.Length == 0 || v.Length == 0)
                throw new ValidationException("Atributos precisam de nome e valor.");
            normalized[k] = v;
        }

        var product = new Product
        {
            Name = name,
            Price = decimal.Round(price, 2),
            Category = string.IsNullOrWhiteSpace(category) ? null : category.Trim(),
            Attributes = new Dictionary<string, string>(normalized),
        };

        return repository.InsertAsync(product);
    }

    public Task<bool> DeleteProductAsync(int id) => repository.DeleteAsync(id);
}
