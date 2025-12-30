using InvoiceApp.Application.Models;

namespace InvoiceApp.Application.UseCases.Commands;

public sealed record SavePriceAgreementFromMix(
    Guid CompanyId,
    string MixName,
    IReadOnlyCollection<MixFingerprintId> MixIds,
    DateOnly EffectiveDate,
    decimal EmptyUnitPrice,
    decimal BaseRate,
    bool UseLegacyFormat = false);
