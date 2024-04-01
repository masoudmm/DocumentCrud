using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.Contracts.Persistence;
using DocumentCrud.Domain.Entities;
using MediatR;

namespace DocumentCrud.Application.Features.Commands.Create;

public record CreateInvoiceCommand(string Number,
        string ExternalInvoiceNumber,
        AccountingDocumentStatus Status,
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

    public async Task<DocumentDto> Handle(CreateInvoiceCommand invoice,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(invoice, nameof(invoice));

        var newInvoice = Invoice.CreateNewInvoice(invoice.Number,
        invoice.ExternalInvoiceNumber,
        invoice.Status,
        invoice.TotalAmount);

        foreach (var dependentCredit in invoice.DependentCreditNotes)
        {
            newInvoice.AddDependentCredit(dependentCredit.Number,
                dependentCredit.ExternalCreditNumber,
                dependentCredit.Status,
                dependentCredit.TotalAmount);
        }

        await _unitOfWork.Invoices
            .AddAsync(newInvoice);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<DocumentDto>(newInvoice);
    }
}
