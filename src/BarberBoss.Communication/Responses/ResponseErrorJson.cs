namespace BarberBoss.Communication.Responses;

public class ResponseErrorJson
{
    public List<string> Messages { get; set; }

    public ResponseErrorJson(string errorMessage)
    {
        Messages = [errorMessage];
    }

    public ResponseErrorJson(List<string> errorMessages)
    {
        Messages = errorMessages;
    }
}
