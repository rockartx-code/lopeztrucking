using InvoiceApp.Application.Models;

namespace InvoiceApp.Application.UseCases.Queries;

public sealed record FindPriceAgreementByMix(
    Guid CompanyId,
    IReadOnlyCollection<MixFingerprintId> MixIds,
    DateOnly AsOfDate,
    bool UseLegacyFormat = false);
