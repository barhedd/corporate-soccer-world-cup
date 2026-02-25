using CorporateSoccerWorldCup.Application.Features.Teams.Interfaces;
using CorporateSoccerWorldCup.Application.Interfaces;
using CorporateSoccerWorldCup.Domain.Interfaces;
using CorporateSoccerWorldCup.Domain.Interfaces.Repositories;
using CorporateSoccerWorldCup.Infrastructure;
using CorporateSoccerWorldCup.Infrastructure.ConnectionFactories;
using CorporateSoccerWorldCup.Infrastructure.Contexts;
using CorporateSoccerWorldCup.Infrastructure.Persistence.ReadRepositories;
using CorporateSoccerWorldCup.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITeamReadRepository, TeamReadRepository>();

builder.Services.Scan(scan => scan
    .FromApplicationDependencies()
    .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.Scan(scan => scan
    .FromApplicationDependencies()
    .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<IDbConnectionFactory>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    return new SqlConnectionFactory(connectionString!);
});

builder.Services.AddDbContext<CorporateSoccerWorldCupContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CorporateSoccerWorldCupContext>();

    var retries = 5;
    while (retries > 0)
    {
        try
        {
            context.Database.Migrate();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
