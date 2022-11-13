using System.Reactive.Disposables;
using System.Reflection;
using System.Windows.Input;

namespace MyExpenseManagement.Views;

public partial class CategoryPage : PageBase<CategoryViewModel>
{
	public CategoryPage(CategoryViewModel vm) : base(vm)
	{
		InitializeComponent();
	}

	protected override void OnActivated(CompositeDisposable disposables)
	{
		base.OnActivated(disposables);

		this.Bind(ViewModel, x => x.Icon, x => x.entryIcon.Text).DisposeWith(disposables);
		this.OneWayBind(ViewModel, x => x.Icon, x => x.fontImageIcon.Glyph).DisposeWith(disposables);
		this.Bind(ViewModel, x => x.Description, x => x.entryDescription.Text).DisposeWith(disposables);
		this.Bind(ViewModel, x => x.SelectedColor, x => x.colorPicker.PickedColor).DisposeWith(disposables);
		this.OneWayBind(ViewModel, x => x.SelectedColor, x => x.fontImageIcon.Color).DisposeWith(disposables);

		this.BindCommand(ViewModel, vm => vm.AcceptCommand, v => v.btnAccept).DisposeWith(disposables);
    }
}