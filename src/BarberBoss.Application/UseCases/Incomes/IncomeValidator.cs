using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace BarberBoss.Application.UseCases.Incomes;

public class IncomeValidator : AbstractValidator<RequestIncomeJson>
{
    public IncomeValidator()
    {
        RuleFor(income => income.Title)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(income => income.Amount)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO);
        RuleFor(income => income.Date)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage(ResourceErrorMessages.INCOME_DATE_CANNOT_BE_IN_FUTTURE);
        RuleFor(income => income.PaymentType)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }

}
