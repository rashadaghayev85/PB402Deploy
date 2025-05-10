using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.Brand;
using Organic_Food_MVC_Project.Data;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BrandController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            return View(await _context.Brands.Select(m => new BrandDetailVM
            {
                Id = m.Id,
                Name = m.Name,
                Image = m.Image,
            }).ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            await _context.Brands.AddAsync(new()
            {
                Name = request.Name,
                Image = fileName,
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existBrand=await _context.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (existBrand == null) return NotFound();

            string filePath=Path.Combine(_env.WebRootPath, "assets", "images", "home-03", existBrand.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Brands.Remove(existBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return BadRequest();
            var existBrand = await _context.Brands.Select(m=>new BrandEditVM
            {
                Id = m.Id,
                OldImage = m.Image,
                NewName = m.Name,
            }).FirstOrDefaultAsync(m => m.Id == id);
            if (existBrand == null) return NotFound();
            return View(existBrand);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,BrandEditVM request)
        {
            if (id == null) return BadRequest();
            var existBrand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
            if (existBrand == null) return NotFound();
            if(ModelState.IsValid) return View(request);
            if (request.NewImage != null)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", existBrand.Image);
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                string newFilename = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", newFilename);
                using (FileStream stream = new FileStream(newFilePath, FileMode.Create))
                {
                   await request.NewImage.CopyToAsync(stream);
                }
                existBrand.Image = newFilename;
            }
            existBrand.Name = request.NewName;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
