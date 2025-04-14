using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Incomes.Register;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterIncomeUseCase, RegisterIncomeUseCase>();
    }
    private static void AddAutoMapper(IServiceCollection services) => services.AddAutoMapper(typeof(AutoMapping));
}
