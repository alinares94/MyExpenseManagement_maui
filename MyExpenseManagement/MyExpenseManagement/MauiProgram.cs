
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MyExpenseManagement;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("materialdesignicons.ttf", MaterialIcons.FontFamily);
			});

        _ = SqliteService.InitDatabase(GetCoreSettings().SqliteSettings);
        builder.Services.AddSingleton<ISqliteService, SqliteService>();
		builder.Services.AddMAUICore(GetCoreSettings());

        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<SettingsPage>();
        builder.Services.AddTransient<CategoryPage>();
        builder.Services.AddTransient<ExpensePage>();

		builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<SettingsViewModel>();
        builder.Services.AddTransient<CategoryViewModel>();
        builder.Services.AddTransient<ExpenseViewModel>();

        Routing.RegisterRoute(nameof(ExpensePage), typeof(ExpensePage));
        Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));

        return builder.Build();
	}

	private static CoreSettings GetCoreSettings()
	{
		return new CoreSettings
		{
			SqliteSettings = new()
			{
				DatabaseFilename = Constants.DB_FILE_NAME,
				Types = new List<Type> { typeof(Category), typeof(Expense) }
			},
			NavigationSettings = new()
			{
				NavigationTypeEnum = NavigationTypeEnum.Shell
			}
		};
	}
}
