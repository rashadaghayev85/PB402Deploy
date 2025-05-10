using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Areas.Admin.ViewModels.Product;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.Services.Interfaces;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context,
                                 IProductService productService,
                                 IWebHostEnvironment env)
        {
            _context = context;
            _productService = productService;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            List<ProductDetailVM> result = new List<ProductDetailVM>();
            foreach (var product in products)
            {
                result.Add(new ProductDetailVM
                {
                    CategoryName = product.CategoryName,
                    Description = product.Description,
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Images = product.ProductImages.ToList(),
                });
            }
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            var existProduct = await _productService.GetByIdAsync(id);
           
            return View(new ProductDetailVM
            {
                Discounts=existProduct.Discounts.ToList(),
                CategoryName= existProduct.CategoryName,
                Description= existProduct.Description,
                Id= existProduct.Id,
                Images= existProduct.ProductImages.ToList(),
                Name= existProduct.Name,
                Price= existProduct.Price,
            });
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Name,
            }).ToListAsync();

            ViewBag.Categories = categories;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Name,
            }).ToListAsync();

            ViewBag.Categories = categories;

            if (!ModelState.IsValid) return View(request);

            List<ProductImage> productImages = new List<ProductImage>();

            foreach (var image in request.Images)
            {
                if (!image.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be image!");
                    return View(request);
                }
                if (image.Length / 1024 > 1024)
                {
                    ModelState.AddModelError("Images", "Image size must be smaller than 1mb!");
                    return View(request);
                }
            }

            foreach (var image in request.Images)
            {

                string fileName= Guid.NewGuid().ToString() + "-" + image.FileName;
                string filePath = Path.Combine(_env.WebRootPath,"assets","images","products",fileName);
                using(FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                productImages.Add(new ProductImage
                {
                    Name = fileName,
                    IsMain=false,
                });
            }
            productImages.FirstOrDefault().IsMain = true;

            Product newProduct= new Product(){
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ProductCategoryId = _context.ProductCategories.FirstOrDefault(m => m.Name == request.CategoryName).Id,
                ProductImages = productImages
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var existProduct = await _context.Products.Include(m=>m.ProductImages).FirstOrDefaultAsync(m=>m.Id==id);
            if (existProduct == null) return NotFound();

            foreach (var item in existProduct.ProductImages)
            {
                string filePath = Path.Combine(_env.WebRootPath, "assets", "images", "products", item.Name);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Products.Remove(existProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return BadRequest();
            var existProduct = await _context.Products.Include(m => m.ProductImages)
                                                      .Include(m=>m.ProductCategory)
                                                      .Include(m=>m.DiscountProducts)
                                                      .FirstOrDefaultAsync(m => m.Id == id);
            if (existProduct == null) return NotFound();

            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Id.ToString(),
            }).ToListAsync();

            ViewBag.Categories = categories;

            var discounts = await _context.Discounts.Select(m => new SelectListItem
            {
                Text = m.Percent.ToString(),
                Value = m.Id.ToString(),
            }).ToListAsync();

            ViewBag.Discounts = discounts;

            var existDiscounts = await _context.DiscountsProducts.Include(m=>m.Discount).Where(m => m.ProductId == id).Select(m=>new SelectListItem
            {
                Text=m.Discount.Percent.ToString(),
                Value = m.Discount.Id.ToString(),
            }).ToListAsync();

            ViewBag.ExistDiscounts = existDiscounts;
            return View(new ProductEditVM
            {
                Id=existProduct.Id,
                Name= existProduct.Name,
                Description= existProduct.Description,
                Price= existProduct.Price,
                CategoryId=existProduct.ProductCategoryId,
                ProductImages=existProduct.ProductImages.Select(m=>new ProductImageVM
                {
                    Id=m.Id,
                    IsMain=m.IsMain,
                    Name=m.Name,
                }).ToList(),
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,ProductEditVM request)
        {
            if (id == null) return BadRequest();
            var existProduct = await _context.Products.Include(m => m.ProductImages)
                                                      .Include(m => m.ProductCategory)
                                                      .Include(m => m.DiscountProducts)
                                                      .FirstOrDefaultAsync(m => m.Id == id);
            if (existProduct == null) return NotFound();

            var categories = await _context.ProductCategories.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Id.ToString(),
            }).ToListAsync();

            ViewBag.Categories = categories;

            var discounts = await _context.Discounts.Select(m => new SelectListItem
            {
                Text = m.Percent.ToString(),
                Value = m.Id.ToString(),
            }).ToListAsync();

            ViewBag.Discounts = discounts;

            var existDiscounts = await _context.DiscountsProducts.Include(m => m.Discount).Where(m => m.ProductId == id).Select(m => new SelectListItem
            {
                Text = m.Discount.Percent.ToString(),
                Value = m.Discount.Id.ToString(),
            }).ToListAsync();

            ViewBag.ExistDiscounts = existDiscounts;

            if (ModelState.IsValid) return View(request);

            if (request.UploadImages != null)
            {
                foreach (var image in request.UploadImages)
                {
                    if (!image.ContentType.Contains("image/"))
                    {
                        ModelState.AddModelError("Images", "File type must be image!");
                        return View(request);
                    }
                    if (image.Length / 1024 > 1024)
                    {
                        ModelState.AddModelError("Images", "Image size must be smaller than 1mb!");
                        return View(request);
                    }
                }
                List<ProductImage> uploadImages = new List<ProductImage>();
                foreach (var image in request.UploadImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + image.FileName;
                    string filePath = Path.Combine(_env.WebRootPath,"assets", "images", "products", fileName);
                    using(FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                       await image.CopyToAsync(stream);
                    }
                    uploadImages.Add(new ProductImage { IsMain=false,Name=fileName,ProductId=request.Id});
                }
                await _context.ProductImages.AddRangeAsync(uploadImages);
            }

            if (request.ExistDiscounts!=null)
            {
                var existDiscount = await _context.DiscountsProducts.FirstOrDefaultAsync(m => m.DiscountId == request.ExistDiscounts);

                _context.DiscountsProducts.Remove(existDiscount);
            }
            if (request.NewDiscount != null)
            {
                await _context.DiscountsProducts.AddAsync(new DiscountProduct { ProductId=request.Id,DiscountId=(int)request.NewDiscount});
            }
            existProduct.Name=request.Name;
            existProduct.Description=request.Description;
            existProduct.Price=request.Price;  
            existProduct.ProductCategoryId= request.CategoryId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public async Task<IActionResult> DeleteImageFromProduct(int? id)
        {
            if (id == null) return BadRequest();
            var existProductImage = await _context.ProductImages.FirstOrDefaultAsync(m=>m.Id==id);
            if (existProductImage == null) return NotFound();

            string filePath = Path.Combine(_env.WebRootPath,"assets", "images", "products", existProductImage.Name);
            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.ProductImages.Remove(existProductImage);
            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpPost]
        public async Task<IActionResult> SetImageMain(int? id)
        {
            if (id == null) return BadRequest();
            var existProductImage = await _context.ProductImages.FirstOrDefaultAsync(m => m.Id == id);
            if (existProductImage == null) return NotFound();

            var product = await _context.Products.Include(m=>m.ProductImages).FirstOrDefaultAsync(m=>m.ProductImages.Contains(existProductImage));
            if (product == null) return NotFound();
            product.ProductImages.FirstOrDefault(m => m.IsMain == true).IsMain = false;

            existProductImage.IsMain = true;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
