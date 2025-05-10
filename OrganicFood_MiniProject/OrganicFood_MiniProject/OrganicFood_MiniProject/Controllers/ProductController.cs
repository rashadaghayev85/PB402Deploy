using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;
using OrganicFood_MiniProject.ViewModels;

namespace OrganicFood_MiniProject.Controllers
{
    public class ProductController : Controller
    {
        public readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(m => m.Category)
                                                 .Include(m => m.ProductImages)
                                                 .Include(m => m.ProductDiscounts)
                                                 .ThenInclude(pd => pd.Discount)
                                                 .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var relatedProducts = await _context.Products
                                                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                                                .Include(p => p.ProductImages)
                                                .Include(p => p.ProductDiscounts)
                                                .ThenInclude(pd => pd.Discount)
                                                .ToListAsync() ?? new List<Product>();

            var productVM = new ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                DiscountedPrice = product.ProductDiscounts != null && product.ProductDiscounts.Any()
                    ? product.Price - (product.ProductDiscounts.Sum(pd => pd.Discount.DiscountPercentage) * product.Price / 100)
                    : product.Price,
                Description = product.Description,
                ProductImages = product.ProductImages?.Select(image => new ProductImageVM
                {
                    Name = image.Name,
                    IsMain = image.IsMain,
                }).ToList() ?? new List<ProductImageVM>(),
                CategoryId = product.CategoryId,
                Category = product.Category,
                Banners = _context.Banners
                    .Where(bn => bn.Page == "Product")
                    .Select(bn => new BannerVM { Name = bn.Name, Image = bn.Image, Page = bn.Page })
                    .ToList() ?? new List<BannerVM>(),
                RelatedProducts = relatedProducts.Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DiscountedPrice = p.ProductDiscounts != null && p.ProductDiscounts.Any()
                        ? p.Price - (p.ProductDiscounts.Sum(pd => pd.Discount.DiscountPercentage) * p.Price / 100)
                        : p.Price,
                    Description = p.Description,
                    ProductImages = p.ProductImages?.Select(img => new ProductImageVM
                    {
                        Name = img.Name,
                        IsMain = img.IsMain
                    }).ToList() ?? new List<ProductImageVM>(),
                    Category = p.Category
                }).ToList() ?? new List<ProductVM>()
            };

            return View(productVM);
        }

    }
}
