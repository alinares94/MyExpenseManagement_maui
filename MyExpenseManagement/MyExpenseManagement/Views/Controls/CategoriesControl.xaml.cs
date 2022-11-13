using System.Windows.Input;

namespace MyExpenseManagement.Views.Controls;

public partial class CategoriesControl : ContentView
{
	public CategoriesControl()
	{
		InitializeComponent();
	}
    public ICommand TapCategory
    {
        get { return (ICommand)GetValue(TapCategoryProperty); }
        set { SetValue(TapCategoryProperty, value); }
    }
    public static readonly BindableProperty TapCategoryProperty =
        BindableProperty.Create(nameof(TapCategory), typeof(ICommand), typeof(CategoriesControl), default);

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

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        if (sender is ContentView view && view.BindingContext is Category cat && TapCategory != null && TapCategory.CanExecute(null))
            TapCategory.Execute(cat);
    }
}