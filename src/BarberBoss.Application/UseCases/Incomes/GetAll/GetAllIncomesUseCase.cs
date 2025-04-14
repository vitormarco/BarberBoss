using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Incomes;

namespace BarberBoss.Application.UseCases.Incomes.GetAll;

public class GetAllIncomesUseCase(
    IIncomesReadOnlyRepository repository,
    IMapper mapper) : IGetAllIncomesUseCase
{
    private readonly IIncomesReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseIncomesJson> Execute()
    {
        var result = await _repository.GetAll();

        return new ResponseIncomesJson
        { 
            Incomes = _mapper.Map<List<ResponseShortIncomeJson>>(result)
        };
    }
}
