namespace MyExpenseManagement.Views.Controls;

public partial class CategoriesControl : ContentView
{
	public CategoriesControl()
	{
		InitializeComponent();
	}

    public IEnumerable<Category> Categories
    {
        get { return (IEnumerable<Category>)GetValue(CategoriesProperty); }
        set { SetValue(CategoriesProperty, value); }
    }
    public static readonly BindableProperty CategoriesProperty =
		BindableProperty.Create(nameof(Categories), typeof(IEnumerable<Category>), typeof(CategoriesControl), Enumerable.Empty<Category>(), propertyChanged: OnCategoriesChanged);

    private static void OnCategoriesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is CategoriesControl control)
            control.collection.ItemsSource = (IEnumerable<Category>)newValue;
    }
}