using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;
using System.Threading.Tasks;

namespace Organic_Food_MVC_Project.Controllers
{
    public class ProductDetailController : Controller
    {
        private readonly IProductService _productService;
        private readonly AppDbContext _context;
        public ProductDetailController(IProductService productService,
                                       AppDbContext context)
        {
           _productService = productService;
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            //ProductVM product =await _productService.GetByIdAsync((int)id);
            var product = await _context.Products.Include(m => m.ProductCategory).Include(m => m.DiscountProducts)
                                                                                 .Include(m => m.ProductImages)
                                                                                 .FirstOrDefaultAsync(m=>m.Id==id);
            
            if (product == null)
            {
                return RedirectToAction("NotFoundException", "Error");
            }

            return View(product);
        }
    }
}
