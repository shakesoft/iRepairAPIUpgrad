using Abp.Dependency;
using BEZNgCore.MultiTenancy.Payments;
using BEZNgCore.Url;

namespace BEZNgCore.Web.Url;

public class PaymentUrlGenerator : IPaymentUrlGenerator, ITransientDependency
{
    private readonly IWebUrlService _webUrlService;

    public PaymentUrlGenerator(IWebUrlService webUrlService)
    {
        _webUrlService = webUrlService;
    }

    public string CreatePaymentRequestUrl(SubscriptionPayment subscriptionPayment)
    {
        var webSiteRootAddress = _webUrlService.GetSiteRootAddress();

        return webSiteRootAddress +
               "account/gateway-selection?paymentId=" +
               subscriptionPayment.Id;
    }
}

