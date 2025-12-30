namespace InvoiceApp.Application.UseCases.Commands;

public sealed record SavePriceAgreementFromMix(
    Guid CompanyId,
    string MixName,
    DateOnly EffectiveDate,
    decimal EmptyUnitPrice,
    decimal BaseRate);
