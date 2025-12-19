using Abp.AspNetCore.Mvc.ViewComponents;

namespace BEZNgCore.Web.Public.Views;

public abstract class BEZNgCoreViewComponent : AbpViewComponent
{
    protected BEZNgCoreViewComponent()
    {
        LocalizationSourceName = BEZNgCoreConsts.LocalizationSourceName;
    }
}

