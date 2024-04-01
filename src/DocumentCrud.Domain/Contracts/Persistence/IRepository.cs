namespace DocumentCrud.Domain.Contracts.Persistence;

public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task Update(T entity);
    int Remove(T entity);
}