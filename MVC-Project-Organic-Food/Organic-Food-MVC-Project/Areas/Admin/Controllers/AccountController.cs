using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.Account;
using Organic_Food_MVC_Project.Helpers.Enums;
using Organic_Food_MVC_Project.Models.User;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Users = await _userManager.Users.ToListAsync();

            List<UserRolesDto> userWithRoles = new List<UserRolesDto>();

            foreach (var user in Users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = userRoles.ToList(),

                });
            }
            return View(userWithRoles);
        }
        [HttpGet]
        public async Task<IActionResult> AddRole(string? id)
        {
            if (id == null) return BadRequest();
            var existUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (existUser == null) return NotFound();
            var roles = await _roleManager.Roles.Select(m=>new SelectListItem
            {
                Value = m.Name,
                Text = m.Name,
            }).ToListAsync();
            ViewBag.Roles= roles;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string? id,AddRoleVM request)
        {
            if (id == null) return BadRequest();
            var existUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (existUser == null) return NotFound();

            var roles = await _roleManager.Roles.Select(m => new SelectListItem
            {
                Value = m.Name,
                Text = m.Name,
            }).ToListAsync();

            if (!ModelState.IsValid) return View(request);

            var existRoles = await _userManager.GetRolesAsync(existUser);

            if (existRoles.Contains(request.Role))
            {
                ModelState.AddModelError("Role", "This role is user already exist!");
                return View(request);
            }

            await _userManager.AddToRoleAsync(existUser,request.Role);
           

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string? id)
        {
            if (id == null) return BadRequest();
            var existUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (existUser == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(existUser);
            var roles =userRoles.Select(m => new SelectListItem
            {
                Value = m,
                Text = m,
            }).ToList();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string? id,DeleteRoleVM request)
        {
            if (id == null) return BadRequest();
            var existUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (existUser == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(existUser);
            var roles = userRoles.Select(m => new SelectListItem
            {
                Value = m,
                Text = m,
            }).ToList();
            ViewBag.Roles = roles;

            if (!userRoles.Contains(request.Role))
            {
                ModelState.AddModelError("Role", "This role in user doesn't exist!");
                return View(request);
            }

            await _userManager.RemoveFromRoleAsync(existUser,request.Role);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string? id)
        {
            if (id == null) return BadRequest();
            var existUser = await _userManager.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (existUser == null) return NotFound();

            await _userManager.DeleteAsync(existUser);

            return RedirectToAction(nameof(Index));
        }


        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(Roles)))
        //    {
        //        if(!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name=role.ToString()});
        //        }
        //    }
        //    return Ok();
        //}
    }
}
