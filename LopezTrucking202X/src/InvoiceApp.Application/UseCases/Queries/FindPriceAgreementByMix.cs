namespace InvoiceApp.Application.UseCases.Queries;

public sealed record FindPriceAgreementByMix(
    Guid CompanyId,
    string MixName,
    DateOnly AsOfDate);
