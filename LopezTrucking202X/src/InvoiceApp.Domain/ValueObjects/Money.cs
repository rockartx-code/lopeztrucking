namespace InvoiceApp.Domain.ValueObjects;

public readonly record struct Money(decimal Amount, string Currency)
{
    public static Money USD(decimal amount) => new(amount, "USD");

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
