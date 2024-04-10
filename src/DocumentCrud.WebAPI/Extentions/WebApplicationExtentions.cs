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
        #region Get Endpoint Mappings

        app.MapGet("/api/Documents", (ISender sender,
                CancellationToken ct) =>
            sender.Send(new GetAllDocumentsQuery(), ct))
                .WithName("GetAllDocuments")
                .ProducesGet<DocumentDto[]>();

        app.MapGet("/api/Invoices/{id:int}", (ISender sender,
                [FromRoute] int id,
                CancellationToken ct) =>
            sender.Send(new GetInvoiceByIdQuery(id), ct))
                .WithName("GetInvoiceById")
                .ProducesGet<DocumentDto>();

        app.MapGet("/api/IndependentCredits/{id:int}", (ISender sender,
                [FromRoute] int id,
                CancellationToken ct) =>
            sender.Send(new GetIndependentCreditByIdQuery(id), ct))
                .WithName("GetIndependentCreditById")
                .ProducesGet<DocumentDto>();

        #endregion

        #region Post Endpoint Mappings

        app.MapPost("/api/Invoices", (ISender sender,
            CreateInvoiceCommand command, CancellationToken ct) =>
        sender.Send(command, ct))
            .WithName("CreateInvoice")
            .ProducesPost();

        app.MapPost("/api/IndependentCredits", (ISender sender,
            CreateIndependentCreditCommand command, CancellationToken ct) =>
        sender.Send(command, ct))
            .WithName("CreateIndependentCredit")
            .ProducesPost();

        #endregion

        #region Put Endpoint Mappings

        app.MapPut("/api/Invoices/{id:int}", (ISender sender, 
            EditInvoiceCommand command, 
            CancellationToken ct) => 
        sender.Send(command, ct))
            .WithName("EditInvoice")
            .ProducesPut();

        app.MapPut("/api/IndependentCredits/{id:int}", (ISender sender,
            EditIndependentCreditCommand command,
            CancellationToken ct) =>
        sender.Send(command, ct))
            .WithName("EditIndependentCredit")
            .ProducesPut();

        #endregion

        #region Delete Endpoint Mappings

        app.MapDelete("/api/Invoices/{id:int}", (ISender sender,
            [FromRoute] int id,
            CancellationToken ct) =>
        sender.Send(new DeleteInvoiceCommand(id), ct))
            .WithName("DeleteInvoice")
            .ProducesDelete();

        app.MapDelete("/api/IndependentCredits/{id:int}", (ISender sender,
            [FromRoute] int id,
            CancellationToken ct) =>
        sender.Send(new DeleteIndependentCreditCommand(id), ct))
            .WithName("DeleteIndependentCredit")
            .ProducesDelete();

        #endregion
    }
}

