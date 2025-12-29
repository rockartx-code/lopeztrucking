using System;
using System.Globalization;

namespace LopezTruck202X.Models;

public sealed class InvoiceSummary
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DateTimeOffset Date { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }

    public string DateDisplay => Date.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);

    public string TotalDisplay => Total.ToString("C", CultureInfo.CurrentCulture);
}
