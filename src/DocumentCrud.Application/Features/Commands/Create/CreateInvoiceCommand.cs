﻿using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.Contracts.Persistence;
using DocumentCrud.Domain.InvoiceAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentCrud.Application.Features.Commands.Create;

public record CreateInvoiceCommand(string Number,
        string ExternalInvoiceNumber,
        decimal TotalAmount,
        IReadOnlyList<DependentCreditNoteDto> DependentCreditNotes) : IRequest<DocumentDto>;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, DocumentDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvoiceCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(CreateInvoiceCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var newInvoice = new Invoice(request.Number,
        request.ExternalInvoiceNumber,
        request.TotalAmount);

        foreach (var dependentCredit in request.DependentCreditNotes)
        {
            newInvoice.AddDependentCredit(_mapper.Map<DependentCreditNote>(dependentCredit));
        }

        await _unitOfWork.Invoices
            .AddAsync(newInvoice);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<DocumentDto>(newInvoice);
    }
}
