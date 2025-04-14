using BarberBoss.Application.UseCases.Incomes;
using BarberBoss.Communication.Enums;
using BarberBoss.Exception;
using CommonTestUtilities.Requests;
using Shouldly;


namespace Validators.Tests.Incomes.Register;

public class RegisterIncomeValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new IncomeValidator();
        var request = RequestRegisterIncomeJsonBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("          ")]
    [InlineData(null)]
    public void Error_Title_Empty(string? title)
    {
        // Arrange
        var validator = new IncomeValidator();
        var request = RequestRegisterIncomeJsonBuilder.Build();
        request.Title = title ?? string.Empty;  

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result
            .Errors
            .ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.ShouldContain(
                        message => message
                                        .ErrorMessage
                                        .Equals(ResourceErrorMessages.TITLE_REQUIRED)
                    )
            );
    }

    [Fact]
    public void Error_Income_Date_Future()
    {
        // Arrange
        var validator = new IncomeValidator();
        var request = RequestRegisterIncomeJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result
            .Errors
            .ShouldSatisfyAllConditions(
                e => e.ShouldHaveSingleItem(),
                e => e.ShouldContain(
                    message => message
                                .ErrorMessage
                                .Equals(ResourceErrorMessages.INCOME_DATE_CANNOT_BE_IN_FUTTURE)
                )
            );

    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        // Arrange
        var validator = new IncomeValidator();
        var request = RequestRegisterIncomeJsonBuilder.Build();
        request.PaymentType = (PaymentType)999;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result
            .Errors
            .ShouldSatisfyAllConditions(
               condition => condition.ShouldHaveSingleItem(),
               condition => condition.ShouldContain(
                                message => message
                                            .ErrorMessage
                                            .Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID)
                            )
               );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-200)]
    public void Error_Amount_Must_Be_Greater_Than_Zero(decimal amount)
    {
        // Arrange
        var validator = new IncomeValidator();
        var request = RequestRegisterIncomeJsonBuilder.Build();
        request.Amount = amount;

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.ShouldBeFalse();
        result
            .Errors
            .ShouldSatisfyAllConditions(
                cond => cond.ShouldHaveSingleItem(),
                cond => cond.ShouldContain(
                                message => message
                                            .ErrorMessage
                                            .Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO))
            );
    }
}
