using Microsoft.AspNetCore.Mvc;

namespace ngo_webapp.Areas.Admin.Controllers;
[Area("Admin")]
public class BlogController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
