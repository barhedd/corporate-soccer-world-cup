using CorporateSoccerWorldCup.Api.Middlewares;
using CorporateSoccerWorldCup.Application.Abstractions.Events;
using CorporateSoccerWorldCup.Application.Abstractions.Messaging;
using CorporateSoccerWorldCup.Application.Features.Teams.Queries.Common.Interfaces;
using CorporateSoccerWorldCup.Domain.Abstractions;
using CorporateSoccerWorldCup.Domain.Abstractions.Repositories;
using CorporateSoccerWorldCup.Infrastructure.ConnectionFactories;
using CorporateSoccerWorldCup.Infrastructure.Contexts;
using CorporateSoccerWorldCup.Infrastructure.Events;
using CorporateSoccerWorldCup.Infrastructure.Persistence;
using CorporateSoccerWorldCup.Infrastructure.Persistence.ReadRepositories;
using CorporateSoccerWorldCup.Infrastructure.Persistence.Repositories;
using CorporateSoccerWorldCup.Infrastructure.Pipelines.Logging;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Read Repositories
builder.Services.AddScoped<ITeamReadRepository, TeamReadRepository>();

// Event Dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Commands
builder.Services.Scan(scan => scan
    .FromApplicationDependencies()
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Queries
builder.Services.Scan(scan => scan
    .FromApplicationDependencies()
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// Logging decorators
builder.Services.Decorate(
    typeof(ICommandHandler<,>),
    typeof(LoggingCommandHandlerDecorator<,>));

builder.Services.Decorate(
    typeof(IQueryHandler<,>),
    typeof(LoggingQueryHandlerDecorator<,>));

// Database string connection configuration
builder.Services.AddScoped<IDbConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return new SqlConnectionFactory(connectionString!);
});

builder.Services.AddDbContext<CorporateSoccerWorldCupContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] " +
            "[CorrelationId: {CorrelationId}] " +
            "[TraceId: {TraceId}] " +
            "{Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddSource("CorporateSoccerWorldCup.Application")
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddOtlpExporter();
    });

var app = builder.Build();

// Apply migrations and data seed to database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<CorporateSoccerWorldCupContext>();

    var retries = 5;
    while (retries > 0)
    {
        try
        {
            dbContext.Database.Migrate();
            break;
        }
        catch
        {
            retries--;
            Thread.Sleep(5000);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<CorrelationMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
