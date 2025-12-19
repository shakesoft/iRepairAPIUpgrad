using Microsoft.AspNetCore.Antiforgery;

namespace BEZNgCore.Web.Controllers;

public class AntiForgeryController : BEZNgCoreControllerBase
{
    private readonly IAntiforgery _antiforgery;

    public AntiForgeryController(IAntiforgery antiforgery)
    {
        _antiforgery = antiforgery;
    }

    public void GetToken()
    {
        _antiforgery.SetCookieTokenAndHeader(HttpContext);
    }
}

