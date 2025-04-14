namespace BarberBoss.Application.UseCases.Incomes.DeleteById;

public interface IDeleteIncomeByIdUseCase
{
    Task Execute(long id);
}
