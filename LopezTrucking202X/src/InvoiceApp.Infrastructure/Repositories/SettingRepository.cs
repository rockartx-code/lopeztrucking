using System.Globalization;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Repositories;

public sealed class SettingRepository : ISettingRepository
{
    private readonly InvoiceAppDbContext _dbContext;

    public SettingRepository(InvoiceAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Setting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return _dbContext.Settings
            .FirstOrDefaultAsync(setting => setting.Key == key, cancellationToken);
    }

    public async Task<decimal?> GetDecimalByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var setting = await GetByKeyAsync(key, cancellationToken).ConfigureAwait(false);
        if (setting is null)
        {
            return null;
        }

        if (decimal.TryParse(setting.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        return decimal.TryParse(setting.Value, NumberStyles.Number, CultureInfo.CurrentCulture, out parsed)
            ? parsed
            : null;
    }
}
