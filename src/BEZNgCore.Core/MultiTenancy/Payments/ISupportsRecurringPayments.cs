using Abp.Events.Bus.Handlers;
using BEZNgCore.MultiTenancy.Subscription;

namespace BEZNgCore.MultiTenancy.Payments;

public interface ISupportsRecurringPayments :
    IEventHandler<RecurringPaymentsDisabledEventData>,
    IEventHandler<RecurringPaymentsEnabledEventData>,
    IEventHandler<SubscriptionUpdatedEventData>,
    IEventHandler<SubscriptionCancelledEventData>
{

}

