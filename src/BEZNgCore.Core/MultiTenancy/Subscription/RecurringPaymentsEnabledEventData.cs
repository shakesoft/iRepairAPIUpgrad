using Abp.Events.Bus;

namespace BEZNgCore.MultiTenancy.Subscription;

public class RecurringPaymentsEnabledEventData : EventData
{
    public int TenantId { get; set; }
}

