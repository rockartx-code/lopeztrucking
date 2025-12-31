using InvoiceApp.Application.DTOs;

namespace InvoiceApp.Application.Interfaces;

public interface ICompanyPlaceRepository
{
    Task<IReadOnlyList<CompanyPlaceLinkDto>> GetByCompanyIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default);
    Task ReplaceLinksAsync(
        Guid companyId,
        IReadOnlyCollection<CompanyPlaceLinkDto> links,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
