using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Banner;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.FreshFruit;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FreshFruitController : Controller
    {
        public readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FreshFruitController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<FreshFruitVM> freshFruits = await _context.FreshFruits.Select(freshFruit => new FreshFruitVM { Id = freshFruit.Id, Img = freshFruit.Img, Title = freshFruit.Title }).ToListAsync();
            return View(freshFruits);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            FreshFruit freshFruit = await _context.FreshFruits.FirstOrDefaultAsync(m => m.Id == id);
            if (freshFruit is null) return NotFound();
            return View(new FreshFruitDetailVM { Title = freshFruit.Title, Image = freshFruit.Img });
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FreshFruitCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            if (request.UploadImage == null || !request.UploadImage.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("UploadImage", "Input type must be only image");
                return View(request);
            }

            string fileName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(request.UploadImage.FileName);

            string originalFileName = Path.GetFileName(request.UploadImage.FileName);



            string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await request.UploadImage.CopyToAsync(stream);
            }

            var freshFruit = new FreshFruit
            {
                Title = request.Title,
                Img = fileName,
            };

            await _context.FreshFruits.AddAsync(freshFruit);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var freshFruit = await _context.FreshFruits.FindAsync(id);
			if (freshFruit == null)
			{
				return NotFound();
			}

			var filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", freshFruit.Img);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			_context.FreshFruits.Remove(freshFruit);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var freshFruit = await _context.FreshFruits.FindAsync(id);
			if (freshFruit == null)
			{
				return NotFound();
			}

			var freshFruitVM = new FreshFruitEditVM
			{
				Id = freshFruit.Id,
				Title = freshFruit.Title,
				CurrentImagePath = freshFruit.Img
			};

			return View(freshFruitVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int? id, FreshFruitEditVM request)
		{
			if (id != request.Id) return BadRequest();

			var freshFruit = await _context.FreshFruits.FindAsync(id);
			if (freshFruit == null) return NotFound();

			freshFruit.Title = request.Title;

			if (request.UploadImage != null)
			{
				if (!request.UploadImage.ContentType.StartsWith("image/"))
				{
					ModelState.AddModelError("UploadImage", "File must be an image");
					return View(request);
				}

				string fileExtension = Path.GetExtension(request.UploadImage.FileName);
				string fileName = Guid.NewGuid().ToString() + fileExtension;

				string originalFileName = Path.GetFileName(request.UploadImage.FileName);

				string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", freshFruit.Img);
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}

				string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fileName);

				using (FileStream stream = new FileStream(filePath, FileMode.Create))
				{
					await request.UploadImage.CopyToAsync(stream);
				}

				freshFruit.Img = fileName;
			}

			_context.FreshFruits.Update(freshFruit);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}
