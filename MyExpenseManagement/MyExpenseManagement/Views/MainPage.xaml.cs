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
		this.OneWayBind(ViewModel, x => x.TotalExpense, x => x.expense.Total).DisposeWith(disposables);

		this.BindCommand(ViewModel, vm => vm.NewExpenseCommand, v => v.btnNewExpense).DisposeWith(disposables);
		this.OneWayBind(ViewModel, vm => vm.FilterCommand, v => v.categories.TapCategory).DisposeWith(disposables);

    }

	private async void Button_Clicked(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync(nameof(ExpensePage));
    }

	private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
	{
		if (sender is Grid grid && grid.BindingContext is Expense expense)
		ViewModel.UpdateExpenseCommand.Execute(expense);
    }
}
