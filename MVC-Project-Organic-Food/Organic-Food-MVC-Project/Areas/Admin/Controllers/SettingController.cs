using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.Setting;
using Organic_Food_MVC_Project.Data;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _context.Settings.Select(m => new SettingDetailVM { Id = m.Id, Key = m.Key, Value = m.Value }).ToListAsync();
            return View(settings);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var existSetting = await _context.Settings.Select(m=>new SettingEditVM
            {
                Id = m.Id,
                Key = m.Key,
                Value = m.Value
            }).FirstOrDefaultAsync(m => m.Id == id);
            if (existSetting == null) return NotFound();
            return View(existSetting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,SettingEditVM request)
        {
            if (id == null) return BadRequest();
            var existSetting = await _context.Settings.FirstOrDefaultAsync(m => m.Id == id);
            if (existSetting == null) return NotFound();

            existSetting.Value = request.Value;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
