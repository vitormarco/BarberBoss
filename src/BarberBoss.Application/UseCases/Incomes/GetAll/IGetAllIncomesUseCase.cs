using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Incomes.GetAll;

public interface IGetAllIncomesUseCase
{
    Task<ResponseIncomesJson> Execute();
}
