using AutoMapper;
using DocumentCrud.Domain.Contracts.Persistence;
using MediatR;

namespace DocumentCrud.Application.Features.Commands.Edit;

public record DeleteIndependentCreditCommand(int Id) : IRequest<int>;

public class DeleteIndependentCreditCommandHandler : IRequestHandler<DeleteIndependentCreditCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteIndependentCreditCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DeleteIndependentCreditCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var independentCreditToDelete = await _unitOfWork.IndependentCreditNotes
            .GetByIdAsync(request.Id);

        independentCreditToDelete.Delete();

        var deletedIndependentCreditNoteId = _unitOfWork.IndependentCreditNotes
            .Remove(independentCreditToDelete);

        await _unitOfWork.CommitAsync();

        return deletedIndependentCreditNoteId;
    }
}

