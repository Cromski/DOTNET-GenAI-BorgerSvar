using BorgerSvar.Application.Common.Interfaces;
using BorgerSvar.Infrastructure.Persistence;
using BorgerSvar.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BorgerSvar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Forbind vores DbContext til SQLite. Vi henter "DefaultConnection" fra appsettings senere.
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=BorgerSvar.db";
        
        services.AddDbContext<BorgerSvarDbContext>(options =>
            options.UseSqlite(connectionString));

        // 2. Registrer vores ægte repository, så vores Handler i applikationslaget kan få det leveret
        services.AddScoped<ICitizenQueryRepository, CitizenQueryRepository>();
        services.AddScoped<IAIService, AIService>();

        return services;
    }
}