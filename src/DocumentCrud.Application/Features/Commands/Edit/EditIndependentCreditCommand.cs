﻿using AutoMapper;
using DocumentCrud.Application.Dtos;
using DocumentCrud.Application.Exceptions;
using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.Contracts.Persistence;
using MediatR;

namespace DocumentCrud.Application.Features.Commands.Edit;

public record EditIndependentCreditCommand(int Id,
    string Number,
    string ExternalCreditNumber,
    AccountingDocumentStatus Status,
    decimal TotalAmount) : IRequest<DocumentDto>;

public class EditIndependentCreditCommandHandler : IRequestHandler<EditIndependentCreditCommand, DocumentDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public EditIndependentCreditCommandHandler(IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<DocumentDto> Handle(EditIndependentCreditCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        var independentCreditNoteToEdit = await _unitOfWork.IndependentCreditNotes
            .GetByIdAsync(request.Id);
        if (independentCreditNoteToEdit is null)
        {
            throw new DbEntityNotFoundException("ExternalCreditNumber",
                request.Id);
        }

        independentCreditNoteToEdit.Edit(request.Number,
            request.ExternalCreditNumber,
            request.Status,
            request.TotalAmount);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<DocumentDto>(independentCreditNoteToEdit);
    }
}

