using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.OpenIddict.EntityFrameworkCore.Tokens;
using BEZNgCore.EntityFrameworkCore;

namespace BEZNgCore.OpenIddict.Tokens;

public class OpenIddictTokenRepository : EfCoreOpenIddictTokenRepository<BEZNgCoreDbContext>
{
    public OpenIddictTokenRepository(
        IDbContextProvider<BEZNgCoreDbContext> dbContextProvider,
        IUnitOfWorkManager unitOfWorkManager) : base(dbContextProvider, unitOfWorkManager)
    {
    }
}

