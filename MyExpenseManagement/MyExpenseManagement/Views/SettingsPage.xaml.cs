using ReactiveUI;
using System.Reactive.Disposables;

namespace MyExpenseManagement.Views;

public partial class SettingsPage : PageBase<SettingsViewModel>
{
	public SettingsPage(SettingsViewModel vm) : base(vm)
	{
		InitializeComponent();
	}

	protected override void OnActivated(CompositeDisposable disposables)
	{
		base.OnActivated(disposables);

        this.BindCommand(ViewModel, vm => vm.NewCategoryCommand, v => v.btnNewCategory).DisposeWith(disposables);
    }
}