using Xunit;

namespace BEZNgCore.Tests;

public sealed class MultiTenantFactAttribute : FactAttribute
{
    private readonly bool _multiTenancyEnabled = BEZNgCoreConsts.MultiTenancyEnabled;

    public MultiTenantFactAttribute()
    {
        if (!_multiTenancyEnabled)
        {
            Skip = "MultiTenancy is disabled.";
        }
    }
}
