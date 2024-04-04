using DocumentCrud.Application.Extentions;
using DocumentCrud.Infrastructure.Extentions;
using DocumentCrud.WebAPI.Extentions;
using DocumentCrud.WebAPI.Filters;

namespace DocumentCrud.WebAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();
        
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder => {
                builder.SetIsOriginAllowed(a => a.Contains("localhost"));
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseExceptionFilter();
        app.MapDocumentrEndPoints();

        app.Run();
    }
}