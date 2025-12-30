using FluentValidation;
using InvoiceApp.Application.UseCases.Commands;

namespace InvoiceApp.Application.UseCases.Validators;

public sealed class UpdateDetailGroupValidator : AbstractValidator<UpdateDetailGroup>
{
    public UpdateDetailGroupValidator()
    {
        RuleFor(command => command.DetailGroupId).NotEmpty();
        RuleFor(command => command.Description).NotEmpty();
        RuleFor(command => command.AmountBase).GreaterThanOrEqualTo(0m);
        RuleFor(command => command.EmptiesCount).GreaterThanOrEqualTo(0);
    }
}
