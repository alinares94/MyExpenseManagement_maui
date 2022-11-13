using MAUI.Core.Services.Dialog;
using System.Reactive.Disposables;

namespace MyExpenseManagement.ViewModels;
public class CategoryViewModel : ViewModelBase
{
    public CategoryViewModel(INavigationService navigationService, ISqliteService sqliteService, IDialogService dialogService)
    {
        _navigationService = navigationService;
        _sqliteService = sqliteService;
        _dialogService = dialogService;
    }

    #region Fields

    private readonly INavigationService _navigationService;
    private readonly ISqliteService _sqliteService;
    private readonly IDialogService _dialogService;

    private Color _selectedColor = Colors.Red;
    private string _icon;
    private string _description;

    #endregion

    #region Properties

    public Color SelectedColor
    {
        get => _selectedColor;
        set => this.RaiseAndSetIfChanged(ref _selectedColor, value);
    }

    public string Icon
    {
        get => _icon;
        set => this.RaiseAndSetIfChanged(ref _icon, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> AcceptCommand { get; private set; }

    protected override void RegisterCommands(CompositeDisposable disposables)
    {
        base.RegisterCommands(disposables);

        AcceptCommand = ReactiveCommand.CreateFromTask(Accept);
    }

    #endregion

    #region Methods

    private async Task Accept()
    {
        try
        {
            var entity = GetEntity();
            ValidateEntity(entity);
            await _sqliteService.Save(entity);
            await _navigationService.NavigateBack();
            MessagingCenter.Send(this, Constants.MSG_REFRESH_EXPENSES);
        }
        catch (Exception ex) 
        {
            await _dialogService.ShowDialogAsync("Error", ex.Message, "Cerrar");
        }
    }

    private static void ValidateEntity(Category entity)
    {
        if (entity == null)
            throw new Exception("Entity shouldn't be null");
        if (string.IsNullOrEmpty(entity.Icon))
            throw new Exception("Icon shouldn't be null");
        if (string.IsNullOrEmpty(entity.Description))
            throw new Exception("Description shouldn't be null");
    }

    private Category GetEntity()
    {
        return new Category
        {
            Color = SelectedColor.ToHex(),
            Icon = Icon,
            Description = Description
        };
    }

    #endregion
}
