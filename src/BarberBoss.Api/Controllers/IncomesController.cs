using BarberBoss.Application.UseCases.Incomes.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;

public class IncomesController : BarberBossApiBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredIncomeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterIncomeUseCase useCase,
        [FromBody] RequestIncomeJson request)
    {
        var response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}
