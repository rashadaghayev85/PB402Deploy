using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;
using OrganicFood_MiniProject.ViewModels;
using System.Diagnostics;

namespace OrganicFood_MiniProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _accessor;

        public HomeController(AppDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            SliderImage image = await _context.SliderImages.FirstOrDefaultAsync();
            var sliderVM = sliders.Select(slider => new SliderVM
            {
                FirstTitle = slider.FirstTitle,
                SecondTitle = slider.SecondTitle,
                Description = slider.Description,
                Image = slider.Image,
                SliderImage = image?.BackgroundImage
            }).ToList();


            IEnumerable<FreshFruit> freshFruits = await _context.FreshFruits.ToListAsync();
            var freshFruitVM = freshFruits.Select(freshFruit => new FreshFruitVM
            {
                Title = freshFruit.Title,
                Img = freshFruit.Img,
            });


            IEnumerable<Product> products = await _context.Products
                                                          .Include(m => m.Category)
                                                          .Include(m => m.ProductImages)
                                                          .Include(m => m.ProductDiscounts)
                                                          .ThenInclude(pd => pd.Discount)
                                                          .ToListAsync();
            var productVM = products.Select(product => new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ProductImages = product.ProductImages.Select(image => new ProductImageVM
                {
                    Name = image.Name,
                    IsMain = image.IsMain,
                }).ToList(),
                CategoryId = product.CategoryId,
                Category = product.Category,
                DiscountedPrice = product.ProductDiscounts.Any()
                                  ? product.Price - (product.ProductDiscounts.Sum(pd => pd.Discount.DiscountPercentage) * product.Price / 100)
                                  : product.Price
            });


            IEnumerable<Category> categories = await _context.Categories
                                                             .Include(c => c.Products)
                                                             .ThenInclude(p => p.ProductImages)
                                                             .ToListAsync();

            var categoryVM = categories.Select(category => new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Image = category.Image
            }).ToList();


            Advertisement advertisement = await _context.Advertisements.FirstOrDefaultAsync();
            var advertisementVM = new AdvertisementVM
            {
                BackgroundImage = advertisement.BackgroundImage,
                FirstImage = advertisement.FirstImage,
                SecondImage = advertisement.SecondImage,
                ThirdImage = advertisement.ThirdImage,
                FourthImage = advertisement.FourthImage
            };


            Promotion promotion = await _context.Promotions.FirstOrDefaultAsync();
            var promotionVM = new PromotionVM
            {
                Image = promotion.Image,
                Title = promotion.Title,
                Description = promotion.Description,
            };


            IEnumerable<Discount> discounts = await _context.Discounts.ToListAsync();
            var discountVM = discounts.Select(discount => new DiscountVM
            {
                Name = discount.Name,
                DiscountPercentage = discount.DiscountPercentage
            });


            IEnumerable<Brand> brands = await _context.Brands.ToListAsync();
            var brandVM = brands.Select(brand => new BrandVM
            {
                Logo = brand.Logo
            });


            IEnumerable<Blog> blogs = await _context.Blogs.ToListAsync();
            var blogVM = blogs.Select(blog => new BlogVM
            {
                Title = blog.Title,
                Description= blog.Description,
                Image = blog.Image,
                CreatedDate = blog.CreatedDate
            });


            var homeVM = new HomeVM
            {
                Sliders = sliderVM,
                SliderImage = image,
                FreshFruits = freshFruitVM,
                Products = productVM,
                Categories = categoryVM,
                Advertisement = advertisementVM,
                Promotion = promotionVM,
                Discounts = discountVM,
                Brands = brandVM,
                Blogs = blogVM,
            };

            return View(homeVM);
        }


        public async Task<IActionResult> Search(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return View(new List<ProductVM>());
            }

            var products = await _context.Products
                .Where(p => p.Name.Contains(s))
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount)
                .ToListAsync();

            var productVMs = products.Select(p => new ProductVM
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DiscountedPrice = p.ProductDiscounts.Any()
                    ? p.Price - (p.ProductDiscounts.Sum(pd => pd.Discount.DiscountPercentage) * p.Price / 100)
                    : p.Price,
                Description = p.Description,
                ProductImages = p.ProductImages.Select(img => new ProductImageVM
                {
                    Name = img.Name,
                    IsMain = img.IsMain
                }).ToList(),
                Category = p.Category
            }).ToList();

            return View(productVMs);
        }


        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            List<BasketVM> basketDatas = [];

            if (_accessor.HttpContext.Request.Cookies["basket"] != null)
            {
                basketDatas = JsonConvert.DeserializeObject<List<BasketVM>>(_accessor.HttpContext.Request.Cookies["basket"]);
            }

            var existProduct = basketDatas.FirstOrDefault(m => m.ProductId == id);
            if (existProduct != null)
            {
                existProduct.ProductCount++;
            }
            else
            {
                basketDatas.Add(new BasketVM { ProductId = id, ProductCount = 1 });
            }

            _accessor.HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basketDatas), new CookieOptions { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) });

            return Ok();
        }
    }
}
           

