using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterIncomeJsonBuilder
{
    public static RequestIncomeJson Build()
    {
        return new Faker<RequestIncomeJson>()
                    .RuleFor(req => req.Title, faker => faker.Commerce.ProductName())
                    .RuleFor(req => req.Description, faker => faker.Commerce.ProductDescription())
                    .RuleFor(req => req.Date, faker => faker.Date.Past())
                    .RuleFor(req => req.PaymentType, faker => faker.PickRandom<PaymentType>())
                    .RuleFor(req => req.Amount, faker => faker.Finance.Amount(min: 1, decimals: 2, max: 100));
    }
}
