using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Account;
using OrganicFood_MiniProject.Helpers.Enums;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            List<UserRolesDto> userWithRoles = new();

            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new UserRolesDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Roles = userRoles.ToList()
                });
            }

            return View(userWithRoles);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> AddRoleToUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var model = new AddRoleToUserDto
            {
                UserId = userId,
                Roles = roles.Select(r => new SelectListItem
                {
                    Value = r.Id,
                    Text = r.Name
                }).ToList()
            };

            return View(model);
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserDto model)
        {
            if (string.IsNullOrEmpty(model.RoleId))
            {
                ModelState.AddModelError("", "Please select a role.");
                model.Roles = await _roleManager.Roles
                    .Select(r => new SelectListItem { Value = r.Id, Text = r.Name })
                    .ToListAsync();
                return View(model);
            }

            model.Roles = await _roleManager.Roles
                .Select(r => new SelectListItem { Value = r.Id, Text = r.Name })
                .ToListAsync();

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }

            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                return View(model);
            }

            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                ModelState.AddModelError("", "User already has this role");
                return View(model);
            }

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Role '{role.Name}' added to user '{user.UserName}'.";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                Console.WriteLine($"Error adding role: {error.Description}");
            }

            return View(model);
        }





        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new DeleteRoleDto
            {
                UserId = id,
                Roles = userRoles.Select(role => new SelectListItem
                {
                    Value = role,
                    Text = role
                }).ToList()
            };

            return View(model);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(DeleteRoleDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (string.IsNullOrEmpty(model.RoleId))
            {
                ModelState.AddModelError("", "Please select a role to remove.");
                return View(model);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Count == 1 && userRoles.Contains("Member"))
            {
                ModelState.AddModelError("", "User must have at least member role.");
                return View(model);
            }

            if (model.RoleId == "Member" && userRoles.Count == 1)
            {
                ModelState.AddModelError("", "Cannot remove the member role");
                return View(model);
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleId);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            userRoles = await _userManager.GetRolesAsync(user);
            if (!userRoles.Any())
            {
                await _userManager.AddToRoleAsync(user, "Member");
            }

            TempData["SuccessMessage"] = $"Role '{model.RoleId}' removed from user '{user.UserName}'.";
            return RedirectToAction("Index");
        }




        //[HttpGet]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(Roles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }

        //    return Ok();
        //}
    }
}
