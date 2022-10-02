using SQLiteNetExtensions.Attributes;

namespace MyExpenseManagement.Models;
public class Expense : ModelWithId
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }

    [ForeignKey(typeof(Category))]
    public int IdCategory { get; set; }

    [ManyToOne]
    public Category Category { get; set; }

}
