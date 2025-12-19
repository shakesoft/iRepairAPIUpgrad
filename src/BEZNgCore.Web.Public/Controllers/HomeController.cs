using Microsoft.AspNetCore.Mvc;
using BEZNgCore.Web.Controllers;

namespace BEZNgCore.Web.Public.Controllers;

public class HomeController : BEZNgCoreControllerBase
{
    public ActionResult Index()
    {
        return View();
    }
}

