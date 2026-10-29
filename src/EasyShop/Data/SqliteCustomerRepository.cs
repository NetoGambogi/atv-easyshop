using EasyShop.Models;
using SQLite;

namespace EasyShop.Data;

/// <summary>
/// Acesso ao banco relacional (SQLite via sqlite-net-pcl). A tabela "customers"
/// é criada a partir dos atributos de mapeamento da classe <see cref="Customer"/>.
/// </summary>
public sealed class SqliteCustomerRepository : ICustomerRepository
{
    private SQLiteAsyncConnection? _database;

    private async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_database is not null)
            return _database;

        var db = new SQLiteAsyncConnection(DatabaseConstants.CustomersDbPath, DatabaseConstants.SqliteFlags);
        await db.CreateTableAsync<Customer>();
        _database = db;
        return db;
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync()
    {
        var db = await GetConnectionAsync();
        return await db.Table<Customer>().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        var db = await GetConnectionAsync();
        return await db.Table<Customer>().Where(c => c.Email == email).FirstOrDefaultAsync();
    }

    public async Task<Customer> InsertAsync(Customer customer)
    {
        var db = await GetConnectionAsync();
        await db.InsertAsync(customer);
        return customer;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var db = await GetConnectionAsync();
        return await db.DeleteAsync<Customer>(id) > 0;
    }
}
