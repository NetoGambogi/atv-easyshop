using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyShop.Models;
using EasyShop.Services;

namespace EasyShop.ViewModels;

public partial class ProductsViewModel(ProductService productService) : ObservableObject
{
    public ObservableCollection<Product> Products { get; } = new();
    public ObservableCollection<AttributeEntry> PendingAttributes { get; } = new();

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _priceText = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;

    [ObservableProperty]
    private string _newAttributeKey = string.Empty;

    [ObservableProperty]
    private string _newAttributeValue = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    [RelayCommand]
    private async Task LoadAsync()
    {
        var items = await productService.GetProductsAsync();
        Products.Clear();
        foreach (var item in items)
            Products.Add(item);
    }

    [RelayCommand]
    private void AddAttribute()
    {
        var key = NewAttributeKey.Trim();
        var value = NewAttributeValue.Trim();
        if (key.Length == 0 || value.Length == 0)
        {
            ShowStatus("Preencha o nome e o valor do atributo.", isError: true);
            return;
        }

        var existing = PendingAttributes.FirstOrDefault(a => a.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
            PendingAttributes.Remove(existing);

        PendingAttributes.Add(new AttributeEntry(key, value));
        NewAttributeKey = string.Empty;
        NewAttributeValue = string.Empty;
        StatusMessage = string.Empty;
    }

    [RelayCommand]
    private void RemoveAttribute(AttributeEntry entry) => PendingAttributes.Remove(entry);

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!TryParsePrice(PriceText, out var price))
        {
            ShowStatus("Preço inválido. Use, por exemplo, 199.90 ou 199,90.", isError: true);
            return;
        }

        try
        {
            var attributes = PendingAttributes.Select(a => KeyValuePair.Create(a.Key, a.Value));
            var product = await productService.AddProductAsync(Name, price, Category, attributes);

            Products.Insert(0, product);
            ClearForm();
            ShowStatus($"Produto \"{product.Name}\" salvo no LiteDB (NoSQL).", isError: false);
        }
        catch (ValidationException ex)
        {
            ShowStatus(ex.Message, isError: true);
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(Product product)
    {
        if (await productService.DeleteProductAsync(product.Id))
            Products.Remove(product);
    }

    private void ClearForm()
    {
        Name = string.Empty;
        PriceText = string.Empty;
        Category = string.Empty;
        NewAttributeKey = string.Empty;
        NewAttributeValue = string.Empty;
        PendingAttributes.Clear();
    }

    private void ShowStatus(string message, bool isError)
    {
        StatusMessage = message;
        HasError = isError;
    }

    // Aceita vírgula ou ponto como separador decimal, independente da cultura do aparelho.
    private static bool TryParsePrice(string text, out decimal price) =>
        decimal.TryParse(text?.Trim().Replace(',', '.'), NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out price);
}
