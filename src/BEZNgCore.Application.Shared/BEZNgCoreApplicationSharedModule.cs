using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BEZNgCore;

[DependsOn(typeof(BEZNgCoreCoreSharedModule))]
public class BEZNgCoreApplicationSharedModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreApplicationSharedModule).GetAssembly());
    }
}

