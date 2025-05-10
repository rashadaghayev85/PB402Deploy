using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly AppDbContext _context;
        public AdvertisementService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AdvertisementVM> GetAsync()
        {
            var product = await _context.Products.Include(m=>m.ProductImages).OrderBy(m=>m.Price).FirstOrDefaultAsync();
            var existAdd = await _context.Advertisement.FirstAsync();
            existAdd.Description = product.Description;
            existAdd.Price = product.Price;
            existAdd.Product = product.Name;
            await _context.SaveChangesAsync();

            return await _context.Advertisement.Select(m => new AdvertisementVM
            {
                Description = m.Description,
                Price = m.Price,
                Product = m.Product,
                Title = m.Title,
                ProductId=product.Id,
            }).FirstAsync();
        }
    }
}
