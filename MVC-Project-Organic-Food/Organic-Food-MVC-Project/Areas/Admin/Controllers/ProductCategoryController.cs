using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.ProductCategory;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Home;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly AppDbContext _context;
        public ProductCategoryController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productCategories = await _context.ProductCategories.Include(m => m.Products).Select(m => new ProductCategoryDetailVM
            {
                
                Id = m.Id,
                HasProduct = m.Products.Any(),
                Logo = m.Logo,
                Name = m.Name,
            }).ToListAsync();

            return View(productCategories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategoryCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var existCategory = await _context.ProductCategories.FirstOrDefaultAsync(m=>m.Name==request.Name);
            if (existCategory != null)
            {
                ModelState.AddModelError("Name", "This Category already exist!");
                return View(request);
            }
            await _context.ProductCategories.AddAsync(new ProductCategory { Name=request.Name});
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest(); 
            var existCategory = await _context.ProductCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory == null) return NotFound();
            _context.ProductCategories.Remove(existCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var existCategory = await _context.ProductCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory == null) return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,ProductCategoryEditVM request)
        {
            if (id == null) return BadRequest();
            var existCategory = await _context.ProductCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (existCategory == null) return NotFound();

            if(await _context.ProductCategories.FirstOrDefaultAsync(m => m.Name == request.Name && m.Id!=id)!=null)
            {
                ModelState.AddModelError("Name", "This Category already exist!");
                return View(request);
            }
            existCategory.Name = request.Name;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
