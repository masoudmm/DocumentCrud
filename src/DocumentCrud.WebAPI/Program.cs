using DocumentCrud.Application.Extentions;
using DocumentCrud.Infrastructure.Extentions;
using DocumentCrud.Server.Extentions;
using DocumentCrud.Server.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseExceptionFilter();
app.MapDocumentrEndPoints();

app.Run();
