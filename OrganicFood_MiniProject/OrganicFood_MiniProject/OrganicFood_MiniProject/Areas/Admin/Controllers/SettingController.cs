using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Setting;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _context.Settings.Select(setting => new SettingVM { Id = setting.Id, Key = setting.Key, Value = setting.Value}).ToListAsync();
            return View(settings);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
                return NotFound();

            var model = new SettingEditVM
            {
                Id = setting.Id,
                Key = setting.Key,
                Value = setting.Value
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SettingEditVM model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            var setting = await _context.Settings.FindAsync(id);
            if (setting == null)
                return NotFound();

            setting.Key = model.Key;
            setting.Value = model.Value;

            _context.Update(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
