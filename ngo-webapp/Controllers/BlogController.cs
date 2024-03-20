using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ngo_webapp.Models;
using ngo_webapp.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ngo_webapp.Controllers
{
    public class BlogController : Controller
    {
        private readonly NgoManagementContext _dbContext;

        public BlogController(NgoManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.ToListAsync();
            return View(blogs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogViewModelAD model)
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

        public IActionResult Create()
        {
            var appeals = _dbContext.Appeals.ToList();
            ViewBag.Appeals = appeals;

            var userId = HttpContext.Session.GetString("UserID");
            var model = new BlogViewModelAD { UserId = int.Parse(userId) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, BlogViewModelAD model)
        {
            if (id != model.BlogId)
            {
                return NotFound();
            }

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
                    if (!BlogExists(model.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private bool BlogExists(int id)
        {
            return _dbContext.Blogs.Any(e => e.BlogId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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

            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
