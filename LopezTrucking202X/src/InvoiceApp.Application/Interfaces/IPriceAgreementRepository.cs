using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface IPriceAgreementRepository
{
    Task<PriceAgreement?> FindByMixAsync(
        Guid companyId,
        string mixName,
        DateOnly asOfDate,
        CancellationToken cancellationToken = default);

    Task AddAsync(PriceAgreement priceAgreement, CancellationToken cancellationToken = default);
    Task UpdateAsync(PriceAgreement priceAgreement, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
