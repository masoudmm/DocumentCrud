using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Features.Commands.Create;
using DocumentCrud.Domain.BaseEntities;
using FluentValidation.TestHelper;

namespace Application.Tests.Validators;

public class CreateInvoiceCommandValidatorTests
{
    private readonly CreateInvoiceCommandValidator _validator;

    public CreateInvoiceCommandValidatorTests()
    {
        _validator = new CreateInvoiceCommandValidator();
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenNumberIsInvalid()
    {
        // Arrange
        var command = new CreateInvoiceCommand("123456789000000",
            "123456inv1",
            AccountingDocumentStatus.WaitingForApproval,
            100,
            Enumerable.Empty<DependentCreditNoteDto>().ToList());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Number);
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenExternalCreditNumberIsInvalid()
    {
        // Arrange
        var command = new CreateInvoiceCommand("1234567890",
            "123456inv111111",
            AccountingDocumentStatus.WaitingForApproval,
            100,
            Enumerable.Empty<DependentCreditNoteDto>().ToList());
        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.ExternalInvoiceNumber);
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateInvoiceCommand("1234567890",
            "1234567890",
            AccountingDocumentStatus.WaitingForApproval, 
            100,
            Enumerable.Empty<DependentCreditNoteDto>().ToList());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}