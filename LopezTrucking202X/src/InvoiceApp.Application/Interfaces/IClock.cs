namespace InvoiceApp.Application.Interfaces;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
    DateOnly Today { get; }
}
