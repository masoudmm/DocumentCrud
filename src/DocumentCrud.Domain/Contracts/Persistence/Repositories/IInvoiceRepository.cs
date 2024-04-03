using DocumentCrud.Domain.Entities;
using DocumentCrud.Domain.InvoiceAggregate;

namespace DocumentCrud.Domain.Contracts.Persistence.Repositories;

public interface IInvoiceRepository : IRepository<Invoice>
{
}