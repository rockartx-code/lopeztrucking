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
        var items = command.MixIds
            .Select(id => id.ItemType)
            .Distinct()
            .OrderBy(type => type)
            .Select(type => new PriceAgreementItem
            {
                ItemType = type,
                EmptyUnitPrice = command.EmptyUnitPrice,
                BaseRate = command.BaseRate
            })
            .ToList();

        var agreement = new PriceAgreement
        {
            CompanyId = command.CompanyId,
            MixName = command.MixName,
            FingerprintText = fingerprint.FingerprintText,
            FingerprintHash = fingerprint.FingerprintHash.Value,
            EffectiveDate = command.EffectiveDate,
            IsActive = true,
            Items = items
        };

        await _priceAgreementRepository.AddAsync(agreement, cancellationToken);
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
