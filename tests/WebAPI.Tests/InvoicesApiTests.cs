using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Features.Commands.Create;
using DocumentCrud.Domain.BaseEntities;
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
    public async Task PostInvoice_ReturnsCreated_WithValidCommand()
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
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var createdDto = await response.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_Wrong_Invoice_Should_Fail_With_Bad_Request()
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
        var createdDto = await response.Content.ReadFromJsonAsync<DocumentDto>();
        createdDto.Should().NotBeNull();
    }


    [Fact]
    public async Task Post_Wrong_Invoice_Should_Fail()
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
}