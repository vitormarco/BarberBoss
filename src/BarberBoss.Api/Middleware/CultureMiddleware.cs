using System.Globalization;

namespace BarberBoss.Api.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo
                                    .GetCultures(CultureTypes.AllCultures)
                                    .ToList();
        var requestedCulture = context
                                    .Request
                                    .Headers
                                    .AcceptLanguage
                                    .FirstOrDefault();
        var cultureInfo = new CultureInfo("en-US");
        
        if (!string.IsNullOrWhiteSpace(requestedCulture)
            && supportedLanguages.Exists(language => language.Name.Equals(requestedCulture)))
        {
            cultureInfo = new CultureInfo(requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
