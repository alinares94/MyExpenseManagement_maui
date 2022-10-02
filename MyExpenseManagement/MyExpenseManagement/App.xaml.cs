using MyExpenseManagement.Views;

namespace MyExpenseManagement;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        MainPage = new AppShell();
	}
}
