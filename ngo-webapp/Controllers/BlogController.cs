using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Areas.Admin.Models;
using ngo_webapp.Models.Entities;

namespace ngo_webapp.Controllers;
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
	[HttpGet]
    public IActionResult Add()
    {
        return View();
    }
	[HttpPost]
    public async Task<IActionResult> Add(BlogViewModel model)
	{
		Blog bl = new();
		try
		{
			bl.BlogId = model.BlogId;
			bl.Title = model.Title;
			bl.Content = model.Content;
			bl.CreationDate = model.CreationDate;
			bl.UserId = model.;
			await _dbContext.Blogs.AddAsync(bl);
			await _dbContext.SaveChangesAsync();
			return RedirectToAction("Show", "Blog");
		}
		catch (Exception)
		{

			throw;
		}
	}

	public ActionResult Delete(int id)
	{

		var blog = _dbContext.Blogs.Find(id);

		_dbContext.Blogs.Remove(blog);
		_dbContext.SaveChanges();

		return RedirectToAction("Index");
	}

	public ActionResult Edit(int id)
	{

		var blog = _dbContext.Blogs.Find(id);

		return View("Index");
	}

	[HttpPost]
	public ActionResult Edit(BlogViewModel model)
	{
		var blog = _dbContext.Blogs.Find(model.BlogId);

		blog.Title = model.Title;
		blog.Content = model.Content;
		blog.CreationDate = model.CreationDate;


		_dbContext.SaveChanges();

		return RedirectToAction("Index");
	}
}
