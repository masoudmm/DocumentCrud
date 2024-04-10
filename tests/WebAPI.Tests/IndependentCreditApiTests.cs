using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Features.Commands.Create;
using DocumentCrud.Application.Features.Commands.Edit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace WebAPI.Tests.Validators;

public class IndependentCreditApiTests : IClassFixture<WebApplicationFactory<DocumentCrud.WebAPI.Program>>
{
    private readonly WebApplicationFactory<DocumentCrud.WebAPI.Program> _factory;
    private readonly HttpClient _client;

    public IndependentCreditApiTests(WebApplicationFactory<DocumentCrud.WebAPI.Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_Should_Succeed()
    {
        // Arrange
        var command = new CreateIndependentCreditCommand(
                "1234567890",
                "123456inv1",
                -100m);

        // Act
        var response = await _client.PostAsJsonAsync("/api/IndependentCredits", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdDto = await response.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
    }

    [Fact]
    public async Task Edit_Should_Succeed()
    {
        // Arrange
        var createCreditCommand = new CreateIndependentCreditCommand(
                "1234567890",
                "123456inv1",
                -100m);

        var createdResponse = await _client.PostAsJsonAsync("/api/IndependentCredits", createCreditCommand);
        var documentDto = await createdResponse.Content.ReadFromJsonAsync<DocumentDto>();

        var editCreditCommand = new EditIndependentCreditCommand(
            documentDto.Id,
            "1234567888",
            createCreditCommand.ExternalCreditNumber,
            DocumentCrud.Domain.BaseEntities.AccountingDocumentStatus.WaitingForApproval,
            createCreditCommand.TotalAmount);

        // Act
        var editResponse = await _client.PutAsJsonAsync($"/api/IndependentCredits/{documentDto.Id}", editCreditCommand);

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
        var createCreditCommand = new CreateIndependentCreditCommand(
                "1234567890",
                "123456inv1",
                -100m);

        var createdResponse = await _client.PostAsJsonAsync("/api/IndependentCredits", createCreditCommand);
        var documentDto = await createdResponse.Content.ReadFromJsonAsync<DocumentDto>();

        var editInvoiceCommand = new EditIndependentCreditCommand(
            documentDto.Id,
            createCreditCommand.Number,
            createCreditCommand.ExternalCreditNumber,
            DocumentCrud.Domain.BaseEntities.AccountingDocumentStatus.WaitingForApproval,
            100m);

        // Act
        var editResponse = await _client.PutAsJsonAsync($"/api/IndependentCredits/{documentDto.Id}", editInvoiceCommand);

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        editResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_With_More_Then_10_Number_Digits_Should_Fail()
    {
        // Arrange
        var command = new CreateIndependentCreditCommand(
                "12345678900000000000000",
                "123456inv1",
                -100m);

        // Act
        var response = await _client.PostAsJsonAsync("/api/IndependentCredits", command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var createdDto = await response.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
    }


    [Fact]
    public async Task Delete_Should_Succeed()
    {
        // Arrange
        var createCreditCommand = new CreateIndependentCreditCommand(
                "1234567890",
                "123456inv1",
                -100m);

        var createdResponse = await _client.PostAsJsonAsync("/api/IndependentCredits", createCreditCommand);
        var documentDto = await createdResponse.Content.ReadFromJsonAsync<DocumentDto>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/IndependentCredits/{documentDto.Id}");

        // Assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var deletedDocumentId = await deleteResponse.Content.ReadAsStringAsync();
        documentDto.Should().NotBeNull();
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        deletedDocumentId.Should().Be(documentDto.Id.ToString());
    }
}