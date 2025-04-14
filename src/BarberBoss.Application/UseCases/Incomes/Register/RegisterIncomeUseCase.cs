using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Incomes;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Incomes.Register;

public class RegisterIncomeUseCase(
    IIncomesWriteOnlyRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRegisterIncomeUseCase
{
    private readonly IIncomesWriteOnlyRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<ResponseRegisteredIncomeJson> Execute(RequestIncomeJson request)
    {
        Validate(request);
        var entity = _mapper.Map<Income>(request);
        
        await _repository.Add(entity);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredIncomeJson>(entity);
    }

    private static void Validate(RequestIncomeJson request)
    {
        var validator = new IncomeValidator();
        var result = validator.Validate(request);

        if (result.IsValid is false)
        {
            var errorMessages = result
                                    .Errors
                                    .Select(error => error.ErrorMessage)
                                    .ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
