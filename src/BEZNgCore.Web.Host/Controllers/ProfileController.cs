using Abp.AspNetCore.Mvc.Authorization;
using BEZNgCore.Authorization.Users.Profile;
using BEZNgCore.Storage;

namespace BEZNgCore.Web.Controllers;

[AbpMvcAuthorize]
public class ProfileController : ProfileControllerBase
{
    public ProfileController(
        ITempFileCacheManager tempFileCacheManager,
        IProfileAppService profileAppService) :
        base(tempFileCacheManager, profileAppService)
    {
    }
}

