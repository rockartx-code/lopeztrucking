using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class CompanyRepository : ICompanyRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public CompanyRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Companies
            .OrderBy(company => company.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}
