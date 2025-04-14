using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Reports;

namespace BarberBoss.Domain.Extensions;

public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportGenerationMessages.CASH,
            PaymentType.CreditCard => ResourceReportGenerationMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportGenerationMessages.DEBIT_CARD,
            PaymentType.ElectronicTransfer => ResourceReportGenerationMessages.ELECTRONIC_TRANSFER,
            PaymentType.Other => ResourceReportGenerationMessages.OTHER_PAYMENT_TYPE,
            _ => string.Empty
        };
    }
}
