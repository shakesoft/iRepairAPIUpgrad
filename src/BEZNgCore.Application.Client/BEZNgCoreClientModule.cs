using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BEZNgCore;

public class BEZNgCoreClientModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreClientModule).GetAssembly());
    }
}

