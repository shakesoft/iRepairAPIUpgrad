using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;

namespace BEZNgCore.EntityFrameworkCore;

public class AbpZeroDbMigrator : AbpZeroDbMigrator<BEZNgCoreDbContext>
{
    public AbpZeroDbMigrator(
        IUnitOfWorkManager unitOfWorkManager,
        IDbPerTenantConnectionStringResolver connectionStringResolver,
        IDbContextResolver dbContextResolver) :
        base(
            unitOfWorkManager,
            connectionStringResolver,
            dbContextResolver)
    {

    }
}

