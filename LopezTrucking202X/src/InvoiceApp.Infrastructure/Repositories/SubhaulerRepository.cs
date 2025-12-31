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
    }
}
