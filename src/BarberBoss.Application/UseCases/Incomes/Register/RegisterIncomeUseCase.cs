using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Incomes.Register;

public class RegisterIncomeUseCase : IRegisterIncomeUseCase
{
    public async Task<ResponseRegisteredIncomeJson> Execute(RequestIcomeJson request)
    {
        // Validate the request

        // Attach the request to Entity

        // prepare the sql to insert the data
        await Task.Delay(100);
        // Execute the sql
        await Task.Delay(100);

        // return the response
        return new ResponseRegisteredIncomeJson { Id = 1, Title = request.Title };
    }
}
