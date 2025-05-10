using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.About;
using Organic_Food_MVC_Project.Data;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AboutController(AppDbContext context,
                               IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.About.Select(m=>new AboutDetailVM
            {
                Description = m.Description,
                Id = m.Id,
                Image= m.Image,
                Title = m.Title
            }).FirstOrDefaultAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var existAbout = await _context.About.Select(m => new AboutEditVM
            {
                Description = m.Description,
                Id = m.Id,
                OldImage = m.Image,
                Title = m.Title,
            }).FirstOrDefaultAsync();
            if (existAbout == null) return NotFound();
            return View(existAbout);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, AboutEditVM request)
        {
            if (id == null) return BadRequest();
            var existAbout = await _context.About.FirstOrDefaultAsync();
            if (existAbout == null) return NotFound();
            if(!ModelState.IsValid) return View(request);

            if(request.NewImage != null)
            {
                if (request.NewImage.Length / 1024 > 1024)
                {
                    ModelState.AddModelError("NewImage","File size must be smaller than 1mb");
                }
                if (!request.NewImage.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("NewImage", "File type must be only image");
                }
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", existAbout.Image);
                if(System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                string newFileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "home-03", newFileName);
                using(FileStream stream = new(newFilePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }
                existAbout.Image = newFileName;
            }
            existAbout.Description= request.Description;
            existAbout.Title= request.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
