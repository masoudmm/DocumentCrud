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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseExceptionFilter();
        app.MapDocumentrEndPoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.Run();
    }
}