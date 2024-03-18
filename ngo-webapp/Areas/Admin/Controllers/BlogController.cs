using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Areas.Admin.Models;
using ngo_webapp.Models.Entities;

namespace ngo_webapp.Areas.Admin.Controllers;
[Area("Admin")]
public class BlogController : Controller
{
    private readonly NgoManagementContext _dbContext;

    public BlogController(NgoManagementContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IActionResult Index()
    {
        return View();
    }
	public async Task<IActionResult> Show()
	{
        var blogs = await _dbContext.Blogs.ToListAsync();
        return View(blogs);
	}
	public async Task<IActionResult> Add(BlogViewModel model)
	{
		Blog bl = new();
		try
		{
			bl.BlogId = model.BlogId;
			bl.Title = model.Title;
			bl.Content = model.Content;
			bl.UserId = model.UserID;
			bl.CreationDate = model.CreationDate;
			bl.AppealId = model.AppealID;
			await _dbContext.Blogs.AddAsync(bl);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction("Show", "Blog");
		}
		catch (Exception)
		{

			throw;
		}
	}
}
