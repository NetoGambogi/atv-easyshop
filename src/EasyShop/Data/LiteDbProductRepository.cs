using EasyShop.Models;
using LiteDB;

namespace EasyShop.Data;

/// <summary>
/// Acesso ao banco NoSQL (LiteDB). Os produtos são documentos BSON na coleção "products".
/// </summary>
public sealed class LiteDbProductRepository : IProductRepository, IDisposable
{
    private const string CollectionName = "products";

    private readonly LiteDatabase _database;
    private readonly ILiteCollection<Product> _collection;

    public LiteDbProductRepository()
    {
        _database = new LiteDatabase(DatabaseConstants.ProductsDbPath);
        _collection = _database.GetCollection<Product>(CollectionName);
        _collection.EnsureIndex(p => p.Name);
        _collection.EnsureIndex(p => p.Category);
    }

    public Task<IReadOnlyList<Product>> GetAllAsync() =>
        Task.Run<IReadOnlyList<Product>>(() =>
            _collection.Query().OrderByDescending(p => p.CreatedAt).ToList());

    public Task<Product> InsertAsync(Product product) =>
        Task.Run(() =>
        {
            _collection.Insert(product);
            return product;
        });

    public Task<bool> DeleteAsync(int id) =>
        Task.Run(() => _collection.Delete(id));

    public void Dispose() => _database.Dispose();
}
