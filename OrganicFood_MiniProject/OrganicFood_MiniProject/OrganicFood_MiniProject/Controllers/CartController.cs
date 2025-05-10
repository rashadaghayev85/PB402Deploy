using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.ViewModels;

namespace OrganicFood_MiniProject.Controllers
{
    public class CartController : Controller
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly AppDbContext _context;

        public CartController(IHttpContextAccessor accessor, AppDbContext context)
        {
            _accessor = accessor;
            _context = context;
        }

        public IActionResult Index()
        {
            List<BasketVM> basketDatas = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            Dictionary<ProductVM, int> products = new();

            foreach (var item in basketDatas)
            {
                var product = _context.Products
                                      .Where(p => p.Id == item.ProductId)
                                      .Select(p => new ProductVM { Id = p.Id, Name = p.Name, Price = p.Price })
                                      .FirstOrDefault();
                if (product != null)
                {
                    products.Add(product, item.ProductCount);
                }
            }

            decimal total = products.Sum(m => m.Key.Price * m.Value);
            return View(new BasketDetailVM { Products = products, Total = total });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            List<BasketVM> basketDatas = new();

            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            var existBasketData = basketDatas.FirstOrDefault(m => m.ProductId == id);
            if (existBasketData != null)
            {
                basketDatas.Remove(existBasketData);
            }

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas));
            Dictionary<ProductVM, int> products = new();

            foreach(var item in basketDatas)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    products.Add(new ProductVM { Id = product.Id, Name = product.Name, Price = product.Price }, item.ProductCount);
                }

            }

            decimal total = products.Sum(m=>m.Key.Price * m.Value);
            int basketCount = basketDatas.Sum(m => m.ProductCount);

            return Ok(new {total, basketCount});
        }

        


        [HttpPost]
        public IActionResult ClearCart()
        {
            _accessor.HttpContext.Response.Cookies.Delete("basket");
            return Ok();
        }
    }

}


