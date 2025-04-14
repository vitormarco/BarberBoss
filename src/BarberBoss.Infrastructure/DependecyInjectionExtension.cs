using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Incomes;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Infrastructure;

public static class DependecyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        AddRepositories(service);
        AddDbContext(service, configuration);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IIncomesWriteOnlyRepository, IncomesRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var serverVersion = new MySqlServerVersion(new Version(major: 9, minor: 2, build: 0));

        services
            .AddDbContext<BarberBossDbContext>(
                config => config
                            .UseMySql(connectionString, serverVersion));
    }
}
