using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        public SliderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SliderVM>> GetAllAsync()
        {
            return await _context.Sliders.Select(x => new SliderVM
            {
                Description = x.Description,
                Image = x.Image,
                Product = x.Product,
                Title = x.Title,
            }).ToListAsync();

        }
    }
}
