using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace BEZNgCore.Web.Public.Views;

public abstract class BEZNgCoreRazorPage<TModel> : AbpRazorPage<TModel>
{
    [RazorInject]
    public IAbpSession AbpSession { get; set; }

    protected BEZNgCoreRazorPage()
    {
        LocalizationSourceName = BEZNgCoreConsts.LocalizationSourceName;
    }
}

