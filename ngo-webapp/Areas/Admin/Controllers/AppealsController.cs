using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Areas.Admin.Models;
using ngo_webapp.Areas.Admin.Models.Entities;

namespace ngo_webapp.Areas.Admin.Controllers;
[Area("Admin")]
public class AppealsController(NGODbContext dbContext) : Controller
{
	private readonly NGODbContext dbContext = dbContext;


	// ---------- List ---------- //
	[HttpGet]
	public async Task<IActionResult> List()
	{
		var events = await dbContext.Appeals.ToListAsync();
		return View(events);
	}


	// ---------- Create ---------- //
	[HttpGet]
	public IActionResult Create() => View();

	[HttpPost]
	public async Task<IActionResult> CreateEvent(EventViewModel model)
	{
		try
		{
			Appeal events = new()
			{
				AppealsName = model.AppealsName,
				Organization = model.Organization,
				Description = model.Description,
				EndDate = model.EndDate,
				Amount = model.Amount,
				AppealsImage = model.AppealsImage,
			};
			await dbContext.Appeals.AddAsync(events);
			await dbContext.SaveChangesAsync();
			return RedirectToAction("List", "Event");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
		return NotFound();
	}


	// ---------- Update ---------- //
	[HttpGet]
	public async Task<IActionResult> Update(int id)
	{
		var events = await dbContext.Appeals.FindAsync(id);
		return View(events);
	}

	[HttpPost]
	public async Task<IActionResult> UpdateEvent(Appeal viewModel)
	{
		try
		{
			var events = await dbContext.Appeals.FindAsync(viewModel.AppealsId);
			if (events != null)
			{
				events.AppealsName = viewModel.AppealsName;
				events.Organization = viewModel.Organization;
				events.Description = viewModel.Description;
				events.EndDate = viewModel.EndDate;
				events.Amount = viewModel.Amount;
				events.AppealsImage = viewModel.AppealsImage;

				await dbContext.SaveChangesAsync();
			}
			return RedirectToAction("List", "Event");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
		return NotFound();
	}


	// ---------- Delete ---------- //
	[HttpPost]
	public async Task<IActionResult> DeleteEvent(Appeal viewModel)
	{
		try
		{
			var events = await dbContext.Appeals.AsNoTracking().FirstOrDefaultAsync(x => x.AppealsId == viewModel.AppealsId);
			if (events != null)
			{
				dbContext.Appeals.Remove(events);
				await dbContext.SaveChangesAsync();
			}
			return RedirectToAction("List", "Event");
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
		}
		return NotFound();
	}
}
