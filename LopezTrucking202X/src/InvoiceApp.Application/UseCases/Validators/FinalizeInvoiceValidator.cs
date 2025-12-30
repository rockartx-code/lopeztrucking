using FluentValidation;
using InvoiceApp.Application.UseCases.Commands;

namespace InvoiceApp.Application.UseCases.Validators;

public sealed class FinalizeInvoiceValidator : AbstractValidator<FinalizeInvoice>
{
    public FinalizeInvoiceValidator()
    {
        RuleFor(command => command.InvoiceId).NotEmpty();
    }
}
