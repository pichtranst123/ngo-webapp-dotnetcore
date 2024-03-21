using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Models;
using ngo_webapp.Models.Entities;
namespace ngo_webapp.Controllers
{
	public class BlogController(NgoManagementContext dbContext) : Controller
	{
		private readonly NgoManagementContext _dbContext = dbContext;

		public async Task<IActionResult> Index()
		{
			var blogs = await _dbContext.Blogs.ToListAsync();
			return View(blogs);
		}

		public IActionResult Create()
		{
			var appeals = _dbContext.Appeals.ToList();
			ViewBag.Appeals = appeals;

			var userId = HttpContext.Session.GetString("UserID");
			var model = new BlogViewModel { UserId = int.Parse(userId) };
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BlogViewModel model)
		{
			if (ModelState.IsValid)
			{
				var userId = HttpContext.Session.GetString("UserID");

				var blog = new Blog
				{
					Title = model.Title,
					Content = model.Content,
					AppealId = model.AppealId,
					UserId = int.Parse(userId),
					CreationDate = DateTime.Now
				};
				_dbContext.Add(blog);
				await _dbContext.SaveChangesAsync();
				return RedirectToAction("Index", "Home");
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(int id, BlogViewModel model)
		{
			if (id == model.BlogId)
			{
				var userId = HttpContext.Session.GetString("UserID");
				var blog = await _dbContext.Blogs.FindAsync(id);
				if (blog == null)
				{
					return NotFound();
				}

				if (blog.UserId != int.Parse(userId))
				{
					return Unauthorized();
				}

				if (ModelState.IsValid)
				{
					try
					{
						blog.Title = model.Title;
						blog.Content = model.Content;
						blog.AppealId = model.AppealId;

						_dbContext.Update(blog);
						await _dbContext.SaveChangesAsync();
					}
					catch (DbUpdateConcurrencyException)
					{
						if (BlogExists(model.BlogId))
						{
							throw;
						}
						return NotFound();
					}
					return RedirectToAction(nameof(Index));
				}
				return View(model);
			}
			return NotFound();
		}

		private bool BlogExists(int id) => _dbContext.Blogs.Any(e => e.BlogId == id);

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var userId = HttpContext.Session.GetString("UserID");
			var blog = await _dbContext.Blogs.FindAsync(id);

			if (blog != null)
			{
				if (blog.UserId != int.Parse(userId))
				{
					return Unauthorized();
				}
				_dbContext.Blogs.Remove(blog);
				await _dbContext.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return NotFound();
		}
	}
}