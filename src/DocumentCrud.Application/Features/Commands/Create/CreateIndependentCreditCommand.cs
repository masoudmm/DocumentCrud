using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.Contracts.Persistence;
using DocumentCrud.Domain.Entities;
using MediatR;

namespace DocumentCrud.Application.Features.Commands.Create;

public record CreateIndependentCreditCommand(string Number,
        string ExternalInvoiceNumber,
        AccountingDocumentStatus Status,
        decimal TotalAmount) : IRequest<DocumentDto>;

public class CreateIndependentCreditCommandHandler : IRequestHandler<CreateIndependentCreditCommand, DocumentDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateIndependentCreditCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(CreateIndependentCreditCommand invoice,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(invoice, nameof(invoice));

        var newInvoice = Invoice.CreateNewInvoice(invoice.Number,
        invoice.ExternalInvoiceNumber,
        invoice.Status,
        invoice.TotalAmount);

        await _unitOfWork.Invoices
            .AddAsync(newInvoice);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<DocumentDto>(newInvoice);
    }
}
