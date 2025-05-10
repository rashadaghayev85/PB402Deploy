using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Brand;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.SliderImage;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BrandController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<BrandVM> brands = await _context.Brands.Select(brand => new BrandVM { Id = brand.Id, Logo = brand.Logo}).ToListAsync();
            
            return View(brands);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (!request.UploadImage.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("UploadImage", "Input type must be only image");
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-01/", fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }

            await _context.Brands.AddAsync(new Brand { Logo = fileName });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null) return NotFound();

            string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-01/", brand.Logo);

            if (!string.IsNullOrEmpty(brand.Logo) && System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            if (!_context.Brands.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Brand brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null) return NotFound();
            return View(new BrandDetailVM { Logo = brand.Logo });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);

            if (brand == null) return NotFound();

            return View(new BrandEditVM { Id = brand.Id, ExistingImage = brand.Logo });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BrandEditVM request)
        {
            if (id == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null) return NotFound();

            if (request.UploadImage != null)
            {
                if (!request.UploadImage.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("UploadImage", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-01/", brand.Logo);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.UploadImage.FileName);
                string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-01/", fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }

                brand.Logo = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
