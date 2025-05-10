using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;
using OrganicFood_MiniProject.ViewModels;

namespace OrganicFood_MiniProject.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<HeaderVM> GetAllAsync()
        {
            var settings = await _context.Settings
            .ToDictionaryAsync(m => m.Key, m => m.Value);

            var categories = await _context.Categories
                .Select(c => new CategoryVM
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();

            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            return new HeaderVM
            {
                Settings = settings,
                Categories = categories,
                CurrencyOptions = settings.ContainsKey("CurrencyOptions")
                                  ? settings["CurrencyOptions"].Split(", ").ToList()
                                  : new List<string>(),

                LanguageOptions = settings.ContainsKey("LanguageOptions")
                                  ? settings["LanguageOptions"].Split(", ").ToList()
                                  : new List<string>(),
                UserFullName = user?.FullName

            };
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var datas = await GetAllAsync();
            return View(datas);
        }
    }
}
