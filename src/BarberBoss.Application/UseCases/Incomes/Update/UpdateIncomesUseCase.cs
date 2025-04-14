using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Incomes;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Incomes.Update;

public class UpdateIncomesUseCase(
    IIncomeUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IUpdateIncomesUseCase
{
    private readonly IIncomeUpdateOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;


    public async Task Execute(long id, RequestIncomeJson request)
    {
        Validate(request);
        var income = await _repository.GetById(id)
                        ?? throw new NotFoundException(ResourceErrorMessages.INCOME_NOT_FOUND);
        _mapper.Map(request, income);
        _repository.Update(income);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestIncomeJson request)
    {
        var validator = new IncomeValidator();
        var result = validator.Validate(request);

        if (result.IsValid is false)
        {
            var errorMessages = result
                                    .Errors
                                    .Select(e => e.ErrorMessage)
                                    .ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
