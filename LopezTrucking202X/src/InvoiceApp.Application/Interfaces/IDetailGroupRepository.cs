using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface IDetailGroupRepository
{
    Task<DetailGroup?> GetByIdAsync(Guid detailGroupId, CancellationToken cancellationToken = default);
    Task AddToInvoiceAsync(Guid invoiceId, DetailGroup detailGroup, CancellationToken cancellationToken = default);
    Task UpdateAsync(DetailGroup detailGroup, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
