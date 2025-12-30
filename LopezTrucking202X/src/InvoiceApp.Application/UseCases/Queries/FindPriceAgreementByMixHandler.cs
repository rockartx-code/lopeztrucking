using InvoiceApp.Application.DTOs;
using InvoiceApp.Application.Interfaces;
using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Application.UseCases.Queries;

public sealed class FindPriceAgreementByMixHandler
{
    private readonly IFingerprintService _fingerprintService;
    private readonly IPriceAgreementRepository _priceAgreementRepository;

    public FindPriceAgreementByMixHandler(
        IFingerprintService fingerprintService,
        IPriceAgreementRepository priceAgreementRepository)
    {
        _fingerprintService = fingerprintService;
        _priceAgreementRepository = priceAgreementRepository;
    }

    public async Task<PriceAgreementDto?> HandleAsync(
        FindPriceAgreementByMix query,
        CancellationToken cancellationToken = default)
    {
        var fingerprint = _fingerprintService.CreateMixFingerprint(query.MixIds, query.UseLegacyFormat);

        var agreement = await _priceAgreementRepository.FindByMixAsync(
            query.CompanyId,
            fingerprint.FingerprintHash.Value,
            query.AsOfDate,
            cancellationToken);

        return agreement is null ? null : MapToDto(agreement);
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
