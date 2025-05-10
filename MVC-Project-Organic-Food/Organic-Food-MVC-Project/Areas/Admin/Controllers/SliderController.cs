using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.Slider;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Home;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context,
                                IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.Select(m => new SliderDetailVM
            {
                Id = m.Id,
                Description = m.Description,
                Image = m.Image,
                Product = m.Product,
                Title = m.Title,
            }).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var sliders = await _context.Sliders.Select(m => new SliderDetailVM
            {
                Id = m.Id,
                Description = m.Description,
                Image = m.Image,
                Product = m.Product,
                Title = m.Title,
            }).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (!request.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Image", "File type must be only image");
                return View(request);
            }
            if (request.Image.Length / 1024 > 1024)
            {
                ModelState.AddModelError("Image", "File size must be smaller than 1 mb");
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", fileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }


            Slider slider = new Slider()
            {
                Title = request.Title,
                Description = request.Description,
                Image = fileName,
                Product = request.Product
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existSlider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (existSlider == null) return NotFound();

            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", existSlider.Image);
            if (System.IO.File.Exists(existSlider.Image))
            {
                System.IO.File.Delete(existSlider.Image);
            }

            _context.Sliders.Remove(existSlider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return BadRequest();
            var existSlider= await _context.Sliders.Select(m=>new SliderEditVM{
                Id = m.Id,
                Description = m.Description,
                OldImage = m.Image,
                Title = m.Title,
                Product = m.Product,
            }).FirstOrDefaultAsync(s => s.Id == id);
            if (existSlider == null) return NotFound();

            return View(existSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,SliderEditVM request)
        {
            if (id == null) return BadRequest();
            var existSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (existSlider == null) return NotFound();
            if (!ModelState.IsValid) return View(request);
            if (request.NewImage != null)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", existSlider.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                string newFileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", newFileName);
                using(FileStream stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }
                existSlider.Image = newFileName;
            }

            existSlider.Product=request.Product;
            existSlider.Description=request.Description;
            existSlider.Title=request.Title;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    } 
}
