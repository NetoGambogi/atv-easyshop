using System.Text.RegularExpressions;
using EasyShop.Data;
using EasyShop.Models;

namespace EasyShop.Services;

/// <summary>Regras de negócio do cadastro de clientes (persistido no SQL).</summary>
public partial class CustomerService(ICustomerRepository repository)
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    public Task<IReadOnlyList<Customer>> GetCustomersAsync() => repository.GetAllAsync();

    public async Task<Customer> AddCustomerAsync(string fullName, string email)
    {
        fullName = WhitespaceRegex().Replace(fullName?.Trim() ?? string.Empty, " ");
        email = email?.Trim().ToLowerInvariant() ?? string.Empty;

        if (fullName.Split(' ').Length < 2)
            throw new ValidationException("Informe o nome completo (nome e sobrenome).");
        if (fullName.Length > 150)
            throw new ValidationException("O nome deve ter no máximo 150 caracteres.");
        if (email.Length > 150 || !EmailRegex().IsMatch(email))
            throw new ValidationException("Informe um e-mail válido.");
        if (await repository.GetByEmailAsync(email) is not null)
            throw new ValidationException("Já existe um cliente cadastrado com este e-mail.");

        return await repository.InsertAsync(new Customer { Name = fullName, Email = email });
    }

    public Task<bool> DeleteCustomerAsync(int id) => repository.DeleteAsync(id);
}
