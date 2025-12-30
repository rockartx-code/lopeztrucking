using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface IPlaceRepository
{
    Task<IReadOnlyList<Place>> GetByCompanyIdsAsync(
        IReadOnlyCollection<Guid> companyIds,
        CancellationToken cancellationToken = default);
    Task<Place?> GetByIdAsync(Guid placeId, CancellationToken cancellationToken = default);
    Task UpdateFlagsAsync(
        Guid placeId,
        bool isCompany,
        bool isFrom,
        bool isTo,
        CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
