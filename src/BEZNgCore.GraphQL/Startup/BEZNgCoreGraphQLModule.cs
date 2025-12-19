using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace BEZNgCore.Startup;

[DependsOn(typeof(BEZNgCoreCoreModule))]
public class BEZNgCoreGraphQLModule : AbpModule
{
    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreGraphQLModule).GetAssembly());
    }

    public override void PreInitialize()
    {
        base.PreInitialize();

        //Adding custom AutoMapper configuration
        Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
    }
}

