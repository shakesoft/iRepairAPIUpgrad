using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Applications;
using BEZNgCore.EntityFrameworkCore;

namespace BEZNgCore.OpenIddict.Applications;

public class OpenIddictApplicationRepository : EfCoreOpenIddictApplicationRepository<BEZNgCoreDbContext>
{
    public OpenIddictApplicationRepository(
        IDbContextProvider<BEZNgCoreDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

