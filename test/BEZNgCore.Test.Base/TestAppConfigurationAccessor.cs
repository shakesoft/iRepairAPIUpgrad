using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using BEZNgCore.Configuration;

namespace BEZNgCore.Test.Base;

public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
{
    public IConfigurationRoot Configuration { get; }

    public TestAppConfigurationAccessor()
    {
        Configuration = AppConfigurations.Get(
            typeof(BEZNgCoreTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
        );
    }
}
