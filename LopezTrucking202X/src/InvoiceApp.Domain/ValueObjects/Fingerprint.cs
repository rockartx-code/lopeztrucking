namespace InvoiceApp.Domain.ValueObjects;

public readonly record struct Fingerprint(string Value)
{
    public override string ToString() => Value;
}
