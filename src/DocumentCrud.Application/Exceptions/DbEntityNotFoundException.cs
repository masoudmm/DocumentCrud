namespace DocumentCrud.Application.Exceptions;

public class DbEntityNotFoundException : Exception
{
    public DbEntityNotFoundException()
        : base()
    {
    }

    public DbEntityNotFoundException(string message)
        : base(message)
    {
    }

    public DbEntityNotFoundException(string message, 
        Exception innerException)
        : base(message, innerException)
    {
    }

    public DbEntityNotFoundException(string name, 
        object key)
        : base($"DbEntity {name} with key {key} not found!")
    {
    }
}
