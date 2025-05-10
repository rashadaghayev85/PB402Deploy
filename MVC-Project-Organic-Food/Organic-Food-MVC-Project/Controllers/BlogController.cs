using Microsoft.AspNetCore.Mvc;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Blog;

namespace Organic_Food_MVC_Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBlogCategoryService _blogCategoryService;
        public BlogController(IBlogService blogService,
                              IBlogCategoryService blogCategoryService)
        {
            _blogCategoryService = blogCategoryService;
            _blogService = blogService;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<BlogVM> blogs=await _blogService.GetAllAsync();
            IEnumerable<BlogCategoryVM> blogCategories= await _blogCategoryService.GetAllAsync();

            BlogAllVM result = new BlogAllVM()
            {
                Blogs = blogs,
                BlogCategories = blogCategories
            };
            return View(result);
        }
    }
}
