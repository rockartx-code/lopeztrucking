using InvoiceApp.Application.Interfaces;
using InvoiceApp.Application.UseCases;
using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.UseCases.Commands;

public sealed class AddDetailGroupHandler
{
    private readonly IDetailGroupRepository _detailGroupRepository;
    private readonly ISettingRepository _settingRepository;

    public AddDetailGroupHandler(
        IDetailGroupRepository detailGroupRepository,
        ISettingRepository settingRepository)
    {
        _detailGroupRepository = detailGroupRepository;
        _settingRepository = settingRepository;
    }

    public async Task<DetailGroup> HandleAsync(AddDetailGroup command, CancellationToken cancellationToken = default)
    {
        var detailGroup = new DetailGroup
        {
            Description = command.Description,
            AmountBase = command.AmountBase,
            EmptiesCount = command.EmptiesCount
        };

        var emptyUnitPrice = await _settingRepository
            .GetDecimalByKeyAsync(SettingKeys.EmptyUnitPrice, cancellationToken)
            .ConfigureAwait(false);

        detailGroup.SnapshotEmptyUnitPrice(emptyUnitPrice ?? 0m);

        await _detailGroupRepository.AddToInvoiceAsync(command.InvoiceId, detailGroup, cancellationToken)
            .ConfigureAwait(false);
        await _detailGroupRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return detailGroup;
    }
}
