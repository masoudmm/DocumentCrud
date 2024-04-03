﻿using DocumentCrud.Domain.BaseEntities;

namespace DocumentCrud.Application.Dtos;

public class DependentCreditNoteDto
{
    public int Id { get; private set; }

    public string Number { get; protected set; }

    public string ExternalCreditNumber { get; protected set; }

    public AccountingDocumentStatus Status { get; protected set; }

    public decimal TotalAmount { get; protected set; }
}
