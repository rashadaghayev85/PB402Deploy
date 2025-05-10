using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IHttpContextAccessor _accessor;
        private readonly AppDbContext _context;
        public HeaderViewComponent(ISettingService settingService, 
                                   IHttpContextAccessor accessor,
                                   AppDbContext context)
        {
            _settingService = settingService;
            _accessor = accessor;
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<BasketVM> basketDatas = new();
            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            int basketCount = basketDatas.Sum(m=>m.ProductCount);

            Dictionary<Product, int> products = new();
            foreach (var item in basketDatas)
            {
                var product = await _context.Products.Include(m => m.ProductImages).Include(m => m.DiscountProducts).FirstOrDefaultAsync(m => m.Id == item.ProductId);
                products.Add(product, item.ProductCount);
            }
            decimal total = products.Sum(m => m.Key.Price * m.Value);

            var datas = await _settingService.GetAllAsync();

            return View(new HeaderVM {Settings=datas,BasketProductCount=basketCount,BasketTotal=(int)total});
        }
    }
}
