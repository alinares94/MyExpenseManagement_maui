using System.ComponentModel;

namespace MyExpenseManagement.Enums;
internal enum TypeFilterEnum
{
    [Description("Semana")]
    Week,
    [Description("Mes")]
    Month,
    [Description("Año")]
    Year
}
