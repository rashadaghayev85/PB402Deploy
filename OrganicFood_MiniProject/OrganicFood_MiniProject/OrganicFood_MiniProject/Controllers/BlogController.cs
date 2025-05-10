using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.ViewModels;

namespace OrganicFood_MiniProject.Controllers
{
    public class BlogController : Controller
    {
        public readonly AppDbContext _context;
        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? categoryId)
        {
            var blogsQuery = _context.Blogs.Include(b => b.Category).AsQueryable();

            if (categoryId.HasValue)
            {
                blogsQuery = blogsQuery.Where(b => b.BlogCategoryId == categoryId.Value);
            }

            var blogs = blogsQuery.Select(b => new BlogVM
            {
                Id = b.Id,
                Title = b.Title,
                Image = b.Image,
                Description = b.Description,
                CreatedDate = b.CreatedDate,
                CategoryId = b.BlogCategoryId,
                BlogCategory = b.Category,
                Banners = _context.Banners
                    .Where(bn => bn.Page == "Blog")
                    .Select(bn => new BannerVM { Name = bn.Name, Image = bn.Image, Page = bn.Page })
                    .ToList()
            }).ToList();

            ViewBag.Categories = _context.BlogCategories.ToList();

            return View(blogs);
        }

    }
}
