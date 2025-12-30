using FluentValidation;
using InvoiceApp.Application.UseCases.Commands;

namespace InvoiceApp.Application.UseCases.Validators;

public sealed class AddDetailGroupValidator : AbstractValidator<AddDetailGroup>
{
    public AddDetailGroupValidator()
    {
        RuleFor(command => command.InvoiceId).NotEmpty();
        RuleFor(command => command.Description).NotEmpty();
        RuleFor(command => command.AmountBase).GreaterThanOrEqualTo(0m);
        RuleFor(command => command.EmptiesCount).GreaterThanOrEqualTo(0);
    }
}
