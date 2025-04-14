namespace BarberBoss.Application.UseCases.Incomes.Reports.Pdf;

public interface IGenerateIncomesReportPdfUseCase
{
    Task<byte[]> Execute(DateOnly month);
}
