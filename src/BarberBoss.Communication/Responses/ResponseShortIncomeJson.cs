namespace BarberBoss.Communication.Responses;

public class ResponseShortIncomeJson
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}
