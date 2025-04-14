using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Incomes.GetById;

public interface IGetIncomeByIdUseCase
{
    Task<ResponseIncomeJson> Execute(long id);
}
