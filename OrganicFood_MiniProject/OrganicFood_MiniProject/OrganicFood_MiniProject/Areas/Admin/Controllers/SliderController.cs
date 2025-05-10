using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Slider;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.SliderImage;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _environment;
		public SliderController(AppDbContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		[HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SliderVM> sliders = await _context.Sliders.Select(slider => new SliderVM { Id = slider.Id, FirstTitle = slider.FirstTitle, SecondTitle = slider.SecondTitle, Description = slider.Description, Image = slider.Image }).ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(m => m.Id == id);
            if (slider is null) return NotFound();
            return View(new SliderDetailVM { FirstTitle = slider.FirstTitle,SecondTitle = slider.SecondTitle, Description = slider.Description, Image = slider.Image });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SliderCreateVM request)
		{
			if (!ModelState.IsValid) return View(request);

			if (request.UploadImage == null || !request.UploadImage.ContentType.StartsWith("image/"))
			{
				ModelState.AddModelError("UploadImage", "Input type must be only image");
				return View(request);
			}

			string fileName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(request.UploadImage.FileName);
			string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fileName);

			using (FileStream stream = new FileStream(filePath, FileMode.Create))
			{
				await request.UploadImage.CopyToAsync(stream);
			}

			var slider = new Slider
			{
				Image = fileName, 
				FirstTitle = request.FirstTitle, 
				SecondTitle = request.SecondTitle,
				Description = request.Description 
			};

			await _context.Sliders.AddAsync(slider);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var slider = await _context.Sliders.FindAsync(id);
			if (slider == null)
			{
				return NotFound();
			}

			var filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", slider.Image);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			_context.Sliders.Remove(slider);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            var sliderVM = new SliderEditVM
            {
                Id = slider.Id,
                FirstTitle = slider.FirstTitle,
                SecondTitle = slider.SecondTitle,
                Description = slider.Description,
                CurrentImagePath = slider.Image  
            };

            return View(sliderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, SliderEditVM request)
        {
            if (id != request.Id) return BadRequest();
            if (!ModelState.IsValid) return View(request);

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            slider.FirstTitle = request.FirstTitle;
            slider.SecondTitle = request.SecondTitle;
            slider.Description = request.Description;

            if (request.UploadImage != null)
            {
                if (!request.UploadImage.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("UploadImage", "File must be an image");
                    return View(request);
                }

                string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", slider.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                string fileExtension = Path.GetExtension(request.UploadImage.FileName);
                string fileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.UploadImage.CopyToAsync(stream);
                }

                slider.Image = fileName;
            }
            else
            {
                slider.Image = request.CurrentImagePath ?? slider.Image;
            }

            _context.Sliders.Update(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
