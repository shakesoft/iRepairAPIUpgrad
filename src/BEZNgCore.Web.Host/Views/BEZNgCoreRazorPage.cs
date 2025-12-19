using Abp.AspNetCore.Mvc.Views;

namespace BEZNgCore.Web.Views;

public abstract class BEZNgCoreRazorPage<TModel> : AbpRazorPage<TModel>
{
    protected BEZNgCoreRazorPage()
    {
        LocalizationSourceName = BEZNgCoreConsts.LocalizationSourceName;
    }
}

