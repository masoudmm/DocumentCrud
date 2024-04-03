using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Exceptions;
using DocumentCrud.Application.Features.Commands.Create;
using DocumentCrud.Application.Validation;
using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.InvoiceAggregate;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace Application.Tests.Validators;

public class PipelineValidatorTests
{
    [Fact]
    public async Task Handle_GivenInvalidRequest_ShouldThrowValidationFailureException()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<CreateInvoiceCommand>>();
        var request = new CreateInvoiceCommand("1234567890",
            "123456inv1",
            100m,
            Enumerable.Empty<DependentCreditNoteDto>().ToList());

        var validationResult = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(Invoice.Number),
                    "Invalid Number")
            });

        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateInvoiceCommand>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var validators = new List<IValidator<CreateInvoiceCommand>> { mockValidator.Object };
        var behavior = new DocumentValidationBehavior<CreateInvoiceCommand, CreateInvoiceCommand>(validators);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailureException>(() =>
            behavior.Handle(request,
            default,
            CancellationToken.None));
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ShouldPassToNextDelegate()
    {
        // Arrange
        var mockValidator = new Mock<IValidator<CreateInvoiceCommand>>();
        var request = new CreateInvoiceCommand("1234567890",
                        "123456inv1",
                        100m,
                        Enumerable.Empty<DependentCreditNoteDto>().ToList());

        var validationResult = new ValidationResult();

        mockValidator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateInvoiceCommand>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var validators = new List<IValidator<CreateInvoiceCommand>> { mockValidator.Object };
        var behavior = new DocumentValidationBehavior<CreateInvoiceCommand, CreateInvoiceCommand>(validators);

        var response = request;
        RequestHandlerDelegate<CreateInvoiceCommand> next = () => Task.FromResult(response);

        // Act
        var result = await behavior.Handle(request,
            next,
            CancellationToken.None);

        // Assert
        Assert.Equal(response, result);
    }
}
