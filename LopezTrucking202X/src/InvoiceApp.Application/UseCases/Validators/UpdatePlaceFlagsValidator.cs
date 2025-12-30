using FluentValidation;
using InvoiceApp.Application.UseCases.Commands;

namespace InvoiceApp.Application.UseCases.Validators;

public sealed class UpdatePlaceFlagsValidator : AbstractValidator<UpdatePlaceFlags>
{
    public UpdatePlaceFlagsValidator()
    {
        RuleFor(command => command.PlaceId).NotEmpty();
        RuleFor(command => command)
            .Must(command => command.IsCompany || command.IsFrom || command.IsTo)
            .WithMessage("At least one place flag must be selected.");
    }
}
