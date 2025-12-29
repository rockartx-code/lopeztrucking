using System;

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
}
