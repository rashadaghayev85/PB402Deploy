using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Controllers
{
    public class ProductSearchController : Controller
    {
        private readonly AppDbContext _context;
        public ProductSearchController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string searchText)
        {
            var products= await _context.Products.Include(m=>m.ProductImages)
                                                 .Include(m => m.ProductCategory)
                                                 .Include(m => m.DiscountProducts)
                                                 .Select(m=>new ProductVM
            {
                Id = m.Id,
                Name = m.Name,
                CategoryName=m.ProductCategory.Name,
                Description=m.Description,
                Price=m.Price,
                ProductImages=m.ProductImages.Select(m=>new ProductImageVM { Name=m.Name,IsMain=m.IsMain}).ToList(),
                Discounts=m.DiscountProducts.Select(m=>new Discount { Id=m.DiscountId}).ToList(),
            }).Where(m=>m.Name.Contains(searchText)).ToListAsync();

            return View(products);
        }
    }
}
