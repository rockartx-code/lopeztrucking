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
            .OrderBy(subhauler => subhauler.Name)
            .AsNoTracking()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public Task<Subhauler?> GetByIdAsync(Guid subhaulerId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Subhaulers
            .FirstOrDefaultAsync(subhauler => subhauler.Id == subhaulerId, cancellationToken);
    }

    public Task AddAsync(Subhauler subhauler, CancellationToken cancellationToken = default)
    {
        _dbContext.Subhaulers.Add(subhauler);
        return Task.CompletedTask;
    }

    public void Remove(Subhauler subhauler)
    {
        _dbContext.Subhaulers.Remove(subhauler);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
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
