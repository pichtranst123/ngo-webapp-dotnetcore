using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Models;
using ngo_webapp.Models.Entities; 
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; 
using System.IO;

namespace ngo_webapp.Controllers;

public class AppealsController : Controller
{
    private readonly NgoManagementContext _dbContext;

    public AppealsController(NgoManagementContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> List()
    {
        var appeals = await _dbContext.Appeals.ToListAsync();
        return View(appeals);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppealViewModelAD model)
    {
        if (ModelState.IsValid)
        {
            var appeal = new Appeal
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
        if (id == null)
        {
            return NotFound();
        }

        var appeal = await _dbContext.Appeals.FindAsync(id);
        if (appeal == null)
        {
            return NotFound();
        }

        var model = new AppealViewModelAD
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, AppealViewModelAD model)
    {
        if (id != model.AppealsId)
        {
            return NotFound();
        }

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
                if (!AppealExists(model.AppealsId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(List));
        }

        return View(model);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var appeal = await _dbContext.Appeals
            .FirstOrDefaultAsync(m => m.AppealsId == id);
        if (appeal == null)
        {
            return NotFound();
        }

        return View(appeal);
    }

    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var appeal = await _dbContext.Appeals.FindAsync(id);
        if (appeal != null)
        {
            _dbContext.Appeals.Remove(appeal);
            await _dbContext.SaveChangesAsync();
        }
        return RedirectToAction(nameof(List));
    }

    private bool AppealExists(int id)
    {
        return _dbContext.Appeals.Any(e => e.AppealsId == id);
    }
}