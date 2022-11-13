using ReactiveUI;
using System.Reactive.Disposables;

namespace MyExpenseManagement.ViewModels;
public class ExpenseViewModel : ViewModelBase
{
    public ExpenseViewModel(ISqliteService sqliteService, INavigationService navigationService)
    {
        _sqliteService = sqliteService;
        _navigationService = navigationService;
        _ = InitAsync();
    }

    #region Fields

    private readonly ISqliteService _sqliteService;
    private readonly INavigationService _navigationService;

    private decimal _amount;
	private List<Category> _categories = new();
	private DateTime _date = DateTime.Now;
	private string _description;
	private Category _selectedCategory;
	private int _id;

    #endregion

    #region Properties

    public decimal Amount
	{
		get => _amount;
		set => this.RaiseAndSetIfChanged(ref _amount, value);
	}

    public DateTime Date
    {
        get => _date;
        set => this.RaiseAndSetIfChanged(ref _date, value);
    }

    public List<Category> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    public string Description
    {
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    public Category SelectedCategory
    {
        get => _selectedCategory;
        set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
    }

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> AcceptCommand { get; private set; }

    protected override void RegisterCommands(CompositeDisposable disposables)
    {
        base.RegisterCommands(disposables);

        AcceptCommand = ReactiveCommand.CreateFromTask(Accept).DisposeWith(disposables);
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
        catch { }
    }

    private static void ValidateEntity(Expense entity)
    {
        if (entity == null)
            throw new Exception("Entity shouldn't be null");
        if (entity.Amount == 0)
            throw new Exception("Amount shouldn't be zero");
        if (entity.IdCategory == 0)
            throw new Exception("Category shouldn't be null");
    }

    private Expense GetEntity()
    {
        return new Expense
        {
            Id = _id,
            IdCategory = _selectedCategory.Id,
            Amount = _amount,
            Date = _date,
            Description = _description,
        };
    }

    public async Task InitAsync()
    {
        Categories = (await _sqliteService.GetAll<Category>()).ToList();
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);

        if (query.TryGetValue(nameof(Expense.Id), out object id))
            _ = LoadById((int)id);
    }

    private async Task LoadById(int id)
    {
        var entity = await _sqliteService.GetById<Expense>(id);
        if (entity == null)
            return;

        _id = entity.Id;
        Description = entity.Description;
        Date = entity.Date;
        Amount = entity.Amount;
        SelectedCategory = Categories.FirstOrDefault(x => x.Id == entity.IdCategory);
    }

    #endregion
}
