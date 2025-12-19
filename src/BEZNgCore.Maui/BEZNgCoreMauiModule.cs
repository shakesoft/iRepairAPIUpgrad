using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BEZNgCore.ApiClient;
using BEZNgCore.Maui.Core;

namespace BEZNgCore.Maui;

[DependsOn(typeof(BEZNgCoreClientModule), typeof(AbpAutoMapperModule))]
public class BEZNgCoreMauiModule : AbpModule
{
    public override void PreInitialize()
    {
        Configuration.Localization.IsEnabled = false;
        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

        Configuration.ReplaceService<IApplicationContext, MauiApplicationContext>();
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreMauiModule).GetAssembly());
    }
}