using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Incomes;

public interface IIncomeUpdateOnlyRepository
{
    void Update(Income income);

    Task<Income?> GetById(long id);
}
