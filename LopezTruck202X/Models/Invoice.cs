using System;
using System.Collections.Generic;
using System.Globalization;

namespace LopezTruck202X.Models;

public sealed class Invoice
{
    public int Id { get; set; }
    public int Number { get; set; }
    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    public string CheckNumber { get; set; } = string.Empty;
    public Customer Customer { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Advance { get; set; }
    public decimal Total { get; set; }
    public List<InvoiceLine> Lines { get; set; } = new();

    public string DateDisplay => Date.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);

    public string AdvanceDisplay => $"Advance: {Advance.ToString("C", CultureInfo.CurrentCulture)}";

    public string SubtotalDisplay => $"Subtotal: {Subtotal.ToString("C", CultureInfo.CurrentCulture)}";

    public string TotalDisplay => $"Total: {Total.ToString("C", CultureInfo.CurrentCulture)}";
}
