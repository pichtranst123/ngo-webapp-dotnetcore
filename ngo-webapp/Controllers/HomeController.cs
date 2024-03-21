using Microsoft.AspNetCore.Mvc;
using ngo_webapp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ngo_webapp.Models;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
			return View("NotFoundPage");
		}

		var appeal = await _context.Appeals.FindAsync(appealId);
		if (appeal == null)
		{
			return View("NotFoundPage");
		}

		// Calculate the total donations for this appeal to ensure it doesn't exceed the goal
		var totalDonationsForAppeal = await _context.Donations
			.Where(d => d.AppealsId == appealId).SumAsync(d => d.Amount);
		if (totalDonationsForAppeal + amount > appeal.Amount)
		{
			ModelState.AddModelError("", "Donation exceeds the appeal goal.");
			return View();
		}

		// Check if the user has enough balance
		if (user.Balance < amount)
		{
			ModelState.AddModelError("", "Insufficient balance for this donation.");
			return View();
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

	[Route("Home/BlogDetail/{appealId:int}")]
	public async Task<IActionResult> BlogDetail(int appealId)
	{
		var blog = await _context.Blogs
			.FirstOrDefaultAsync(b => b.AppealId == appealId);
		if (blog == null)
		{
			return View("NotFoundPage");
		}
		var comments = await _context.Comments
			.Where(c => c.BlogId == blog.BlogId)
			.ToListAsync();


		var blogViewModel = new BlogViewModel
		{
			BlogId = blog.BlogId,
			Title = blog.Title,
			Content = blog.Content,
			CreationDate = blog.CreationDate,
		};

		var model = Tuple.Create(blogViewModel, comments.AsEnumerable());

		return View(model);
	}


	[HttpPost]
	public async Task<IActionResult> AddComment(int BlogId, string Content)
	{
		if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserID")))
		{
			return RedirectToAction("Login", "Account");
		}
		var blog = await _context.Blogs.FindAsync(BlogId); // Make sure this line exists before you use 'blog'

		var userIdString = HttpContext.Session.GetString("UserID") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
		if (!int.TryParse(userIdString, out var userId))
		{
			return RedirectToAction("BlogDetail", new { id = BlogId });
		}

		var comment = new Comment
		{
			Content = Content,
			CreationDate = DateTime.Now,
			UserId = userId,
			BlogId = BlogId
			// ParentCommentId can be set here if you're implementing nested comments
		};

		_context.Comments.Add(comment);
		await _context.SaveChangesAsync();

		return RedirectToAction("BlogDetail", new { id = blog.AppealId });
	}

	public IActionResult NotFoundPage() => View();


}


