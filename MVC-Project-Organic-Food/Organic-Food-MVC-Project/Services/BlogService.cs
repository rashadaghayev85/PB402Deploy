using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Blog;

namespace Organic_Food_MVC_Project.Services
{
    public class BlogService:IBlogService
    {
        private readonly AppDbContext _context;
        public BlogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlogVM>> GetAllAsync()
        {
            return await _context.Blogs.Include(m=>m.BlogCategory).Select(b => new BlogVM
            {
                
                Author = b.Author,
                Comment = b.Comment,
                Date = b.Date,
                Description = b.Description,
                Image = b.Image,
                Like = b.Like,
                Title = b.Title,
            }).ToListAsync();
        }
    }
}
