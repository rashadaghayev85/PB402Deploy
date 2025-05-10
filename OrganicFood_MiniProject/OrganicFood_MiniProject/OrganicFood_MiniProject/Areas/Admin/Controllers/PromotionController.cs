using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.FreshFruit;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Promotion;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class PromotionController : Controller
	{
		public readonly AppDbContext _context;
		private readonly IWebHostEnvironment _environment;

		public PromotionController(AppDbContext context, IWebHostEnvironment environment)
		{
			_context = context;
			_environment = environment;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			IEnumerable<PromotionVM> promotions = await _context.Promotions.Select(promotion => new PromotionVM { Id = promotion.Id, Img = promotion.Image, Title = promotion.Title, Description = promotion.Description }).ToListAsync();
			return View(promotions);
		}

		[HttpGet]
		public async Task<IActionResult> Detail(int? id)
		{
			if (id is null) return BadRequest();
			Promotion promotion = await _context.Promotions.FirstOrDefaultAsync(m => m.Id == id);
			if (promotion is null) return NotFound();
			return View(new PromotionDetailVM { Title = promotion.Title, Image = promotion.Image, Description = promotion.Description });
		}


		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(PromotionCreateVM request)
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

			var promotion = new Promotion
			{
				Title = request.Title,
				Image = fileName,
				Description = request.Description,
			};

			await _context.Promotions.AddAsync(promotion);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var promotion = await _context.Promotions.FindAsync(id);
			if (promotion == null)
			{
				return NotFound();
			}

			var filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", promotion.Image);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			_context.Promotions.Remove(promotion);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var promotion = await _context.Promotions.FindAsync(id);
			if (promotion == null)
			{
				return NotFound();
			}

			var promotionVM = new PromotionEditVM
			{
				Id = promotion.Id,
				Title = promotion.Title,
				CurrentImagePath = promotion.Image,
				Description = promotion.Description,
			};

			return View(promotionVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int? id, PromotionEditVM request)
		{
			if (id != request.Id) return BadRequest();

			var promotion = await _context.Promotions.FindAsync(id);
			if (promotion == null) return NotFound();

			promotion.Title = request.Title;
			promotion.Description = request.Description;

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

				string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", promotion.Image);
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}

				string filePath = Path.Combine(_environment.WebRootPath, "assets/images/home-03/", fileName);

				using (FileStream stream = new FileStream(filePath, FileMode.Create))
				{
					await request.UploadImage.CopyToAsync(stream);
				}

				promotion.Image = fileName;
			}

			_context.Promotions.Update(promotion);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}
