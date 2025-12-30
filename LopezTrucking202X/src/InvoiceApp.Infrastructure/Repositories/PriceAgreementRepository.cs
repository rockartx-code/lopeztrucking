using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class PriceAgreementRepository : IPriceAgreementRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public PriceAgreementRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<PriceAgreement?> FindByMixAsync(
        Guid companyId,
        string mixName,
        DateOnly asOfDate,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.PriceAgreements
            .Include(agreement => agreement.Items)
            .Where(agreement => agreement.CompanyId == companyId)
            .Where(agreement => agreement.MixName == mixName)
            .Where(agreement => agreement.EffectiveDate <= asOfDate)
            .OrderByDescending(agreement => agreement.EffectiveDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task AddAsync(PriceAgreement priceAgreement, CancellationToken cancellationToken = default)
    {
        return _dbContext.PriceAgreements.AddAsync(priceAgreement, cancellationToken).AsTask();
    }

    public Task UpdateAsync(PriceAgreement priceAgreement, CancellationToken cancellationToken = default)
    {
        _dbContext.PriceAgreements.Update(priceAgreement);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
