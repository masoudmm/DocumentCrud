using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.Contracts.Persistence;
using MediatR;

namespace DocumentCrud.Application.Features.Queries;

public record GetDocumentByIdQuery(int Id,
    DocumentType DocumentType) : IRequest<DocumentDto>;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto>
{
    private readonly IMapper _mapper;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IIndependentCreditRepository _independentCreditRepository;

    public GetInvoiceByIdQueryHandler(IMapper mapper,
        IIndependentCreditRepository independentCreditRepository,
        IInvoiceRepository invoiceRepository)
    {
        _mapper = mapper;
        _independentCreditRepository = independentCreditRepository;
        _invoiceRepository = invoiceRepository;
    }

    public async Task<DocumentDto> Handle(
        GetDocumentByIdQuery request,
        CancellationToken cancellationToken)
    {
        DocumentDto documentDto = null;
        if (request.DocumentType == DocumentType.Invoice)
        {
            documentDto = _mapper
                .Map<DocumentDto>(await _invoiceRepository.GetByIdAsync(request.Id));
        }
        else if (request.DocumentType == DocumentType.IndependentCredit)
        {
            documentDto = _mapper
                .Map<DocumentDto>(await _independentCreditRepository.GetByIdAsync(request.Id));
        }

        return documentDto;
    }
}
