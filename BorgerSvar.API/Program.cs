using BorgerSvar.Application.CitizenQueries.Commands;
using BorgerSvar.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. TILFØJ SERVICES FRA VORES ANDRE LAG (Dependency Injection)

// Her registrerer vi MediatR og fortæller, at den skal lede efter Handlers i vores Application-lag
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateCitizenQueryCommand).Assembly));

// Her kalder vi den udvidelsesmetode, vi lavede i infrastrukturen, som opsætter SQLite og AiService
builder.Services.AddInfrastructure(builder.Configuration);

// Vi tilføjer Swagger, så vi nemt kan teste vores API visuelt i browseren
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. KONFIGURER HTTP-PIPELINEN

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. VORES ENDPOINTS (Dørene ind til applikationen)

// POST: /api/queries - Modtager et spørgsmål fra en borger
app.MapPost("/api/queries", async Task<IResult> (
    [FromBody] CreateCitizenQueryCommand command,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    var queryId = await mediator.Send(command, cancellationToken);
    return Results.Created($"/api/queries/{queryId}", new { id = queryId });
})
.WithName("CreateCitizenQuery");

app.Run();