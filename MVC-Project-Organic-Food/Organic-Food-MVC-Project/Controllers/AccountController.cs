using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Organic_Food_MVC_Project.Helpers.Enums;
using Organic_Food_MVC_Project.Models.User;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Account;

namespace Organic_Food_MVC_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM request)
        {
            if(!ModelState.IsValid) return View(request);

            AppUser newUser = new()
            {
                FullName = request.FullName,
                UserName = request.Username,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(newUser,request.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,item.Description);
                }
                return View(request);
            }
            await _userManager.AddToRoleAsync(newUser,Roles.Member.ToString());
            
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            string url = Url.Action("ConfirmEmail", "Account", new {userId=newUser.Id, token =token},Request.Scheme,Request.Host.ToString());

            string html = null;
            using (StreamReader sr = new StreamReader("wwwroot/templates/emailConfirm.html"))
            {
                html = sr.ReadToEnd();
            }
            html = html.Replace("{link-unique}",url);
                _emailService.Send(newUser.Email, "Email confirmation for account.", html);

            return RedirectToAction(nameof(VerifyEmail));
            //return RedirectToAction(nameof(Login));   
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            AppUser existUser = await _userManager.FindByIdAsync(userId);
            await _userManager.ConfirmEmailAsync(existUser,token);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var existUser = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if(existUser == null)
            {
                existUser = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            }
            if (existUser == null)
            {
                ModelState.AddModelError(string.Empty,"Email or Password is wrong!");
                return View(request);
            }

            var result = await _userManager.CheckPasswordAsync(existUser,request.Password);
            if (result == false)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong!");
                return View(request);
            }
            var res = await _signInManager.PasswordSignInAsync(existUser,request.Password,false,false);

            if(res.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Please check your email for confirmation!");
                return View(request);
            }

            return RedirectToAction("index", "home");
        }

    }
}
