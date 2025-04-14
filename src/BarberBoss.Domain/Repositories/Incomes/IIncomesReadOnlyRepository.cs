using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Incomes;

public interface IIncomesReadOnlyRepository
{
    Task<List<Income>> GetAll();
    Task<Income?> GetById(long id);
    Task<List<Income>> FilterByMonth(DateOnly date);
}
