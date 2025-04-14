using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Incomes.Update;

public interface IUpdateIncomesUseCase
{
    Task Execute(long id, RequestIncomeJson request);
}
