using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.BlogCategory;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Blog;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogCategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogCategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogCategories.Select(m=>new BlogCategoryDetailVM
            {
                Id = m.Id,
                Name=m.Name,
            }).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCategoryCreateVM request)
        {
            if(!ModelState.IsValid) return View(request);

            await _context.BlogCategories.AddAsync(new BlogCategory { Name=request.Name});
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existBlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (existBlogCategory == null) return NotFound();

            _context.BlogCategories.Remove(existBlogCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return BadRequest();
            var existBlogCategory = await _context.BlogCategories.Select(m=>new BlogCategoryEditVM
            {
                Id = m.Id,
                Name = m.Name,
            }).FirstOrDefaultAsync(m => m.Id == id);
            if(existBlogCategory == null) return NotFound();

            return View(existBlogCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,BlogCategoryEditVM request)
        {
            if (id == null) return BadRequest();
            var existBlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (existBlogCategory == null) return NotFound();
            if(!ModelState.IsValid) return View(request);

            existBlogCategory.Name = request.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
