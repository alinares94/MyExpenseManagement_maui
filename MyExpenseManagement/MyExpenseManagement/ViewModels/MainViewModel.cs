using ReactiveUI;
using System.Reactive;

namespace MyExpenseManagement.ViewModels;
public class MainViewModel : ViewModelBase
{
    public MainViewModel(ISqliteService sqliteService)
    {
        _sqliteService = sqliteService;
        _ = InitAsync();
    }

    #region Fields

    private readonly ISqliteService _sqliteService;

    private IEnumerable<Expense> _expenses;
    private IEnumerable<Category> _items;
    private decimal _totalExpense;

    #endregion

    #region Properties

    public IEnumerable<Category> Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public IEnumerable<Expense> Expenses
    {
        get => _expenses;
        set => this.RaiseAndSetIfChanged(ref _expenses, value);
    }

    public decimal TotalExpense
    {
        get => _totalExpense;
        set => this.RaiseAndSetIfChanged(ref _totalExpense, value);
    }

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> NewExpenseCommand { get; private set; }

    protected override void RegisterCommands()
    {
        base.RegisterCommands();

        NewExpenseCommand = ReactiveCommand.CreateFromTask(() => Shell.Current.GoToAsync(nameof(ExpensePage)));
    }

    #endregion

    #region Methods

    protected override async Task InitAsync()
    {
        try
        {
            Items = await _sqliteService.GetAll<Category>();
            Expenses = await _sqliteService.GetAll<Expense>();
            TotalExpense = Expenses.Sum(x => x.Amount);
        }
        catch
        {

        }
    }

    #endregion
}
