using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface ISubhaulerRepository
{
    Task<IReadOnlyList<Subhauler>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateLastInvoiceNoAsync(Guid subhaulerId, int lastInvoiceNo, CancellationToken cancellationToken = default);
}
