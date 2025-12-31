using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.UseCases.Commands;

public sealed class SavePriceAgreementFromMixHandler
{
    private readonly IFingerprintService _fingerprintService;
    private readonly IPriceAgreementRepository _priceAgreementRepository;

    public SavePriceAgreementFromMixHandler(
        IFingerprintService fingerprintService,
        IPriceAgreementRepository priceAgreementRepository)
    {
        _fingerprintService = fingerprintService;
        _priceAgreementRepository = priceAgreementRepository;
    }

    public async Task<PriceAgreementDto> HandleAsync(
        SavePriceAgreementFromMix command,
        CancellationToken cancellationToken = default)
    {
        var fingerprint = _fingerprintService.CreateMixFingerprint(command.MixIds, command.UseLegacyFormat);
        var itemTypes = command.MixIds
            .Select(id => id.ItemType)
            .Distinct()
            .OrderBy(type => type)
            .ToList();

        var agreement = await _priceAgreementRepository.FindByMixAndItemsAsync(
            command.CompanyId,
            fingerprint.FingerprintHash.Value,
            itemTypes,
            command.EffectiveDate,
            cancellationToken);

        if (agreement is null)
        {
            agreement = new PriceAgreement
            {
                CompanyId = command.CompanyId,
                MixName = command.MixName,
                FingerprintText = fingerprint.FingerprintText,
                FingerprintHash = fingerprint.FingerprintHash.Value,
                EffectiveDate = command.EffectiveDate,
                IsActive = true,
                Items = itemTypes
                    .Select(type => new PriceAgreementItem
                    {
                        ItemType = type,
                        EmptyUnitPrice = command.EmptyUnitPrice,
                        BaseRate = command.BaseRate
                    })
                    .ToList()
            };

            await _priceAgreementRepository.AddAsync(agreement, cancellationToken);
        }
        else
        {
            agreement.MixName = command.MixName;
            agreement.FingerprintText = fingerprint.FingerprintText;
            agreement.FingerprintHash = fingerprint.FingerprintHash.Value;
            agreement.IsActive = true;

            foreach (var item in agreement.Items)
            {
                item.EmptyUnitPrice = command.EmptyUnitPrice;
                item.BaseRate = command.BaseRate;
            }

            await _priceAgreementRepository.UpdateAsync(agreement, cancellationToken);
        }

        await _priceAgreementRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(agreement);
    }

    private static PriceAgreementDto MapToDto(PriceAgreement agreement)
    {
        return new PriceAgreementDto(
            agreement.Id,
            agreement.CompanyId,
            agreement.MixName,
            agreement.EffectiveDate,
            agreement.Items
                .OrderBy(item => item.ItemType)
                .Select(item => new PriceAgreementItemDto(
                    item.ItemType,
                    item.EmptyUnitPrice,
                    item.BaseRate))
                .ToList());
    }
}
