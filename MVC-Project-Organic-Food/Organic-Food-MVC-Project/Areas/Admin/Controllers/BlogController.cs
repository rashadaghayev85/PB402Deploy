using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.Blog;
using Organic_Food_MVC_Project.Data;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BlogController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Blogs.Include(m=>m.BlogCategory).Select(m=> new BlogDetailVM
            {
                Id = m.Id,
                Author=m.Author,
                CategoryName=m.BlogCategory.Name,
                Comment=m.Comment,
                Description=m.Description,
                Date=m.Date,
                Image=m.Image,
                Like=m.Like,
                Title=m.Title,
            }).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _context.Blogs.Include(m => m.BlogCategory).Select(m => new BlogDetailVM
            {
                Id = m.Id,
                Author = m.Author,
                CategoryName = m.BlogCategory.Name,
                Comment = m.Comment,
                Description = m.Description,
                Date = m.Date,
                Image = m.Image,
                Like = m.Like,
                Title = m.Title,
            }).FirstOrDefaultAsync(m=>m.Id==id));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var blogCategories = await _context.BlogCategories.Select(m=>new SelectListItem
            {
                Value=m.Id.ToString(),
                Text=m.Name
            }).ToListAsync();
            ViewBag.BlogCategories = blogCategories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM request)
        {
            var blogCategories = await _context.BlogCategories.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToListAsync();

            ViewBag.BlogCategories = blogCategories;

            if (!ModelState.IsValid) return View(request);

            string fileName=Guid.NewGuid().ToString() + "-" + request.Image.FileName;
            string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "our-blog", fileName);
            using(FileStream stream = new(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            await _context.Blogs.AddAsync(new()
            {
                Image=fileName,
                Author=request.Author,
                Description=request.Description,
                Date=request.Date,
                Title=request.Title,
                BlogCategoryId=request.CategoryId,
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existBlog= await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (existBlog == null) return NotFound();

            string filePath= Path.Combine(_env.WebRootPath, "assets", "images", "our-blog", existBlog.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Blogs.Remove(existBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id== null) return BadRequest();
            var existBlog=await _context.Blogs.FirstOrDefaultAsync(m=>m.Id== id);
            if (existBlog == null) return NotFound();
            var blogCategories = await _context.BlogCategories.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToListAsync();

            ViewBag.BlogCategories = blogCategories;

            return View(await _context.Blogs.Include(m=>m.BlogCategory).Select(m=>new BlogEditVM
            {
                Id=m.Id,
                Title = m.Title,
                Author = m.Author,
                Description = m.Description,
                CategoryId=m.BlogCategoryId,
            }).FirstOrDefaultAsync(m=>m.Id==id));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,BlogEditVM request)
        {
            if (id == null) return BadRequest();
            var existBlog = await _context.Blogs.FirstOrDefaultAsync(m => m.Id == id);
            if (existBlog == null) return NotFound();
            var blogCategories = await _context.BlogCategories.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToListAsync();

            ViewBag.BlogCategories = blogCategories;
            if (!ModelState.IsValid) return View(request);

            if (request.NewImage != null)
            {
                string oldFilePath = Path.Combine(_env.WebRootPath, "assets", "images", "our-blog", existBlog.Image);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "our-blog", fileName);
                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await request.NewImage.CopyToAsync(stream);
                }
                existBlog.Image = fileName;
            }

            existBlog.Description=request.Description;
            existBlog.Title=request.Title;
            existBlog.Author=request.Author;
            existBlog.BlogCategoryId=request.CategoryId;
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)); 

        }

    }
}
