using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface IPriceAgreementRepository
{
    Task<PriceAgreement?> FindByMixAsync(
        Guid companyId,
        string fingerprintHash,
        DateOnly asOfDate,
        CancellationToken cancellationToken = default);

    Task<PriceAgreement?> FindByMixAndItemsAsync(
        Guid companyId,
        string fingerprintHash,
        IReadOnlyCollection<ItemType> itemTypes,
        DateOnly effectiveDate,
        CancellationToken cancellationToken = default);

    Task AddAsync(PriceAgreement priceAgreement, CancellationToken cancellationToken = default);
    Task UpdateAsync(PriceAgreement priceAgreement, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
