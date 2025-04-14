using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Incomes.DeleteById;
using BarberBoss.Application.UseCases.Incomes.GetAll;
using BarberBoss.Application.UseCases.Incomes.GetById;
using BarberBoss.Application.UseCases.Incomes.Register;
using BarberBoss.Application.UseCases.Incomes.Reports.Excel;
using BarberBoss.Application.UseCases.Incomes.Reports.Pdf;
using BarberBoss.Application.UseCases.Incomes.Update;
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
        services.AddScoped<IGetAllIncomesUseCase, GetAllIncomesUseCase>();
        services.AddScoped<IGetIncomeByIdUseCase, GetIncomeByIdUseCase>();
        services.AddScoped<IUpdateIncomesUseCase, UpdateIncomesUseCase>();
        services.AddScoped<IDeleteIncomeByIdUseCase, DeleteIncomeByIdUseCase>();
        services.AddScoped<IGenerateIncomesReportExcelUseCase, GenerateIncomesReportExcelUseCase>();
        services.AddScoped<IGenerateIncomesReportPdfUseCase, GenerateIncomesExpensesReportPdfUseCase>();
    }
    private static void AddAutoMapper(IServiceCollection services) => services.AddAutoMapper(typeof(AutoMapping));
}
