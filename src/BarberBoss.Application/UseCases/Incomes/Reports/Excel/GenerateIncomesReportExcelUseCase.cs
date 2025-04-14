
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Incomes;
using ClosedXML.Excel;

namespace BarberBoss.Application.UseCases.Incomes.Reports.Excel;

public class GenerateIncomesReportExcelUseCase(IIncomesReadOnlyRepository repository) : IGenerateIncomesReportExcelUseCase
{
    private const string AUTHOR_NAME = "Barber Boss";
    private const int FONT_SIZE = 12;
    private const string CURRENCY_SYMBOL = "R$";
    private readonly List<string> ALIGNMENT_HEADER_COLUMNS_CENTER = ["A1", "B1", "C1", "E1"];
    private readonly List<string> ALIGNMENT_HEADER_COLUMNS_RIGHT = ["D1"];
    private const string FONT_NAME = "Times New Roman";

    private readonly IIncomesReadOnlyRepository _repository = repository;

    public async Task<byte[]> Execute(DateOnly month)
    {
        var incomes = await _repository.FilterByMonth(month);

        if (incomes.Count is 0) return [];

        using var workbook = new XLWorkbook
        {
            Author = AUTHOR_NAME,
        };
        workbook.Style.Font.FontSize = FONT_SIZE;
        workbook.Style.Font.FontName = FONT_NAME;

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
        InsertHeader(worksheet);

        var raw = 2;
        foreach (var income in incomes)
        {
            worksheet.Cell($"A{raw}").Value = income.Title;
            worksheet.Cell($"B{raw}").Value = income.Date.ToString("dd/MM/yyyy");
            worksheet.Cell($"C{raw}").Value = income.PaymentType.PaymentTypeToString();
            worksheet.Cell($"D{raw}").Value = income.Amount;
            worksheet.Cell($"E{raw}").Value = income.Description;

            FormatCurrency(worksheet, $"D{raw}");
            raw++;
        }
        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private static void FormatCurrency(IXLWorksheet worksheet, string cell)
    {
        worksheet.Cell(cell).Style.NumberFormat.Format = $"{CURRENCY_SYMBOL} #,##0.00";
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.FromHtml("#FFFFFF");
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");

        foreach (var item in ALIGNMENT_HEADER_COLUMNS_CENTER)
        {
            worksheet.Cell(item).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }

        foreach (var item in ALIGNMENT_HEADER_COLUMNS_RIGHT)
        {
            worksheet.Cell(item).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }
    }
}
