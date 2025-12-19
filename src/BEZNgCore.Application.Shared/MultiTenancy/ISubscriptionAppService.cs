using System.Threading.Tasks;
using Abp.Application.Services;
using BEZNgCore.MultiTenancy.Dto;
using BEZNgCore.MultiTenancy.Payments.Dto;

namespace BEZNgCore.MultiTenancy;

public interface ISubscriptionAppService : IApplicationService
{
    Task DisableRecurringPayments();

    Task EnableRecurringPayments();

    Task<long> StartExtendSubscription(StartExtendSubscriptionInput input);

    Task<StartUpgradeSubscriptionOutput> StartUpgradeSubscription(StartUpgradeSubscriptionInput input);

    Task<long> StartTrialToBuySubscription(StartTrialToBuySubscriptionInput input);
}

