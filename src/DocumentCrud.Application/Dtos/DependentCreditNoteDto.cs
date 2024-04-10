using DocumentCrud.Domain.BaseEntities;

namespace DocumentCrud.Application.Dtos;

public class DependentCreditNoteDto
{
    public int Id { get; set; }

    public string Number { get; set; }

    public string ExternalCreditNumber { get; set; }

    public AccountingDocumentStatus Status { get; set; }

    public decimal TotalAmount { get; set; }
}
