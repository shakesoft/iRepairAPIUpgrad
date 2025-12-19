using Abp.Domain.Services;

namespace BEZNgCore;

public abstract class BEZNgCoreDomainServiceBase : DomainService
{
    /* Add your common members for all your domain services. */

    protected BEZNgCoreDomainServiceBase()
    {
        LocalizationSourceName = BEZNgCoreConsts.LocalizationSourceName;
    }
}

