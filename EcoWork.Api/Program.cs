using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using AutoMapper;
using EcoWork.Api.Persistence;
using EcoWork.Api.Mappings;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// Logging configurado
// ----------------------------
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ----------------------------
// Swagger
// ----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ----------------------------
// Controllers
// ----------------------------
builder.Services.AddControllers();

// ----------------------------
// DbContext - PostgreSQL
// ----------------------------
builder.Services.AddDbContext<EcoWorkDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------------
// AutoMapper
// ----------------------------
builder.Services.AddAutoMapper(typeof(DepartamentoProfile));

// ----------------------------
// Health Checks
// ----------------------------
builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        name: "postgres",
        failureStatus: HealthStatus.Unhealthy
    )
    .AddCheck("self", () => HealthCheckResult.Healthy());

// ----------------------------
// OpenTelemetry
// ----------------------------
builder.Services.AddOpenTelemetry()
    .WithTracing(tracer =>
    {
        tracer
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("EcoWork.Api"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddConsoleExporter();
    });

var app = builder.Build();

// ----------------------------
// Swagger em Dev e Prod
// ----------------------------
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapControllers();

// -------------------------------------------------
// MIGRATIONS AUTOMÁTICAS AO INICIAR
// (ESSENCIAL PARA O RENDER!)
// -------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EcoWorkDbContext>();
    db.Database.Migrate();
}

app.Run();

public partial class Program { }
