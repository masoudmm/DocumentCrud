using DocumentCrud.Domain.Entities;

namespace DocumentCrud.Domain.Contracts.Persistence.Repositories;

public interface IRepository<T> where T : class, IAggregateRoot
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task Update(T entity);
    int Remove(T entity);
}