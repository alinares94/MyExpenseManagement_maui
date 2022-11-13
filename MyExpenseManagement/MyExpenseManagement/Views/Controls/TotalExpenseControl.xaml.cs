namespace MyExpenseManagement.Views.Controls;

public partial class TotalExpenseControl : ContentView
{
	public TotalExpenseControl()
	{
		InitializeComponent();
	}

    public decimal Total
    {
        get { return (decimal)GetValue(TotalProperty); }
        set { SetValue(TotalProperty, value); }
    }
    public static readonly BindableProperty TotalProperty =
        BindableProperty.Create(nameof(Total), typeof(decimal), typeof(TotalExpenseControl), 0M);

}