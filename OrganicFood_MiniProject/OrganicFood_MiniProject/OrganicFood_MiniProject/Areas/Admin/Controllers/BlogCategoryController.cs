using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.BlogCategory;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Slider;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BlogCategoryController : Controller
	{
		private readonly AppDbContext _context;

        public BlogCategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
		{
			IEnumerable<BlogCategoryVM> blogCategories = await _context.BlogCategories.Select(blogCategory => new BlogCategoryVM { Id = blogCategory.Id, Name = blogCategory.Name }).ToListAsync();
			return View(blogCategories);
		}


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var blogCategory = new BlogCategory
            {
                Name = request.Name,
            };

            await _context.BlogCategories.AddAsync(blogCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();
            BlogCategory blogCategory = await _context.BlogCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (blogCategory == null) return NotFound();

            _context.BlogCategories.Remove(blogCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var blogCategory = await _context.BlogCategories.FindAsync(id);
            if (blogCategory == null)
            {
                return NotFound();
            }

            var blogCategoryVM = new BlogCategoryEditVM
            {
                Id = blogCategory.Id,
                Name = blogCategory.Name,
            };

            return View(blogCategoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogCategoryEditVM request)
        {
            if (id != request.Id) return BadRequest();
            if (!ModelState.IsValid) return View(request);

            var blogCategory = await _context.BlogCategories.FindAsync(id);
            if (blogCategory == null) return NotFound();

            blogCategory.Name = request.Name;

            _context.BlogCategories.Update(blogCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
