using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Features.Commands.Create;
using DocumentCrud.Application.Features.Commands.Edit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace WebAPI.Tests.Validators;

public class InvoicesApiTests : IClassFixture<WebApplicationFactory<DocumentCrud.WebAPI.Program>>
{
    private readonly WebApplicationFactory<DocumentCrud.WebAPI.Program> _factory;
    private readonly HttpClient _client;

    public InvoicesApiTests(WebApplicationFactory<DocumentCrud.WebAPI.Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Invoice_Should_Succeed()
    {
        // Arrange
        var command = new CreateInvoiceCommand(
                "1234567890",
                "123456inv1",
                100m,
                Enumerable.Empty<DependentCreditNoteDto>().ToList());

        // Act
        var response = await _client.PostAsJsonAsync("/api/Invoices", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdDto = await response.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_Invoice_With_Dependent_Credit_Should_Succeed()
    {
        // Arrange
        var command = new CreateInvoiceCommand(
                "1234567890",
                "123456inv1",
                200m,
                new List<DependentCreditNoteDto>() 
                { 
                    new()
                    {
                        Number = "1234567890",
                        ExternalCreditNumber = "1234567dc1",
                        TotalAmount = -100m
                    },
                    new()
                    {
                        Number = "1234567891",
                        ExternalCreditNumber = "1234567dc2",
                        TotalAmount = -100m
                    }
                });

        // Act
        var response = await _client.PostAsJsonAsync("/api/Invoices", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdDto = await response.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
    }


    [Fact]
    public async Task Create_Invoice_With_Dependent_Credit_Greater_TotalAmount_Should_Fail()
    {
        // Arrange
        var command = new CreateInvoiceCommand(
                "1234567890",
                "123456inv1",
                200m,
                new List<DependentCreditNoteDto>()
                {
                    new()
                    {
                        Number = "1234567890",
                        ExternalCreditNumber = "1234567dc1",
                        TotalAmount = -100m
                    },
                    new()
                    {
                        Number = "1234567891",
                        ExternalCreditNumber = "1234567dc2",
                        TotalAmount = -101m
                    }
                });

        // Act
        var response = await _client.PostAsJsonAsync("/api/Invoices", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }


    [Fact]
    public async Task Create_Invoice_With_Dependent_Credit_Longer_Number_Should_Fail_With_bad_Request()
    {
        // Arrange
        var command = new CreateInvoiceCommand(
                "1234567890",
                "123456inv1",
                200m,
                new List<DependentCreditNoteDto>()
                {
                    new()
                    {
                        Number = "12345678900",
                        ExternalCreditNumber = "1234567dc1",
                        TotalAmount = -100m
                    },
                    new()
                    {
                        Number = "1234567891",
                        ExternalCreditNumber = "1234567dc2",
                        TotalAmount = -101m
                    }
                });

        // Act
        var response = await _client.PostAsJsonAsync("/api/Invoices", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Invoice_With_More_Then_10_Number_Digits_Should_Fail()
    {
        // Arrange
        var command = new CreateInvoiceCommand(
                "12345678900000000000000",
                "123456inv1",
                100m,
                Enumerable.Empty<DependentCreditNoteDto>().ToList());

        // Act
        var response = await _client.PostAsJsonAsync("/api/Invoices", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Invoice_With_Same_Intenal_External_Number_Should_Fail()
    {
        // Arrange
        var command = new CreateInvoiceCommand(
            "1234567890",
            "1234567890",
            100m,
            Enumerable.Empty<DependentCreditNoteDto>().ToList());

        // Act
        var response = await _client.PostAsJsonAsync("/api/Invoices", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var createdDto = await response.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
    }


    [Fact]
    public async Task Edit_Invoice_Should_Succeed()
    {
        // Arrange
        var createInvoiceCommand = new CreateInvoiceCommand(
                "1234567890",
                "123456inv1",
                100m,
                Enumerable.Empty<DependentCreditNoteDto>().ToList());

        var createdResponse = await _client.PostAsJsonAsync("/api/Invoices", createInvoiceCommand);
        var documentDto = await createdResponse.Content.ReadFromJsonAsync<DocumentDto>();

        var editInvoiceCommand = new EditInvoiceCommand(
            documentDto.Id,
            "1234567888",
            createInvoiceCommand.ExternalInvoiceNumber,
            DocumentCrud.Domain.BaseEntities.AccountingDocumentStatus.WaitingForApproval,
            createInvoiceCommand.TotalAmount,
            createInvoiceCommand.DependentCreditNotes);

        // Act
        var editResponse = await _client.PutAsJsonAsync($"/api/Invoices/{documentDto.Id}", editInvoiceCommand);

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        editResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var editedDocument = await editResponse.Content.ReadFromJsonAsync<DocumentDto>();
        documentDto.Should().NotBeNull();
        editedDocument.Number.Should().Be("1234567888");
    }

    [Fact]
    public async Task Edit_Invoice_Should_With_Wrong_Total_Amount_Should_Fail()
    {
        // Arrange
        var createInvoiceCommand = new CreateInvoiceCommand(
                "1234567890",
                "123456inv1",
                100m,
                Enumerable.Empty<DependentCreditNoteDto>().ToList());

        var createdResponse = await _client.PostAsJsonAsync("/api/Invoices", createInvoiceCommand);
        var documentDto = await createdResponse.Content.ReadFromJsonAsync<DocumentDto>();

        var editInvoiceCommand = new EditInvoiceCommand(
            documentDto.Id,
            createInvoiceCommand.Number,
            createInvoiceCommand.ExternalInvoiceNumber,
            DocumentCrud.Domain.BaseEntities.AccountingDocumentStatus.WaitingForApproval,
            -100m,
            createInvoiceCommand.DependentCreditNotes);

        // Act
        var editResponse = await _client.PutAsJsonAsync($"/api/Invoices/{documentDto.Id}", editInvoiceCommand);

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdDto = await editResponse.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
        editResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task Delete_Should_Succeed()
    {
        // Arrange
        var createInvoiceCommand = new CreateInvoiceCommand(
                "1234567890",
                "123456inv1",
                100m,
                Enumerable.Empty<DependentCreditNoteDto>().ToList());

        var createdResponse = await _client.PostAsJsonAsync("/api/Invoices", createInvoiceCommand);
        var documentDto = await createdResponse.Content.ReadFromJsonAsync<DocumentDto>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/Invoices/{documentDto.Id}");

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var deletedDocumentId = await deleteResponse.Content.ReadAsStringAsync();
        documentDto.Should().NotBeNull();
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        deletedDocumentId.Should().Be(documentDto.Id.ToString());
    }
}