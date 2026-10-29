using System.Globalization;
using EasyShop.Data;
using EasyShop.Services;
using EasyShop.ViewModels;
using EasyShop.Views;
using Microsoft.Extensions.Logging;

namespace EasyShop;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		// Formata moeda/números no padrão brasileiro independente do idioma do emulador.
		var culture = new CultureInfo("pt-BR");
		CultureInfo.DefaultThreadCurrentCulture = culture;
		CultureInfo.DefaultThreadCurrentUICulture = culture;

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Camada de Acesso a Dados: um repositório por paradigma de banco.
		builder.Services.AddSingleton<IProductRepository, LiteDbProductRepository>();   // NoSQL
		builder.Services.AddSingleton<ICustomerRepository, SqliteCustomerRepository>(); // SQL

		// Camada de Negócio
		builder.Services.AddSingleton<ProductService>();
		builder.Services.AddSingleton<CustomerService>();

		// Camada de Apresentação
		builder.Services.AddSingleton<ProductsViewModel>();
		builder.Services.AddSingleton<CustomersViewModel>();
		builder.Services.AddTransient<ProductsPage>();
		builder.Services.AddTransient<CustomersPage>();

#if ANDROID
		// O teclado numérico do Android só aceita o separador decimal do idioma do aparelho
		// (em en-US a vírgula é descartada: "59,9" vira "599"). Liberamos vírgula e ponto.
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("DecimalSeparator", (handler, entry) =>
		{
			if (entry.Keyboard == Keyboard.Numeric)
				handler.PlatformView.KeyListener = Android.Text.Method.DigitsKeyListener.GetInstance("0123456789.,");
		});
#endif

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
