using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface ICompanyRepository
{
    Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default);
}
