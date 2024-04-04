using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Features.Commands.Create;
using DocumentCrud.Application.Features.Commands.Edit;
using DocumentCrud.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DocumentCrud.WebAPI.Extentions;

public static class WebApplicationExtentions
{
    public static void MapDocumentrEndPoints(this WebApplication app)
    {
        app.MapGet("/api/Documents", (ISender sender,
                CancellationToken ct) =>
            sender.Send(new GetAllDocumentsQuery(), ct))
                .Produces<DocumentDto>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapGet("/api/Invoices/{id:int}", (ISender sender,
            [FromRoute] int id,
            CancellationToken ct) => sender.Send(new GetInvoiceByIdQuery(id), ct))
        .Produces<DocumentDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/IndependentCredits/{id:int}", (ISender sender,
            [FromRoute] int id,
            CancellationToken ct) => sender.Send(new GetInvoiceByIdQuery(id), ct))
        .Produces<DocumentDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPost("/api/Invoices", (ISender sender,
            CreateInvoiceCommand command,
            CancellationToken ct) =>
        sender.Send(command, ct))
        .Produces<DocumentDto>(statusCode: StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPost("/api/IndependentCredits", (ISender sender,
            CreateIndependentCreditCommand command,
            CancellationToken ct) =>
        sender.Send(command, ct))
        .Produces<DocumentDto>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPut("/api/Invoices/{id:int}", (ISender sender,
            EditInvoiceCommand command,
            CancellationToken ct) =>
        sender.Send(command, ct))
        .Produces<DocumentDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapPut("/api/IndependentCredits/{id:int}", (ISender sender,
            EditIndependentCreditCommand command,
            CancellationToken ct) =>
        sender.Send(command, ct))
        .Produces<DocumentDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/Invoices/{id:int}", (ISender sender,
            [FromRoute] int id,
            CancellationToken ct) =>
        sender.Send(new DeleteInvoiceCommand(id), ct))
        .Produces<int>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/IndependentCredits/{id:int}", (ISender sender,
            [FromRoute] int id,
            CancellationToken ct) =>
        sender.Send(new DeleteIndependentCreditCommand(id), ct))
        .Produces<int>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}

