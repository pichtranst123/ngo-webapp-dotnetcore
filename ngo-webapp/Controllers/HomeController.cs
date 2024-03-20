using Microsoft.AspNetCore.Mvc;
using ngo_webapp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace donate_webapp.Controllers;

public class HomeController(NgoManagementContext context) : Controller
{
	private readonly NgoManagementContext _context = context;

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
		var user = await _context.Users.FindAsync(userId);
		if (user == null)
		{
			return NotFound("User not found");
		}

		var appeal = await _context.Appeals.FindAsync(appealId);
		if (appeal == null)
		{
			return NotFound("Appeal not found");
		}

		// Calculate the total donations for this appeal to ensure it doesn't exceed the goal
		var totalDonationsForAppeal = await _context.Donations
			.Where(d => d.AppealsId == appealId).SumAsync(d => d.Amount);
		if (totalDonationsForAppeal + amount > appeal.Amount)
		{
			ModelState.AddModelError("", "Donation exceeds the appeal goal.");
			return View(); // Return with error
		}

		// Check if the user has enough balance
		if (user.Balance < amount)
		{
			ModelState.AddModelError("", "Insufficient balance for this donation.");
			return View(); // Return with error
		}

		// Deduct the donation amount from the user's balance
		user.Balance -= amount;

		// Create and save the donation record
		Donation donation = new()
		{
			UserId = userId,
			AppealsId = appealId,
			Amount = amount,
			DonationDate = DateTime.Now
		};
		_context.Donations.Add(donation);

		_context.Users.Update(user);
		await _context.SaveChangesAsync();
		return RedirectToAction("Index");
	}


	public async Task<IActionResult> BlogDetail(int appealId)
	{
		var blogs = await _context.Blogs
			.Where(b => b.AppealId == appealId)
			.ToListAsync(); // Temporarily remove any .Include() calls

		return View(blogs);
	}
}


