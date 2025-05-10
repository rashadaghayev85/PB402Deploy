using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services
{
    public class AboutService : IAboutService
    {
        private readonly AppDbContext _context;
        public AboutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<AboutVM> GetAsync()
        {
            return await _context.About.Select(x => new AboutVM
            {
                Description = x.Description,
                Image = x.Image,
                Title = x.Title,
            }).FirstOrDefaultAsync();
        }
    }
}
