using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IProductService _productService;
        private readonly AppDbContext _context;
        public CartController(IHttpContextAccessor accessor,
                              IProductService productService,
                              AppDbContext context)
        {
            _accessor=accessor;
            _productService=productService;
            _context=context;
        }
        public async Task<IActionResult> Index()
        {
            List<BasketVM> basketDatas = new();
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            Dictionary<Product,int> products = new();
            foreach (var item in basketDatas)
            {
                var product =await _context.Products.Include(m=>m.ProductImages).Include(m=>m.DiscountProducts).FirstOrDefaultAsync(m=>m.Id==item.ProductId);
                products.Add(product,item.ProductCount);
            }
            decimal total = products.Sum(m=>m.Key.Price*m.Value);
            return View(new BasketDetailVM { Products=products,Total=total});
        }

        public async Task<IActionResult> Delete(int id)
        {
            List<BasketVM> basketDatas = new();
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            var existProduct = basketDatas.FirstOrDefault(m=>m.ProductId==id);
            basketDatas.Remove(existProduct);

            _accessor.HttpContext.Response.Cookies.Append("basket",JsonConvert.SerializeObject(basketDatas));

            var basketCount = basketDatas.Sum(m => m.ProductCount);
            Dictionary<Product, int> products = new();
            foreach (var item in basketDatas)
            {
                var product = await _context.Products.Include(m => m.ProductImages).Include(m => m.DiscountProducts).FirstOrDefaultAsync(m => m.Id == item.ProductId);
                products.Add(product, item.ProductCount);
            }
            decimal total = products.Sum(m => m.Key.Price * m.Value);

            return Ok(new { basketCount, total });
        }
        public IActionResult DeleteAll()
        {
            List<BasketVM> basketDatas = new();
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }
            if (basketDatas.Any())
            {
                basketDatas.Clear();
            }

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));

            return Ok(new { basketCount = 0, total = 0 });
        }
    }
}
