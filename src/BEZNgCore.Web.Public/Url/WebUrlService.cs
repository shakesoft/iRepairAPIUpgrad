using Abp.Dependency;
using BEZNgCore.Configuration;
using BEZNgCore.Url;
using BEZNgCore.Web.Url;

namespace BEZNgCore.Web.Public.Url;

public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
{
    public WebUrlService(
        IAppConfigurationAccessor appConfigurationAccessor) :
        base(appConfigurationAccessor)
    {
    }

    public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

    public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
}

