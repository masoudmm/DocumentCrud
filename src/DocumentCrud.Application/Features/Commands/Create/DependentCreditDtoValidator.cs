using DocumentCrud.Application.Dtos;
using FluentValidation;

namespace DocumentCrud.Application.Features.Commands.Create;

public class DependentCreditDtoValidator : AbstractValidator<DependentCreditNoteDto>
{
    public DependentCreditDtoValidator()
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

        RuleFor(c => c.TotalAmount)
            .NotNull()
            .NotEmpty()
            .LessThan(0);
    }
}
