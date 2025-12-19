using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BEZNgCore.EntityFrameworkCore;
using BEZNgCore.Identity;

namespace BEZNgCore.Test.Base.DependencyInjection;

public static class ServiceCollectionRegistrar
{
    public static void Register(IIocManager iocManager)
    {
        RegisterIdentity(iocManager);

        var builder = new DbContextOptionsBuilder<BEZNgCoreDbContext>();

        var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
        builder.UseSqlite(inMemorySqlite);

        iocManager.IocContainer.Register(
            Component
                .For<DbContextOptions<BEZNgCoreDbContext>>()
                .Instance(builder.Options)
                .LifestyleSingleton()
        );

        inMemorySqlite.Open();

        new BEZNgCoreDbContext(builder.Options).Database.EnsureCreated();
    }

    private static void RegisterIdentity(IIocManager iocManager)
    {
        var services = new ServiceCollection();

        IdentityRegistrar.Register(services);

        WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
    }
}
