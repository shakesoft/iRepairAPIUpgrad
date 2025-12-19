using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BEZNgCore.Configure;
using BEZNgCore.Startup;
using BEZNgCore.Test.Base;

namespace BEZNgCore.GraphQL.Tests;

[DependsOn(
    typeof(BEZNgCoreGraphQLModule),
    typeof(BEZNgCoreTestBaseModule))]
public class BEZNgCoreGraphQLTestModule : AbpModule
{
    public override void PreInitialize()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddAndConfigureGraphQL();

        WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
    }

    public override void Initialize()
    {
        IocManager.RegisterAssemblyByConvention(typeof(BEZNgCoreGraphQLTestModule).GetAssembly());
    }
}