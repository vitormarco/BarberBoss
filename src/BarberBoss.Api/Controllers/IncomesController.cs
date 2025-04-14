using BarberBoss.Application.UseCases.Incomes.DeleteById;
using BarberBoss.Application.UseCases.Incomes.GetAll;
using BarberBoss.Application.UseCases.Incomes.GetById;
using BarberBoss.Application.UseCases.Incomes.Register;
using BarberBoss.Application.UseCases.Incomes.Update;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.Api.Controllers;

public class IncomesController : BarberBossApiBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseIncomesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetAllIncomesUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Incomes.Count is not 0) return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseIncomeJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] long id,
        [FromServices] IGetIncomeByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }

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

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] long id,
        [FromBody] RequestIncomeJson request,
        [FromServices] IUpdateIncomesUseCase useCase)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(
        [FromRoute] long id,
        [FromServices] IDeleteIncomeByIdUseCase useCase) 
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
