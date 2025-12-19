using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Authorizations;
using BEZNgCore.EntityFrameworkCore;

namespace BEZNgCore.OpenIddict.Authorizations;

public class OpenIddictAuthorizationRepository : EfCoreOpenIddictAuthorizationRepository<BEZNgCoreDbContext>
{
    public OpenIddictAuthorizationRepository(
        IDbContextProvider<BEZNgCoreDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

