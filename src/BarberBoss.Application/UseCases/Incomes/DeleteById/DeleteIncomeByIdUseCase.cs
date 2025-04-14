using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Incomes;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Incomes.DeleteById;

public class DeleteIncomeByIdUseCase(
    IIncomesDeleteOnlyRepository repository,
    IUnitOfWork unitOfWork) : IDeleteIncomeByIdUseCase
{
    private readonly IIncomesDeleteOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(long id)
    {
        var result = await _repository.DeleteById(id);

        if (result is false)
            throw new NotFoundException(ResourceErrorMessages.INCOME_NOT_FOUND);

        await _unitOfWork.Commit();
    }
}
