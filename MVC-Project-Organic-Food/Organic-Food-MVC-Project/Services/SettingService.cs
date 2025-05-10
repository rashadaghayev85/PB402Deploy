using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Services.Interfaces;

namespace Organic_Food_MVC_Project.Services
{
    public class SettingService : ISettingService
    {
        private readonly AppDbContext _context;
        public SettingService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string, string>> GetAllAsync()
        {
          return await _context.Settings.ToDictionaryAsync(m=>m.Key, m=>m.Value);
        }
    }
}
