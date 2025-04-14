namespace BarberBoss.Application.UseCases.Incomes.Reports.Excel;

public interface IGenerateIncomesReportExcelUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
