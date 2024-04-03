using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.Contracts.Persistence;
using DocumentCrud.Domain.CreditAggregate;
using MediatR;

namespace DocumentCrud.Application.Features.Commands.Create;

public record CreateIndependentCreditCommand(string Number,
        string ExternalCreditNumber,
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

    public async Task<DocumentDto> Handle(CreateIndependentCreditCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var newCredit = IndependentCreditNote.CreateNew(request.Number,
            request.ExternalCreditNumber,
            request.Status,
            request.TotalAmount);

        await _unitOfWork.IndependentCreditNotes
            .AddAsync(newCredit);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<DocumentDto>(newCredit);
    }
}
