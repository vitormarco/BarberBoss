using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Incomes;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Incomes.GetById;

public class GetIncomeByIdUseCase(
    IIncomesReadOnlyRepository repository, 
    IMapper mapper) : IGetIncomeByIdUseCase
{
    private readonly IIncomesReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    public async Task<ResponseIncomeJson> Execute(long id)
    {
        var result = await _repository.GetById(id)
                     ?? throw new NotFoundException(ResourceErrorMessages.INCOME_NOT_FOUND);

        return _mapper.Map<ResponseIncomeJson>(result);
    }
}
