using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Category;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Slider;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryVM> categories = await _context.Categories.Select(category => new CategoryVM { Id = category.Id, Image = category.Image, Name = category.Name }).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category is null) return NotFound();
            return View(new CategoryDetailVM { Name = category.Name, Image = category.Image });
        }

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CategoryCreateVM request)
		{
			if (!ModelState.IsValid) return View(request);

			if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == request.Name.ToLower()))
			{
				ModelState.AddModelError("Name", "Category name must be unique");
				return View(request);
			}

			if (request.UploadImage == null || !request.UploadImage.ContentType.StartsWith("image/"))
			{
				ModelState.AddModelError("UploadImage", "Input type must be only image");
				return View(request);
			}

			string fileName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(request.UploadImage.FileName);

			string originalFileName = Path.GetFileName(request.UploadImage.FileName);

			if (await _context.Categories.AnyAsync(c => c.Image.Contains(originalFileName)))
			{
				ModelState.AddModelError("UploadImage", "Image already exists");
				return View(request);
			}

			string filePath = Path.Combine(_environment.WebRootPath, "assets/images/", fileName);

			using (FileStream stream = new FileStream(filePath, FileMode.Create))
			{
				await request.UploadImage.CopyToAsync(stream);
			}

			var category = new Category
			{
				Name = request.Name,
				Image = fileName
			};

			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			var filePath = Path.Combine(_environment.WebRootPath, "assets/images/", category.Image);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}



		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category == null)
			{
				return NotFound();
			}

			var categoryVM = new CategoryEditVM
			{
				Id = category.Id,
				Name = category.Name,
				CurrentImagePath = category.Image
			};

			return View(categoryVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
		{
			if (id != request.Id) return BadRequest();
			if (!ModelState.IsValid) return View(request);

			var category = await _context.Categories.FindAsync(id);
			if (category == null) return NotFound();

			if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == request.Name.ToLower() && c.Id != id))
			{
				ModelState.AddModelError("Name", "Category name must be unique");
				return View(request);
			}

			category.Name = request.Name;

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

				if (await _context.Categories.AnyAsync(c => c.Image.Contains(originalFileName) && c.Id != id))
				{
					ModelState.AddModelError("UploadImage", "Image already exists");
					return View(request);
				}

				string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/", category.Image);
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}

				string filePath = Path.Combine(_environment.WebRootPath, "assets/images/", fileName);

				using (FileStream stream = new FileStream(filePath, FileMode.Create))
				{
					await request.UploadImage.CopyToAsync(stream);
				}

				category.Image = fileName;
			}

			_context.Categories.Update(category);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}

	}
}
