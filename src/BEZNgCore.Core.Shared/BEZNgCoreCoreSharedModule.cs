using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BEZNgCore;

public class BEZNgCoreCoreSharedModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreCoreSharedModule).GetAssembly());
    }
}

