namespace MyExpenseManagement.Models;
public class Category : ModelWithId
{
    public string Icon { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }

    [Ignore]
    public Color ColorView => Microsoft.Maui.Graphics.Color.FromArgb(Color);
}
