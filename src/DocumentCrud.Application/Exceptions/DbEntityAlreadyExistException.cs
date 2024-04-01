namespace DocumentCrud.Application.Exceptions;

public class DbEntityAlreadyExistException : Exception
{
    public DbEntityAlreadyExistException()
        : base()
    {
    }

    public DbEntityAlreadyExistException(string message)
        : base(message)
    {
    }

    public DbEntityAlreadyExistException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public DbEntityAlreadyExistException(string name, object key)
        : base($"DbEntity {name} with key {key} already exist!")
    {
    }
}
