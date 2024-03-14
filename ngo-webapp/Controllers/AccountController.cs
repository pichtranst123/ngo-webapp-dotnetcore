﻿using Microsoft.AspNetCore.Mvc;
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
                // Check if the username or email already exists
                var existingUserWithUsername = await context.Users
                    .AnyAsync(u => u.Username == model.Username);
                var existingUserWithEmail = await context.Users
                    .AnyAsync(u => u.Email == model.Email);

                if (existingUserWithUsername)
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                }

                if (existingUserWithEmail)
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                }

                if (existingUserWithUsername || existingUserWithEmail)
                {
                    // Return the view with validation errors
                    return View(model);
                }

                // Proceed with creating the user if the username and email are unique
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    RegistrationDate = DateTime.Now,
                    //Balance = 10000
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return RedirectToAction("Login", "Account");
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
                //Balance = user.Balance,
                Donations = donations,
                TotalAmount = donations.Sum(d => d.Amount), // Calculate the total donated amount
                ProjectCount = donations.Select(d => d.AppealId).Distinct().Count()
            };

            return View(model);
        }
    }


}
