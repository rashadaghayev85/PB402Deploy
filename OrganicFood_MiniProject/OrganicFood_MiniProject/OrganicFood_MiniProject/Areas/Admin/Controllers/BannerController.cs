using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Banner;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Category;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        public readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BannerController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BannerVM> banners = await _context.Banners.Select(banner => new BannerVM { Id = banner.Id, Name = banner.Name, Image = banner.Image, Page = banner.Page }).ToListAsync();   
            return View(banners);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Banner banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);
            if (banner is null) return NotFound();
            return View(new BannerDetailVM { Name = banner.Name, Image = banner.Image, Page = banner.Page });
        }


		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BannerCreateVM request)
		{
			if (!ModelState.IsValid) return View(request);

			if (request.UploadImage == null || !request.UploadImage.ContentType.StartsWith("image/"))
			{
				ModelState.AddModelError("UploadImage", "Input type must be only image");
				return View(request);
			}

			string fileName = Guid.NewGuid().ToString() + "-" + Path.GetFileName(request.UploadImage.FileName);

			string originalFileName = Path.GetFileName(request.UploadImage.FileName);

			

			string filePath = Path.Combine(_environment.WebRootPath, "assets/images/", fileName);

			using (FileStream stream = new FileStream(filePath, FileMode.Create))
			{
				await request.UploadImage.CopyToAsync(stream);
			}

			var banner = new Banner
			{
				Name = request.Name,
				Image = fileName,
				Page = request.Page,
			};

			await _context.Banners.AddAsync(banner);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var banner = await _context.Banners.FindAsync(id);
			if (banner == null)
			{
				return NotFound();
			}

			var filePath = Path.Combine(_environment.WebRootPath, "assets/images/", banner.Image);
			if (System.IO.File.Exists(filePath))
			{
				System.IO.File.Delete(filePath);
			}

			_context.Banners.Remove(banner);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}


		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var banner = await _context.Banners.FindAsync(id);
			if (banner == null)
			{
				return NotFound();
			}

			var bannerVM = new BannerEditVM
			{
				Id = banner.Id,
				Name = banner.Name,
				Page = banner.Page,
				CurrentImagePath = banner.Image
			};

			return View(bannerVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int? id, BannerEditVM request)
		{
			if (id != request.Id) return BadRequest();

			var banner = await _context.Banners.FindAsync(id);
			if (banner == null) return NotFound();

			banner.Name = request.Name;
			banner.Page = request.Page;

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

				string oldImagePath = Path.Combine(_environment.WebRootPath, "assets/images/", banner.Image);
				if (System.IO.File.Exists(oldImagePath))
				{
					System.IO.File.Delete(oldImagePath);
				}

				string filePath = Path.Combine(_environment.WebRootPath, "assets/images/", fileName);

				using (FileStream stream = new FileStream(filePath, FileMode.Create))
				{
					await request.UploadImage.CopyToAsync(stream);
				}

				banner.Image = fileName;
			}

			_context.Banners.Update(banner);
			await _context.SaveChangesAsync();

			return RedirectToAction(nameof(Index));
		}
	}
}
