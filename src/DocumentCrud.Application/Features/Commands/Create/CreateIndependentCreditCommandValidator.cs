using FluentValidation;

namespace DocumentCrud.Application.Features.Commands.Create;

public class CreateIndependentCreditCommandValidator : AbstractValidator<CreateIndependentCreditCommand>
{
    public CreateIndependentCreditCommandValidator()
    {
        RuleFor(c => c.Number)
            .NotNull()
            .NotEmpty()
            .Length(10)
            .Must(x => int.TryParse(x, out var val) &&
            val > 0).WithMessage("Invalid Number.");

        RuleFor(c => c.ExternalCreditNumber)
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
