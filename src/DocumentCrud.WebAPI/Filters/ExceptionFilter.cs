using DocumentCrud.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DocumentCrud.Server.Filters;

public static class ExceptionFilter
{
    public static void UseExceptionFilter(this WebApplication app)
    {
        app.UseExceptionHandler(builder
            => builder.Run(async httpContext =>
            {
                var exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error!;
                var result = Results.Problem(statusCode: StatusCodes.Status500InternalServerError);

                switch (exception)
                {
                    case ValidationFailureException validationFailureException:
                        result = Results.ValidationProblem(validationFailureException.Failures);
                        break;

                    case DbEntityNotFoundException dbEntityNotFoundException:
                    case DbEntityAlreadyExistException DbEntityAlreadyExistException:
                        result = Results.Problem(detail: exception.Message, statusCode: StatusCodes.Status500InternalServerError);
                        break;

                    default:
                        result = Results.Problem(detail: "Operation Failed!", statusCode: StatusCodes.Status500InternalServerError);
                        break;
                }

                await result.ExecuteAsync(httpContext);
            }));
    }
}
