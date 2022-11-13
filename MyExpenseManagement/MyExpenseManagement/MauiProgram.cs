
using CommunityToolkit.Maui;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;
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
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("materialdesignicons.ttf", MaterialIcons.FontFamily);
			});

		builder.Services.AddMAUICore(x =>
		{
			x.SqliteSettings.DatabaseFilename = Constants.DB_FILE_NAME;
			x.SqliteSettings.Types = new List<Type> { typeof(Category), typeof(Expense) };
			x.NavigationSettings.NavigationTypeEnum = NavigationTypeEnum.Shell;
        });

        builder.Services.AddSingleton<MainPage, MainViewModel>();
		builder.Services.AddSingleton<SettingsPage, SettingsViewModel>();
        builder.Services.AddTransientWithShellRoute<CategoryPage, CategoryViewModel>(nameof(CategoryPage));
        builder.Services.AddTransientWithShellRoute<ExpensePage, ExpenseViewModel>(nameof(ExpensePage));

		return builder.Build();
	}
}
