using BarberBoss.Application.UseCases.Incomes.Reports.Pdf.Colors;
using BarberBoss.Application.UseCases.Incomes.Reports.Pdf.Fonts;
using BarberBoss.Domain.Extensions;
using BarberBoss.Domain.Reports;
using BarberBoss.Domain.Repositories.Incomes;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Incomes.Reports.Pdf;

public class GenerateIncomesExpensesReportPdfUseCase : IGenerateIncomesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_INCOME_TABLE = 25;
    private readonly IIncomesReadOnlyRepository _repository;
    public GenerateIncomesExpensesReportPdfUseCase(IIncomesReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new IncomesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var incomes = await _repository.FilterByMonth(month);
        if (incomes.Count is 0) return [];

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);
        var totalIncomes = incomes.Sum(income => income.Amount);
        CreateTotalIncome(page, month, totalIncomes);

        foreach (var income in incomes)
        {
            var table = CreateIncomeTable(page);
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_INCOME_TABLE;

            AddIncomeTitle(cell: row.Cells[0], incomeTitle: income.Title.ToUpper());
            AddHeaderForTotal(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_INCOME_TABLE;

            row.Cells[0].AddParagraph(income.Date.ToString("D"));
            row.Cells[0].Format.LeftIndent = 9;
            SetStyleBaseForIncomeInformation(row.Cells[0]);

            row.Cells[1].AddParagraph(income.Date.ToString("t"));
            SetStyleBaseForIncomeInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(income.PaymentType.PaymentTypeToString());
            SetStyleBaseForIncomeInformation(row.Cells[2]);

            AddAmountForIncome(cell: row.Cells[3], incomeAmout: income.Amount);

            if (string.IsNullOrEmpty(income.Description) is false)
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_INCOME_TABLE;
                AddDescriptionForIncome(cell: descriptionRow.Cells[0], incomeDescription: income.Description);

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private static Document CreateDocument(DateOnly month)
    {
        var document = new Document();
        document.Info.Title = $"{ResourceReportGenerationMessages.INCOME_FOR} {month:Y}";
        document.Info.Author = "Barber Boss";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.DEFAULT_FONT;
        return document;
    }
    private static Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 50;
        section.PageSetup.BottomMargin = 50;

        return section;
    }
    private static void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");
        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "ProfilePhoto.png");

        row.Cells[0].AddImage(pathFile);
        row.Cells[1].AddParagraph("Barber Boss");
        row.Cells[1].Format.Font = new Font
        {
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 25,
        };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        row.Cells[1].Format.LeftIndent = 18;
    }
    private static void CreateTotalIncome(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "38";
        paragraph.Format.SpaceAfter = "64";
        var title = string.Format(ResourceReportGenerationMessages.TOTAL_INCOME_IN, month.ToString("Y"));
        paragraph.AddFormattedText(title, new Font { Name = FontHelper.ROBOTO_MEDIUM, Size = 15 });
        paragraph.AddLineBreak();
        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses}", new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 50 });
    }
    private static Table CreateIncomeTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }
    private static void AddIncomeTitle(Cell cell, string incomeTitle)
    {
        cell.AddParagraph(incomeTitle);
        cell.Format.Font = new Font { Name = FontHelper.BEBAS_NEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.BLACK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 7;
    }
    private static void AddHeaderForTotal(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font
        {
            Name = FontHelper.BEBAS_NEUE_REGULAR,
            Size = 15,
            Color = ColorsHelper.WHITE
        };
        cell.Shading.Color = ColorsHelper.BLACK_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.RightIndent = 4;
    }
    private static void SetStyleBaseForIncomeInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GRAY_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
    private static void AddAmountForIncome(Cell cell, decimal incomeAmout)
    {
        cell.AddParagraph($"{CURRENCY_SYMBOL} {incomeAmout}");
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
    private static void AddDescriptionForIncome(Cell cell, string incomeDescription)
    {
        cell.AddParagraph(incomeDescription);
        cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = 9, Color = ColorsHelper.GRAY_DARKNESS };
        cell.Shading.Color = ColorsHelper.GRAY_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 9;
    }
    private static void AddWhiteSpace(Table table) 
    {
        var row = table.AddRow();
        row.Height = 16;
        row.Borders.Visible = false;
    }
    private static byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
