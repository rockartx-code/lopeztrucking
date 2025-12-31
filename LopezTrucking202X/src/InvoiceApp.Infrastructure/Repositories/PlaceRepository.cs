using InvoiceApp.Application.Interfaces;
using System;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class PlaceRepository : IPlaceRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public PlaceRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Place>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Places
            .OrderBy(place => place.Name)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<Place>> GetByCompanyIdsAsync(
        IReadOnlyCollection<Guid> companyIds,
        CancellationToken cancellationToken = default)
    {
        if (companyIds.Count == 0)
        {
            return Array.Empty<Place>();
        }

        var places = await _dbContext.CompanyPlaces
            .Where(link => companyIds.Contains(link.CompanyId))
            .Select(link => new { link.Place, link.SortOrder })
            .Where(item => item.Place != null)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return places
            .GroupBy(item => item.Place!.Id)
            .Select(group => new
            {
                Place = group.First().Place!,
                SortOrder = group.Min(item => item.SortOrder)
            })
            .OrderBy(item => item.SortOrder)
            .ThenBy(item => item.Place.Name)
            .Select(item => item.Place)
            .ToList();
    }

    public Task<Place?> GetByIdAsync(Guid placeId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Places
            .FirstOrDefaultAsync(place => place.Id == placeId, cancellationToken);
    }

    public Task AddAsync(Place place, CancellationToken cancellationToken = default)
    {
        _dbContext.Places.Add(place);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Place place, CancellationToken cancellationToken = default)
    {
        _dbContext.Places.Update(place);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid placeId, CancellationToken cancellationToken = default)
    {
        var place = await _dbContext.Places
            .FirstOrDefaultAsync(item => item.Id == placeId, cancellationToken)
            .ConfigureAwait(false);

        if (place is null)
        {
            return;
        }

        _dbContext.Places.Remove(place);
    }

    public async Task UpdateFlagsAsync(
        Guid placeId,
        bool isCompany,
        bool isFrom,
        bool isTo,
        CancellationToken cancellationToken = default)
    {
        var place = await _dbContext.Places
            .FirstOrDefaultAsync(item => item.Id == placeId, cancellationToken);

        if (place is null)
        {
            return;
        }

        place.IsCompany = isCompany;
        place.IsFrom = isFrom;
        place.IsTo = isTo;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
