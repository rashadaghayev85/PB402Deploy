using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.SliderImage;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderImageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderImageController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            SliderImage sliderImage = _context.SliderImages.FirstOrDefault();
            if (sliderImage is null) return View(null);
            return View(new SliderImageVM { Id = sliderImage.Id, BackgroundImage = sliderImage.BackgroundImage });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderImageCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (!request.UploadImage.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("UploadImage", "Input type must be only image");
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.UploadImage.FileName;
            string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }

            await _context.SliderImages.AddAsync(new SliderImage { BackgroundImage = fileName });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            SliderImage sliderImage = await _context.SliderImages.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderImage == null) return NotFound();

            string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", sliderImage.BackgroundImage);

            if (!string.IsNullOrEmpty(sliderImage.BackgroundImage) && System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.SliderImages.Remove(sliderImage);
            await _context.SaveChangesAsync();

            if (!_context.SliderImages.Any()) 
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            SliderImage sliderImage = await _context.SliderImages.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderImage == null) return NotFound();
            return View(new SliderImageDetailVM { Image = sliderImage.BackgroundImage });
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            SliderImage sliderImage = await _context.SliderImages.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderImage == null) return NotFound();

            return View(new SliderImageEditVM { Id = sliderImage.Id, ExistingImage = sliderImage.BackgroundImage });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderImageEditVM request)
        {
            if (id == null) return BadRequest();

            SliderImage sliderImage = await _context.SliderImages.FirstOrDefaultAsync(m => m.Id == id);
            if (sliderImage == null) return NotFound();

            if (request.UploadImage != null)
            {
                if (!request.UploadImage.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("UploadImage", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", sliderImage.BackgroundImage);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.UploadImage.FileName);
                string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }

                sliderImage.BackgroundImage = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
