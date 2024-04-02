using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.Contracts.Persistence;
using MediatR;

namespace DocumentCrud.Application.Features.Queries;

public record GetIndependentCreditByIdQuery(int Id) : IRequest<DocumentDto>;

public class GetIndependentCreditByIdQueryHandler : IRequestHandler<GetIndependentCreditByIdQuery, DocumentDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetIndependentCreditByIdQueryHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(
        GetIndependentCreditByIdQuery request,
        CancellationToken cancellationToken)
    {
        var credit = _unitOfWork.IndependentCreditNotes
            .GetByIdAsync(request.Id);
        
        return _mapper.Map<DocumentDto>(credit);
    }
}
