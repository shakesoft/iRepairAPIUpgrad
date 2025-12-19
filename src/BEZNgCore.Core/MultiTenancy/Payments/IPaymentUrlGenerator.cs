namespace BEZNgCore.MultiTenancy.Payments;

public interface IPaymentUrlGenerator
{
    string CreatePaymentRequestUrl(SubscriptionPayment subscriptionPayment);
}

