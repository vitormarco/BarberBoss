using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Incomes;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

public class IncomesRepository(BarberBossDbContext dbContext) : IIncomesWriteOnlyRepository
{
    private readonly BarberBossDbContext _dbContext = dbContext;

    public async Task Add(Income income) => await _dbContext.Incomes.AddAsync(income);
}
