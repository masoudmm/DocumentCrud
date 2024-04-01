using DocumentCrud.Domain.Entities;

namespace DocumentCrud.Application.Dtos;

public class DocumentDto
{
    public int Id { get; set; }

    public string Number { get; set; }
    public string ExternalNumber { get; set; }

    public AccountingDocumentStatus Status { get; set; }

    public decimal TotalAmount { get; set; }

    public DocumentType documentType { get; set; }

    public IReadOnlyList<DependentCreditNoteDto> DependentCreditNoteDtos { get; set; }

}

public enum DocumentType
{
    Invoice,
    IndependentCredit,
    DependentCredit
}