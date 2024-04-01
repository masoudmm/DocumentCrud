using FluentValidation;

namespace DocumentCrud.Application.Features.Commands.Edit;

public class EditIndependentCreditCommandValidator : AbstractValidator<EditIndependentCreditCommand>
{
    public EditIndependentCreditCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);

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
            .LessThan(0);
    }
}
