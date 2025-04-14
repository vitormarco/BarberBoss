using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Incomes.Register;

public interface IRegisterIncomeUseCase
{
    Task<ResponseRegisteredIncomeJson> Execute(RequestIncomeJson request);
}
