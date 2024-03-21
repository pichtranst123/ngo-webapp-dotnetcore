using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Models;
using ngo_webapp.Models.Entities;

namespace ngo_webapp.Controllers;
public class AdminController : Controller
{
	public IActionResult LoginAdmin() => View();

	public async Task<IActionResult> LoginAdminPOST(AdminLoginViewModel model)
	{
		if (ModelState.IsValid)
		{
			using (NgoManagementContext context = new())
			{
				var user = await context.Users
					.FirstOrDefaultAsync(u => u.Email == model.Email);

				if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
				{
					HttpContext.Session.SetString("UserID", user.UserId.ToString());
					HttpContext.Session.SetString("Is_Admin", user.IsAdmin.ToString());
					if (user.IsAdmin != false)
					{
						return RedirectToAction("Index", "Admin");
					}
				}
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				return View(model);
			}
		}
		return View(model);
	}

	public IActionResult Index() => View();

}
