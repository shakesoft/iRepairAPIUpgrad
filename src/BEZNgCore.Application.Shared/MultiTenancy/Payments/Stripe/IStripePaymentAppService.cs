using System.Threading.Tasks;
using Abp.Application.Services;
using BEZNgCore.MultiTenancy.Payments.Dto;
using BEZNgCore.MultiTenancy.Payments.Stripe.Dto;

namespace BEZNgCore.MultiTenancy.Payments.Stripe;

public interface IStripePaymentAppService : IApplicationService
{
    Task ConfirmPayment(StripeConfirmPaymentInput input);

    StripeConfigurationDto GetConfiguration();

    Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
}

