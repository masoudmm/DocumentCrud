using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Exceptions;
using DocumentCrud.Domain.Contracts.Persistence;
using DocumentCrud.Domain.Entities;
using MediatR;

namespace DocumentCrud.Application.Features.Commands.Edit;

public record EditInvoiceCommand(int Id,
    string Number,
    string ExternalInvoiceNumber,
    AccountingDocumentStatus Status,
    decimal TotalAmount,
    IReadOnlyList<DependentCreditNoteDto> DependentCreditNotes) : IRequest<DocumentDto>;

public class EditInvoiceCommandHandler : IRequestHandler<EditInvoiceCommand, DocumentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditInvoiceCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(EditInvoiceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var invoiceToEdit = await _unitOfWork.Invoices
            .GetByIdAsync(request.Id);
        if (invoiceToEdit is null)
        {
            throw new DbEntityNotFoundException("Invoice",
                request.Id);
        }

        invoiceToEdit.Edit(request.Number,
            request.ExternalInvoiceNumber,
            request.Status,
            request.TotalAmount,
            request.DependentCreditNotes
            .Select(dcn => _mapper.Map<DependentCreditNote>(dcn))
            .ToList());
        
        await _unitOfWork.CommitAsync();

        return _mapper.Map<DocumentDto>(invoiceToEdit);
    }
}

