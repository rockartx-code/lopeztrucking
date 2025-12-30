using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class DetailGroupRepository : IDetailGroupRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public DetailGroupRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<DetailGroup?> GetByIdAsync(Guid detailGroupId, CancellationToken cancellationToken = default)
    {
        return _dbContext.DetailGroups
            .FirstOrDefaultAsync(detailGroup => detailGroup.Id == detailGroupId, cancellationToken);
    }

    public async Task AddToInvoiceAsync(
        Guid invoiceId,
        DetailGroup detailGroup,
        CancellationToken cancellationToken = default)
    {
        var entry = _dbContext.Entry(detailGroup);
        entry.Property("InvoiceId").CurrentValue = invoiceId;
        await _dbContext.DetailGroups.AddAsync(detailGroup, cancellationToken);
    }

    public Task UpdateAsync(DetailGroup detailGroup, CancellationToken cancellationToken = default)
    {
        _dbContext.DetailGroups.Update(detailGroup);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
