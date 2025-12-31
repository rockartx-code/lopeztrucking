using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface ISubhaulerRepository
{
    Task<IReadOnlyList<Subhauler>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Subhauler?> GetByIdAsync(Guid subhaulerId, CancellationToken cancellationToken = default);
    Task AddAsync(Subhauler subhauler, CancellationToken cancellationToken = default);
    void Remove(Subhauler subhauler);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
