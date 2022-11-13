using MAUI.Core.Services.Dialog;
using System.Reactive.Disposables;

namespace MyExpenseManagement.ViewModels;
public class MainViewModel : ViewModelBase
{
    public MainViewModel(ISqliteService sqliteService, INavigationService navigationService, IDialogService dialogService)
    {
        _sqliteService = sqliteService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _ = InitAsync();
    }

    #region Fields

    private readonly ISqliteService _sqliteService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    private List<Expense> _expenses;
    private IEnumerable<Category> _items;
    private decimal _totalExpense;

    #endregion

    #region Properties

    public IEnumerable<Category> Items
    {
        get => _items;
        set => this.RaiseAndSetIfChanged(ref _items, value);
    }

    public List<Expense> Expenses
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

    public ReactiveCommand<Category, Unit> FilterCommand { get; private set; }
    public ReactiveCommand<Unit, Unit> NewExpenseCommand { get; private set; }
    public ReactiveCommand<Expense, Unit> UpdateExpenseCommand { get; private set; }

    protected override void RegisterCommands(CompositeDisposable disposables)
    {
        base.RegisterCommands(disposables);

        NewExpenseCommand = ReactiveCommand.CreateFromTask(NewExpense).DisposeWith(disposables);
        UpdateExpenseCommand = ReactiveCommand.CreateFromTask<Expense>(x => UpdateExpense(x)).DisposeWith(disposables);
        FilterCommand = ReactiveCommand.CreateFromTask<Category>(x => Filter(x)).DisposeWith(disposables);
    }

    #endregion

    #region Methods

    private Task Filter(Category x)
    {
        return Task.CompletedTask;
    }

    private async Task UpdateExpense(Expense x)
    {
        MessagingCenter.Unsubscribe<ExpenseViewModel>(this, Constants.MSG_REFRESH_EXPENSES);
        MessagingCenter.Subscribe<ExpenseViewModel>(this, Constants.MSG_REFRESH_EXPENSES, async _ => await LoadExpenses());
        await _navigationService.NavigateTo<ExpensePage>(new List<CustomParam>
        {
            new CustomParam { Name = nameof(Expense.Id), Value = x.Id }
        });
    }

    private async Task NewExpense()
    {
        MessagingCenter.Unsubscribe<ExpenseViewModel>(this, Constants.MSG_REFRESH_EXPENSES);
        MessagingCenter.Subscribe<ExpenseViewModel>(this, Constants.MSG_REFRESH_EXPENSES, async _ => await LoadExpenses());
        await _navigationService.NavigateTo<ExpensePage>();
    }

    protected async Task InitAsync()
    {
        try
        {
            MessagingCenter.Unsubscribe<ExpenseViewModel>(this, Constants.MSG_REFRESH_CATEGORIES);
            MessagingCenter.Subscribe<ExpenseViewModel>(this, Constants.MSG_REFRESH_CATEGORIES, async _ => await LoadExpenses());
            await LoadCategories();
            await LoadExpenses();
        }
        catch (Exception e) 
        {
            await _dialogService.ShowErrorAsync(e);
        }
    }

    private async Task LoadCategories()
    {
        Items = await _sqliteService.GetAll<Category>();
    }

    private async Task LoadExpenses()
    {
        var expenses = (await _sqliteService.GetAll<Expense>()).OrderByDescending(x => x.Date).ToList();
        foreach (var item in expenses)
        {
            item.Category = Items.FirstOrDefault(c => c.Id == item.IdCategory);
        }
        Expenses = expenses;
        TotalExpense = Expenses.Sum(x => x.Amount);
    }

    #endregion
}
