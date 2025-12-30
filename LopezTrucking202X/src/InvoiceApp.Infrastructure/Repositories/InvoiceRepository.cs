using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public InvoiceRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Invoice?> GetByIdAsync(Guid invoiceId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Invoices
            .Include(invoice => invoice.DetailGroups)
            .FirstOrDefaultAsync(invoice => invoice.Id == invoiceId, cancellationToken);
    }

    public Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        return _dbContext.Invoices.AddAsync(invoice, cancellationToken).AsTask();
    }

    public Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        _dbContext.Invoices.Update(invoice);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
