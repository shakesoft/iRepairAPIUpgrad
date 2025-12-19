using System;
using System.IO;
using Abp;
using Abp.AspNetZeroCore;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using BEZNgCore.Authorization.Users;
using BEZNgCore.Configuration;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.MultiTenancy;
using BEZNgCore.Security.Recaptcha;
using BEZNgCore.Test.Base.DependencyInjection;
using BEZNgCore.Test.Base.UiCustomization;
using BEZNgCore.Test.Base.Url;
using BEZNgCore.Test.Base.Web;
using BEZNgCore.UiCustomization;
using BEZNgCore.Url;
using NSubstitute;

namespace BEZNgCore.Test.Base;

[DependsOn(
    typeof(BEZNgCoreApplicationModule),
    typeof(BEZNgCoreEntityFrameworkCoreModule),
    typeof(AbpTestBaseModule))]
public class BEZNgCoreTestBaseModule : AbpModule
{
    public BEZNgCoreTestBaseModule(BEZNgCoreEntityFrameworkCoreModule abpZeroTemplateEntityFrameworkCoreModule)
    {
        abpZeroTemplateEntityFrameworkCoreModule.SkipDbContextRegistration = true;
    }

    public override void PreInitialize()
    {
        var configuration = GetConfiguration();

        Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

        Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
        Configuration.UnitOfWork.IsTransactional = false;

        //Use database for language management
        Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

        RegisterFakeService<AbpZeroDbMigrator>();

        IocManager.Register<IAppUrlService, FakeAppUrlService>();
        IocManager.Register<IWebUrlService, FakeWebUrlService>();
        IocManager.Register<IRecaptchaValidator, FakeRecaptchaValidator>();

        Configuration.ReplaceService<IAppConfigurationAccessor, TestAppConfigurationAccessor>();
        Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
        Configuration.ReplaceService<IUiThemeCustomizerFactory, NullUiThemeCustomizerFactory>();

        Configuration.Modules.AspNetZero().LicenseCode = configuration["AbpZeroLicenseCode"];

        //Uncomment below line to write change logs for the entities below:
        Configuration.EntityHistory.IsEnabled = true;
        Configuration.EntityHistory.Selectors.Add("BEZNgCoreEntities", typeof(User), typeof(Tenant));
    }

    public override void Initialize()
    {
        ServiceCollectionRegistrar.Register(IocManager);
    }

    private void RegisterFakeService<TService>()
        where TService : class
    {
        IocManager.IocContainer.Register(
            Component.For<TService>()
                .UsingFactoryMethod(() => Substitute.For<TService>())
                .LifestyleSingleton()
        );
    }

    private static IConfigurationRoot GetConfiguration()
    {
        return AppConfigurations.Get(Directory.GetCurrentDirectory(), addUserSecrets: true);
    }
}
