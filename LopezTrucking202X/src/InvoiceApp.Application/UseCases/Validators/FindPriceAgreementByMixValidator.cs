using FluentValidation;
using InvoiceApp.Application.UseCases.Queries;

namespace InvoiceApp.Application.UseCases.Validators;

public sealed class FindPriceAgreementByMixValidator : AbstractValidator<FindPriceAgreementByMix>
{
    public FindPriceAgreementByMixValidator()
    {
        RuleFor(query => query.CompanyId).NotEmpty();
        RuleFor(query => query.MixName).NotEmpty();
        RuleFor(query => query.AsOfDate).NotEqual(default(DateOnly));
    }
}
