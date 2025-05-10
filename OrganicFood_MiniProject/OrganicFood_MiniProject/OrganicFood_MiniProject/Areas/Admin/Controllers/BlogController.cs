using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Blog;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Category;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class BlogController : Controller
	{
		public readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BlogController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
		{
			IEnumerable<BlogVM> blogs = _context.Blogs.Select(blog => new BlogVM { Id = blog.Id, CreatedDate = blog.CreatedDate, Description = blog.Description, Image = blog.Image, Title = blog.Title});
			return View(blogs);
		}

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Blog blog = await _context.Blogs.Include(b => b.Category).FirstOrDefaultAsync(m => m.Id == id);
            if (blog is null) return NotFound();
            return View(new BlogDetailVM {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                Image = blog.Image,
                CreatedDate = blog.CreatedDate,
                CategoryName = blog.Category?.Name
            });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, "assets/images/our-blog/", blog.Image);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.BlogCategories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            var model = new BlogCreateVM
            {
                Categories = categories
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM request)
        {
            var categoryExists = await _context.BlogCategories.AnyAsync(c => c.Id == request.BlogCategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("BlogCategoryId", "Invalid category");
                return View(request);
            }

            if (request.ImageFile == null || request.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Image file is required");
                return View(request);
            }

            if (!request.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "Invalid image file.");
                return View(request);
            }

            string fileExtension = Path.GetExtension(request.ImageFile.FileName);
            string ImageFileName = Guid.NewGuid().ToString() + fileExtension;
            string ImageFilePath = Path.Combine(_environment.WebRootPath, "assets/images/our-blog/", ImageFileName);

            using (FileStream stream = new FileStream(ImageFilePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            Blog newBlog = new Blog
            {
                Title = request.Title,
                Description = request.Description,
                Image = ImageFileName, 
                BlogCategoryId = request.BlogCategoryId
            };

            _context.Blogs.Add(newBlog);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();

            var categories = await _context.BlogCategories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            var model = new BlogEditVM
            {
                Id = blog.Id,
                Title = blog.Title,
                Description = blog.Description,
                Image = blog.Image,
                BlogCategoryId = blog.BlogCategoryId,
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogEditVM request)
        {
            if (id != request.Id) return BadRequest();

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();

            var categoryExists = await _context.BlogCategories.AnyAsync(c => c.Id == request.BlogCategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("BlogCategoryId", "Invalid category");
                return View(request);
            }

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                if (!request.ImageFile.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Invalid image file");
                    return View(request);
                }

                string fileExtension = Path.GetExtension(request.ImageFile.FileName);
                string ImageFileName = Guid.NewGuid().ToString() + fileExtension;
                string ImageFilePath = Path.Combine(_environment.WebRootPath, "assets/images/our-blog/", ImageFileName);

                using (FileStream stream = new FileStream(ImageFilePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(blog.Image))
                {
                    string oldFilePath = Path.Combine(_environment.WebRootPath, "assets/images/our-blog/", blog.Image);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                blog.Image = ImageFileName;
            }

            blog.Title = request.Title;
            blog.Description = request.Description;
            blog.BlogCategoryId = request.BlogCategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
