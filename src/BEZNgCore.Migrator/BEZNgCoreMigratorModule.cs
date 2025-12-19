using Abp.AspNetZeroCore;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using BEZNgCore.Configuration;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.Migrator.DependencyInjection;

namespace BEZNgCore.Migrator;

[DependsOn(typeof(BEZNgCoreEntityFrameworkCoreModule))]
public class BEZNgCoreMigratorModule : AbpModule
{
    private readonly IConfigurationRoot _appConfiguration;

    public BEZNgCoreMigratorModule(BEZNgCoreEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
    {
        abpZeroTemplateEntityFrameworkCoreModule.SkipDbSeed = true;

        _appConfiguration = AppConfigurations.Get(
            typeof(BEZNgCoreMigratorModule).GetAssembly().GetDirectoryPathOrNull(),
            addUserSecrets: true
        );
    }

    public override void PreInitialize()
    {
        Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
            BEZNgCoreConsts.ConnectionStringName
            );
        Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        Configuration.ReplaceService(typeof(IEventBus), () =>
        {
            IocManager.IocContainer.Register(
                Component.For<IEventBus>().Instance(NullEventBus.Instance)
            );
        });
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreMigratorModule).GetAssembly());
        ServiceCollectionRegistrar.Register(IocManager);
    }
}

