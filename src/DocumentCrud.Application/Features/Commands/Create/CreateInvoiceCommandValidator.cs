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

        RuleFor(c => c.Status)
            .NotNull()
            .NotEmpty();

        RuleFor(c => c.TotalAmount)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);
    }
}
