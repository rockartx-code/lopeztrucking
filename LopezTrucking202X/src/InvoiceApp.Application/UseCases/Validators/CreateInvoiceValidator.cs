using FluentValidation;
using InvoiceApp.Application.UseCases.Commands;

namespace InvoiceApp.Application.UseCases.Validators;

public sealed class CreateInvoiceValidator : AbstractValidator<CreateInvoice>
{
    public CreateInvoiceValidator()
    {
        RuleFor(command => command.CompanyId).NotEmpty();
        RuleFor(command => command.InvoiceNumber).NotEmpty();
        RuleFor(command => command.InvoiceDate).NotEqual(default(DateOnly));
    }
}
