using InvoiceApp.Application.Interfaces;
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

    public Task<Place?> GetByIdAsync(Guid placeId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Places
            .FirstOrDefaultAsync(place => place.Id == placeId, cancellationToken);
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
