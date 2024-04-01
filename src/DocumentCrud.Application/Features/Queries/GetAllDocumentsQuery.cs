using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.Contracts.Persistence;
using MediatR;

namespace DocumentCrud.Application.Features.Queries;

public class GetAllDocumentsQuery : IRequest<IReadOnlyList<DocumentDto>> { };

public class GetAllDocumentsQueryHandler : IRequestHandler<GetAllDocumentsQuery, IReadOnlyList<DocumentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllDocumentsQueryHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<DocumentDto>> Handle(
        GetAllDocumentsQuery request,
        CancellationToken cancellationToken)
    {
        var documents = new List<DocumentDto>();
        
        var invoices = await _unitOfWork.Invoices
            .GetAllAsync();
        documents.AddRange(invoices.Select(i => _mapper.Map<DocumentDto>(i)));
        
        var independentCredits = await _unitOfWork.IndependentCreditNotes
            .GetAllAsync();
        documents.AddRange(independentCredits.Select(ic => _mapper.Map<DocumentDto>(ic)));

        return documents;
    }
}
