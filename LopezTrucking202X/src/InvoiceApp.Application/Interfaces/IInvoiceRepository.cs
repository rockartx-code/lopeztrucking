using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface IInvoiceRepository
{
    Task<Invoice?> GetByIdAsync(Guid invoiceId, CancellationToken cancellationToken = default);
    Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
