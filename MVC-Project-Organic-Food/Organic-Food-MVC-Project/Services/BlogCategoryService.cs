using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Blog;

namespace Organic_Food_MVC_Project.Services
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly AppDbContext _context;
        public BlogCategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BlogCategoryVM>> GetAllAsync()
        {
            return await _context.BlogCategories.Include(m => m.Blogs).Select(c => new BlogCategoryVM
            {
                HasBlog = c.Blogs.Any(),
                Name = c.Name,
                BlogCount = c.Blogs.Count()
            }).ToListAsync();
        }
    }
}
