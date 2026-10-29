using EasyShop.Models;

namespace EasyShop.Data;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync();
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer> InsertAsync(Customer customer);
    Task<bool> DeleteAsync(int id);
}
