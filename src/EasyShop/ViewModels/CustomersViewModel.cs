using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyShop.Models;
using EasyShop.Services;

namespace EasyShop.ViewModels;

public partial class CustomersViewModel(CustomerService customerService) : ObservableObject
{
    public ObservableCollection<Customer> Customers { get; } = new();

    [ObservableProperty]
    private string _fullName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    [RelayCommand]
    private async Task LoadAsync()
    {
        var items = await customerService.GetCustomersAsync();
        Customers.Clear();
        foreach (var item in items)
            Customers.Add(item);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        try
        {
            var customer = await customerService.AddCustomerAsync(FullName, Email);
            FullName = string.Empty;
            Email = string.Empty;
            await LoadAsync();
            ShowStatus($"Cliente \"{customer.Name}\" salvo no SQLite (SQL).", isError: false);
        }
        catch (ValidationException ex)
        {
            ShowStatus(ex.Message, isError: true);
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(Customer customer)
    {
        if (await customerService.DeleteCustomerAsync(customer.Id))
            Customers.Remove(customer);
    }

    private void ShowStatus(string message, bool isError)
    {
        StatusMessage = message;
        HasError = isError;
    }
}
