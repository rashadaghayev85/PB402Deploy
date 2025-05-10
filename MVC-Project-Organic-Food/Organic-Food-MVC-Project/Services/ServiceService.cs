using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Services
{
    public class ServiceService : IServiceService
    {
        private readonly AppDbContext _context;
        public ServiceService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ServiceVM>> GetAllAsync()
        {
            return await _context.Services.Select(m=>new ServiceVM
            {
                Number = m.Number,
                Logo = m.Logo,
                Name = m.Name,
            }).ToListAsync();
        }
    }

}
