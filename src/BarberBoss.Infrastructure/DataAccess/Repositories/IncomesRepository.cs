using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Incomes;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

public class IncomesRepository(BarberBossDbContext dbContext) : IIncomesWriteOnlyRepository, IIncomesReadOnlyRepository, IIncomeUpdateOnlyRepository, IIncomesDeleteOnlyRepository
{
    private readonly BarberBossDbContext _dbContext = dbContext;

    public async Task Add(Income income) => await _dbContext.Incomes.AddAsync(income);

    public async Task<List<Income>> GetAll() => await _dbContext.Incomes.AsNoTracking().ToListAsync();

    async Task<Income?> IIncomesReadOnlyRepository.GetById(long id)
    {
        return await _dbContext
            .Incomes
            .AsNoTracking()
            .FirstOrDefaultAsync(income => income.Id == id);
    }

    async Task<Income?> IIncomeUpdateOnlyRepository.GetById(long id)
    {
        return await _dbContext
            .Incomes
            .FirstOrDefaultAsync(income => income.Id == id);
    }

    public void Update(Income income)
    {
        _dbContext.Incomes.Update(income);
    }

    public async Task<bool> DeleteById(long id)
    {
        var result = await _dbContext
                                .Incomes
                                .FirstOrDefaultAsync(income => income.Id == id);

        if (result is null) return false;

        _dbContext.Incomes.Remove(result);
        return true;
    }

    public async Task<List<Income>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1);
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, 
                                   month: date.Month,
                                   day: daysInMonth, 
                                   hour: 23, 
                                   minute: 59, 
                                   second: 59);
        return await _dbContext
                        .Incomes
                        .AsNoTracking()
                        .Where(income => income.Date >= startDate && income.Date <= endDate)
                        .OrderBy(income => income.Date)
                        .ThenBy(income => income.Title)
                        .ToListAsync();
    }
}
