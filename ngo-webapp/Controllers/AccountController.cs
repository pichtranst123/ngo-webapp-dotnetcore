using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Models;
using ngo_webapp.Models.Entities;
namespace ngo_webapp.Controllers;
public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Signup()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignupViewModel model)
    {
        if (ModelState.IsValid)
        {
            using (var context = new NgoManagementContext())
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    RegistrationDate = DateTime.Now
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            using (var context = new NgoManagementContext())
            {
                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetString("UserID", user.UserId.ToString());
                    HttpContext.Session.SetString("Username", user.Username);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
        }

        return View(model);
    }
    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }

    public async Task<IActionResult> Profile()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = int.Parse(HttpContext.Session.GetString("UserID"));

        using (var context = new NgoManagementContext())
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var donations = await context.Donations
                .Where(d => d.UserId == userId)
                .Include(d => d.Appeals)
                .Select(d => new ProfileViewModel.DonationDetail
                {
                    AppealName = d.Appeals.AppealsName,
                    Amount = d.Amount,
                    DonationDate = d.DonationDate
                }).ToListAsync();

            var model = new ProfileViewModel
            {
                Username = user.Username,
                RegistrationDate = user.RegistrationDate,
                Donations = donations
            };

            return View(model);
        }
    }


}
