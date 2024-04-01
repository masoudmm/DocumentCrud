using FluentValidation.Results;

namespace DocumentCrud.Application.Exceptions;

public class ValidationFailureException : Exception
{
    public ValidationFailureException()
        : base("Validation Failures")
    {
        Failures = new Dictionary<string, string[]>();
    }

    public ValidationFailureException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Failures = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureKeyGroupValue => 
                failureKeyGroupValue.Key, failureKeyValue => failureKeyValue
            .ToArray());
    }

    public IDictionary<string, string[]> Failures { get; }
}
