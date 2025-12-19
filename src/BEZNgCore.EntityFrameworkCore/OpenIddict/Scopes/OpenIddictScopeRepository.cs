using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Scopes;
using BEZNgCore.EntityFrameworkCore;

namespace BEZNgCore.OpenIddict.Scopes;

public class OpenIddictScopeRepository : EfCoreOpenIddictScopeRepository<BEZNgCoreDbContext>
{
    public OpenIddictScopeRepository(
        IDbContextProvider<BEZNgCoreDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

