using DocumentCrud.Application.Dtos;
using FluentValidation;

namespace DocumentCrud.Application.Features.Commands.Create;

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(c => c.Number)
            .NotNull()
            .NotEmpty()
            .Length(10)
            .Must(x => int.TryParse(x, out var val) &&
            val > 0).WithMessage("Invalid Number.");

        RuleFor(c => c.ExternalInvoiceNumber)
            .NotNull()
            .NotEmpty()
            .Length(10);

        RuleFor(c => c.TotalAmount)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);

        RuleForEach(c => c.DependentCreditNotes)
            .SetValidator(new DependentCreditDtoValidator());
    }
}
