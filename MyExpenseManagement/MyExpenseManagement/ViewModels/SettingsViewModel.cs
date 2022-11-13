using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reflection;

namespace MyExpenseManagement.ViewModels;
public class SettingsViewModel : ViewModelBase
{
    public SettingsViewModel(INavigationService navigationService, IStorageService storageService)
    {
        _navigationService = navigationService;
        _storageService = storageService;
        _ = InitAsync();
    }

    #region Fields

    private readonly INavigationService _navigationService;
    private readonly IStorageService _storageService;
    private List<string> _filterTypes;
    private string _selectedFilter;

    #endregion

    #region Properties

    public List<string> FilterTypes
    {
        get => _filterTypes; 
        set => this.RaiseAndSetIfChanged(ref _filterTypes, value);
    }

    public string SelectedFilter
    {
        get => _selectedFilter;
        set => this.RaiseAndSetIfChanged(ref _selectedFilter, value);
    }

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> NewCategoryCommand { get; private set; }

    protected override void RegisterCommands(CompositeDisposable disposables)
    {
        base.RegisterCommands(disposables);

        NewCategoryCommand = ReactiveCommand.CreateFromTask(() => _navigationService.NavigateTo<CategoryPage>()).DisposeWith(disposables);
    }

    #endregion

    #region Methods

    protected override void RegisterProperties(CompositeDisposable disposables)
    {
        base.RegisterProperties(disposables);

        this.WhenAnyValue(x => x.SelectedFilter).Subscribe(async x => await SaveFilter(x)).DisposeWith(disposables);
    }

    private async Task SaveFilter(string filter)
    {
        if (string.IsNullOrEmpty(filter))
            return;
        await _storageService.Save(Constants.FILTER_KEY, filter);
    }

    private async Task InitAsync()
    {
        var aux = new List<string>();
        foreach (var item in Enum.GetValues(typeof(TypeFilterEnum)))
            aux.Add(GetEnumDescription((TypeFilterEnum)item));

        FilterTypes = aux;
        SelectedFilter = await _storageService.Get(Constants.FILTER_KEY);
    }

    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        return fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any()
            ? attributes.First().Description
            : value.ToString();
    }

    #endregion
}
