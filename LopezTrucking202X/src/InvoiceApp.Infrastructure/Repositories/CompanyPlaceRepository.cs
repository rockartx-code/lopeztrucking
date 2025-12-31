using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Infrastructure.Persistence;
using InvoiceApp.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class CompanyPlaceRepository : ICompanyPlaceRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public CompanyPlaceRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CompanyPlaceLinkDto>> GetByCompanyIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.CompanyPlaces
            .Where(link => link.CompanyId == companyId)
            .OrderBy(link => link.SortOrder)
            .Select(link => new CompanyPlaceLinkDto(link.PlaceId, link.SortOrder))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task ReplaceLinksAsync(
        Guid companyId,
        IReadOnlyCollection<CompanyPlaceLinkDto> links,
        CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.CompanyPlaces
            .Where(link => link.CompanyId == companyId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var incoming = links.ToDictionary(link => link.PlaceId, link => link.SortOrder);

        foreach (var current in existing)
        {
            if (!incoming.TryGetValue(current.PlaceId, out var sortOrder))
            {
                _dbContext.CompanyPlaces.Remove(current);
                continue;
            }

            current.SortOrder = sortOrder;
        }

        foreach (var link in links)
        {
            if (existing.Any(item => item.PlaceId == link.PlaceId))
            {
                continue;
            }

            _dbContext.CompanyPlaces.Add(new CompanyPlace
            {
                CompanyId = companyId,
                PlaceId = link.PlaceId,
                SortOrder = link.SortOrder
            });
        }
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
