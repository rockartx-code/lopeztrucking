using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.Interfaces;

public interface ISettingRepository
{
    Task<Setting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<decimal?> GetDecimalByKeyAsync(string key, CancellationToken cancellationToken = default);
}
