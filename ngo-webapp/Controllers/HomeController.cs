using Microsoft.AspNetCore.Mvc;
using ngo_webapp.Models;
using System.Diagnostics;
using ngo_webapp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace donate_webapp.Controllers;

public class HomeController : Controller
{
    private readonly NgoManagementContext _context;

    public HomeController(NgoManagementContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var appeals = await _context.Appeals.Include(a => a.Donations).ToListAsync();
        return View(appeals);
    }


    [HttpPost]
    public async Task<IActionResult> Donate(int appealId, decimal amount)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = int.Parse(HttpContext.Session.GetString("UserID"));

        var appeal = await _context.Appeals.FindAsync(appealId);
        if (appeal == null)
        {
            return NotFound();
        }

        var totalDonations = await _context.Donations
            .Where(d => d.AppealsId == appealId)
            .SumAsync(d => d.Amount);

        if (totalDonations + amount > appeal.Amount)
        {
            TempData["Error"] = "Donation amount exceeds the required amount for this appeal.";
            return RedirectToAction("Index");
        }

        var donation = new Donation
        {
            UserId = userId,
            AppealsId = appealId,
            Amount = amount,
            DonationDate = DateTime.Now
        };

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

}
