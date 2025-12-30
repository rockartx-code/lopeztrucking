using FluentValidation;
using InvoiceApp.Application.UseCases.Commands;

namespace InvoiceApp.Application.UseCases.Validators;

public sealed class SavePriceAgreementFromMixValidator : AbstractValidator<SavePriceAgreementFromMix>
{
    public SavePriceAgreementFromMixValidator()
    {
        RuleFor(command => command.CompanyId).NotEmpty();
        RuleFor(command => command.MixName).NotEmpty();
        RuleFor(command => command.EffectiveDate).NotEqual(default(DateOnly));
        RuleFor(command => command.EmptyUnitPrice).GreaterThanOrEqualTo(0m);
        RuleFor(command => command.BaseRate).GreaterThanOrEqualTo(0m);
    }
}
