using ReactiveUI;

namespace MyExpenseManagement.ViewModels;
public class ExpenseViewModel : ViewModelBase
{
    public ExpenseViewModel(ISqliteService sqliteService)
    {
        _sqliteService = sqliteService;
        _ = InitAsync();
    }

    #region Fields

    private ISqliteService _sqliteService;

    private int _amount;
	private List<Category> _categories = new();
	private DateTime _date = DateTime.Now;
	private string _description;

    #endregion

    #region Properties

    public int Amount
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

    #endregion

    #region Methods

    public async Task InitAsync()
    {
        Categories = (await _sqliteService.GetAll<Category>()).ToList();
    }

    #endregion
}
