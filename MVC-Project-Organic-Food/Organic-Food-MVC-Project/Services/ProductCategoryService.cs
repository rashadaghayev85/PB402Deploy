using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly AppDbContext _context;
        public ProductCategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            return await _context.ProductCategories.Include(m => m.Products).Select(m => new CategoryVM 
            { 
                Name = m.Name,
                Id = m.Id,
                Logo = m.Logo,
                HasProduct=m.Products.Any(),
            }).ToListAsync();
        }
    }
}
