using DocumentCrud.Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DocumentCrud.Application.Extentions;

public static class ServiceCollectionExtentions
{
    private static Assembly applicationAssembly = typeof(ServiceCollectionExtentions).Assembly;

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(applicationAssembly);
        services.AddApplicationAutoMapper();


        services.AddApplicationValidators();

        services.AddMediatR();

        return services;
    }

    private static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(applicationAssembly);

        return services;
    }

    private static IServiceCollection AddApplicationValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(applicationAssembly);

        return services;
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(applicationAssembly);
            config.AddOpenBehavior(typeof(DocumentValidationBehavior<,>));
        });

        return services;
    }
}
