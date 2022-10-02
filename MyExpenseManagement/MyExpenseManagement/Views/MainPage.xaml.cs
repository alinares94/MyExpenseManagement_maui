using ReactiveUI;
using System.Reactive.Disposables;

namespace MyExpenseManagement.Views;

public partial class MainPage : PageBase<MainViewModel>
{
	public MainPage(MainViewModel vm) : base(vm)
	{
		InitializeComponent();
	}

	protected override void OnActivated(CompositeDisposable disposables)
	{
		base.OnActivated(disposables);

		this.OneWayBind(ViewModel, x => x.Items, x => x.categories.Categories).DisposeWith(disposables);
		this.OneWayBind(ViewModel, x => x.TotalExpense, x => x.lbTotal.Text).DisposeWith(disposables);

        this.BindCommand(ViewModel, vm => vm.NewExpenseCommand, v => v.btnNewExpense).DisposeWith(disposables);

        var width = 350;

        positive.WidthRequest = width * 0.3;
		negative.WidthRequest = width * 0.7;

    }

	private async void Button_Clicked(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync(nameof(ExpensePage));
    }
}

