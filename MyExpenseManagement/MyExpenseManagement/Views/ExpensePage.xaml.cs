using ReactiveUI;
using System.Reactive.Disposables;

namespace MyExpenseManagement.Views;

public partial class ExpensePage : PageBase<ExpenseViewModel>
{
	public ExpensePage(ExpenseViewModel vm) : base(vm)
	{
		InitializeComponent();
	}

	protected override void OnActivated(CompositeDisposable disposables)
	{
		base.OnActivated(disposables);

		this.Bind(ViewModel, x => x.Amount, x => x.entryAmount.Text).DisposeWith(disposables);
		this.OneWayBind(ViewModel, x => x.Categories, x => x.pickerCategory.ItemsSource).DisposeWith(disposables);
        //this.Bind(ViewModel, x => x.Date, x => x.pickerDate.Date).DisposeWith(disposables);
        this.Bind(ViewModel, x => x.Description, x => x.entryDescription.Text).DisposeWith(disposables);
	}
}