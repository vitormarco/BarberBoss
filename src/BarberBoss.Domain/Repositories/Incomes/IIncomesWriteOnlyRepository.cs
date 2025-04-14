using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Incomes;

public interface IIncomesWriteOnlyRepository
{
    Task Add(Income income);
}
