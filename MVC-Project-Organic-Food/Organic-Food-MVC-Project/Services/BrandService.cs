using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;
        public BrandService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BrandVM>> GetAllAsync()
        {
            return await _context.Brands.Select(b => new BrandVM
            {
                Image = b.Image,
                Name = b.Name,
            }).ToListAsync();
        }
    }
}
