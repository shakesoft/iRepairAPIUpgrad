using Xunit;

namespace BEZNgCore.Tests;

public sealed class MultiTenantTheoryAttribute : TheoryAttribute
{
    private readonly bool _multiTenancyEnabled = BEZNgCoreConsts.MultiTenancyEnabled;

    public MultiTenantTheoryAttribute()
    {
        if (!_multiTenancyEnabled)
        {
            Skip = "MultiTenancy is disabled.";
        }
    }
}