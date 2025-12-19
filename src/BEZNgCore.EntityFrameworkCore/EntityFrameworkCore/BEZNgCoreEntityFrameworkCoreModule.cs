using Abp;
using Abp.Dependency;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.OpenIddict.EntityFrameworkCore;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using BEZNgCore.Configuration;
using BEZNgCore.EntityHistory;
using BEZNgCore.Migrations.Seed;

namespace BEZNgCore.EntityFrameworkCore;

[DependsOn(
    typeof(AbpZeroCoreEntityFrameworkCoreModule),
    typeof(BEZNgCoreCoreModule),
    typeof(AbpZeroCoreOpenIddictEntityFrameworkCoreModule)
)]
public class BEZNgCoreEntityFrameworkCoreModule : AbpModule
{
    /* Used it tests to skip DbContext registration, in order to use in-memory database of EF Core */
    public bool SkipDbContextRegistration { get; set; }

    public bool SkipDbSeed { get; set; }

    public override void PreInitialize()
    {
        if (!SkipDbContextRegistration)
        {
            Configuration.Modules.AbpEfCore().AddDbContext<BEZNgCoreDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    BEZNgCoreDbContextConfigurer.Configure(options.DbContextOptions,
                        options.ExistingConnection);
                }
                else
                {
                    BEZNgCoreDbContextConfigurer.Configure(options.DbContextOptions,
                        options.ConnectionString);
                }
            });
        }

        // Set this setting to true for enabling entity history.
        Configuration.EntityHistory.IsEnabled = true;

        // Uncomment below line to write change logs for the entities below:
        Configuration.EntityHistory.Selectors.Add("BEZNgCoreEntities", EntityHistoryHelper.TrackedTypes);
        Configuration.CustomConfigProviders.Add(new EntityHistoryConfigProvider(Configuration));
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreEntityFrameworkCoreModule).GetAssembly());
    }

    public override void PostInitialize()
    {
        var configurationAccessor = IocManager.Resolve<IAppConfigurationAccessor>();

        using (var scope = IocManager.CreateScope())
        {
            //if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:Default"]))
            //if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:HGCl"]))
            //if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:HCO"]))
            //if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:TCOC"]))
            //if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:HGCH"]))
            // if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:HGCB"]))
            // if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:HGC"]))
            // if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:TEST"]))
            // if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:JCHGC"]))
            //if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:HGCAW"]))
            // if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:HGCM"]))
            // if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:TCOH"]))
            if (!SkipDbSeed && scope.Resolve<DatabaseCheckHelper>().Exist(configurationAccessor.Configuration["ConnectionStrings:TEST"]))
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}

