using ReactiveUI;
using System.Reactive;

namespace MyExpenseManagement.ViewModels;
public class SettingsViewModel : ViewModelBase
{
    #region Commands

    public ReactiveCommand<Unit, Unit> NewCategoryCommand { get; private set; }

    protected override void RegisterCommands()
    {
        base.RegisterCommands();

        NewCategoryCommand = ReactiveCommand.CreateFromTask(() => Shell.Current.GoToAsync(nameof(CategoryPage)));
    }

    #endregion
}
