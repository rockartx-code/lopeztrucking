using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class SubhaulerRepository : ISubhaulerRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public SubhaulerRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Subhauler>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Subhaulers
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task UpdateLastInvoiceNoAsync(
        Guid subhaulerId,
        int lastInvoiceNo,
        CancellationToken cancellationToken = default)
    {
        var subhauler = await _dbContext.Subhaulers
            .FirstOrDefaultAsync(entity => entity.Id == subhaulerId, cancellationToken)
            .ConfigureAwait(false);

        if (subhauler is null)
        {
            return;
        }

        if (subhauler.LastInvoiceNo is null || lastInvoiceNo > subhauler.LastInvoiceNo)
        {
            subhauler.LastInvoiceNo = lastInvoiceNo;
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
