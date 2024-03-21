using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Models;
using ngo_webapp.Models.Entities;

namespace ngo_webapp.Controllers;
public class AppealsController(NgoManagementContext dbContext) : Controller
{
	private readonly NgoManagementContext _dbContext = dbContext;

	public async Task<IActionResult> List()
	{
		var appeals = await _dbContext.Appeals.ToListAsync();
		return View(appeals);
	}

	public IActionResult Create() => View();

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(AppealViewModel model)
	{
		if (ModelState.IsValid)
		{
			Appeal appeal = new()
			{
				AppealsName = model.AppealsName,
				Organization = model.Organization,
				Description = model.Description,
				EndDate = model.EndDate,
				Amount = model.Amount,
				CreationDate = DateTime.Now, // Set automatically
				Status = true // Default value
			};

			if (model.AppealImageFile != null && model.AppealImageFile.Length > 0)
			{
				var fileName = Path.GetFileName(model.AppealImageFile.FileName);
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await model.AppealImageFile.CopyToAsync(stream);
				}
				appeal.AppealsImage = fileName;
			}
			_dbContext.Add(appeal);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction(nameof(List));
		}
		return View(model);
	}

	public async Task<IActionResult> Update(int? id)
	{
		if (id != null)
		{
			var appeal = await _dbContext.Appeals.FindAsync(id);
			if (appeal == null)
			{
				return NotFound();
			}

			AppealViewModel model = new()
			{
				AppealsId = appeal.AppealsId,
				AppealsName = appeal.AppealsName,
				Organization = appeal.Organization,
				Description = appeal.Description,
				EndDate = appeal.EndDate,
				Amount = appeal.Amount,
				ExistingImagePath = appeal.AppealsImage
			};
			return View(model);
		}
		return NotFound();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Update(int id, AppealViewModel model)
	{
		if (id == model.AppealsId)
		{
			if (ModelState.IsValid)
			{
				var appeal = await _dbContext.Appeals.FindAsync(id);
				if (appeal == null)
				{
					return NotFound();
				}

				appeal.AppealsName = model.AppealsName;
				appeal.Organization = model.Organization;
				appeal.Description = model.Description;
				appeal.EndDate = model.EndDate;
				appeal.Amount = model.Amount;

				if (model.AppealImageFile != null && model.AppealImageFile.Length > 0)
				{
					var fileName = Path.GetFileName(model.AppealImageFile.FileName);
					var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await model.AppealImageFile.CopyToAsync(stream);
					}
					appeal.AppealsImage = fileName; // Update the file name after upload
				}

				try
				{
					_dbContext.Update(appeal);
					await _dbContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (AppealExists(model.AppealsId))
					{
						throw;
					}
					return NotFound();
				}
				return RedirectToAction(nameof(List));
			}
			return View(model);
		}
		return NotFound();
	}

	//public async Task<IActionResult> Delete(int? id)
	//{
	//	if (id != null)
	//	{
	//		var appeal = await _dbContext.Appeals.FirstOrDefaultAsync(m => m.AppealsId == id);
	//		if (appeal != null)
	//		{
	//			return View(appeal);
	//		}
	//		else
	//		{
	//			return NotFound();
	//		}
	//	}
	//	return NotFound();
	//}

	//[HttpPost, ActionName("DeleteConfirmed")]
	//[ValidateAntiForgeryToken]
	//public async Task<IActionResult> DeleteConfirmed(int id)
	//{
	//	var appeal = await _dbContext.Appeals.FindAsync(id);
	//	if (appeal != null)
	//	{
	//		_dbContext.Appeals.Remove(appeal);
	//		await _dbContext.SaveChangesAsync();
	//	}
	//	return RedirectToAction(nameof(List));
	//}

	public async Task<IActionResult> Delete(int? apID, int? doID)
	{
		if (apID != null)
		{
			var donation = await _dbContext.Donations.FirstOrDefaultAsync(n => n.DonationId == doID);
			if (donation != null)
			{
				var appeal = await _dbContext.Appeals.FirstOrDefaultAsync(m => m.AppealsId == apID);
				if (appeal != null)
				{
					_dbContext.Donations.Remove(donation);
					_dbContext.Appeals.Remove(appeal);
					await _dbContext.SaveChangesAsync();

					return RedirectToAction(nameof(List));
				}
				return NotFound();
			}
			return NotFound();
		}
		return NotFound();
	}

	private bool AppealExists(int id) => _dbContext.Appeals.Any(e => e.AppealsId == id);
}