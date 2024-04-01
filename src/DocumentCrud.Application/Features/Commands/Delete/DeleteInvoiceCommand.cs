using AutoMapper;
using DocumentCrud.Domain.Contracts.Persistence;
using MediatR;

namespace DocumentCrud.Application.Features.Commands.Edit;

public record DeleteInvoiceCommand(int Id) : IRequest<int>;

public class DeleteInvoiceCommandHandler : IRequestHandler<DeleteInvoiceCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeleteInvoiceCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DeleteInvoiceCommand request, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var invoiceToDelete = await _unitOfWork.Invoices
            .GetByIdAsync(request.Id);

        invoiceToDelete.Delete();

        var deletedInvoiceId = _unitOfWork.Invoices
            .Remove(invoiceToDelete);

        await _unitOfWork.CommitAsync();

        return deletedInvoiceId;
    }
}

