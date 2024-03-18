using Microsoft.AspNetCore.Mvc;

namespace ngo_webapp.Areas.Admin.Controllers;
public class AdminController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}
