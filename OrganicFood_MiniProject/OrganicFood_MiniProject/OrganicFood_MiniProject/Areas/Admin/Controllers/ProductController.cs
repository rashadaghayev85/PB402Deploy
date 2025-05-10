using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Product;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;
using OrganicFood_MiniProject.ViewModels;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<OrganicFood_MiniProject.Areas.Admin.ViewModels.Product.ProductVM> products = await _context.Products.Include(m => m.Category).Include(m => m.ProductImages).Include(m => m.ProductDiscounts).ThenInclude(pd => pd.Discount).Select(product => new OrganicFood_MiniProject.Areas.Admin.ViewModels.Product.ProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = product.Category.Name,
                ProductImages = product.ProductImages.Select(image => new ProductImageVM
                {
                    Name = image.Name,
                    IsMain = image.IsMain,
                }).ToList(),

            }).ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductCreateVM
            {
                Categories = await _context.Categories
             .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
             .ToListAsync(),
                Discounts = await _context.Discounts
             .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
             .ToListAsync(),
                SelectedDiscountIds = new List<int>()
            };

            return View(viewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM request)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Invalid category");
                return View(request);
            }

            var validDiscounts = await _context.Discounts
                .Where(d => request.SelectedDiscountIds.Contains(d.Id))
                .ToListAsync();

            if (validDiscounts.Count != request.SelectedDiscountIds.Count)
            {
                ModelState.AddModelError("SelectedDiscountIds", "Some discounts are invalid.");
                return View(request);
            }

            string mainImageFileName = Guid.NewGuid().ToString() + "-" + request.MainImage.FileName;
            string mainImageFilePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", mainImageFileName);

            using (FileStream stream = new FileStream(mainImageFilePath, FileMode.Create))
            {
                await request.MainImage.CopyToAsync(stream);
            }

            Product newProduct = new Product
            {
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
                CategoryId = request.CategoryId,
                ProductImages = new List<ProductImage>(),
                ProductDiscounts = validDiscounts.Select(d => new ProductDiscount { DiscountId = d.Id }).ToList()
            };

            newProduct.ProductImages.Add(new ProductImage
            {
                Name = mainImageFileName,
                IsMain = true
            });

            if (request.ProductImages != null)
            {
                foreach (var item in request.ProductImages)
                {
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                    string filePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", fileName);

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    newProduct.ProductImages.Add(new ProductImage
                    {
                        Name = fileName,
                        IsMain = false
                    });
                }
            }

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) return NotFound();

            var categories = await _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToListAsync();

            var discounts = await _context.Discounts
                .Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name })
                .ToListAsync();

            return View(new ProductEditVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                Categories = categories,
                Discounts = discounts,
                ExistingMainImage = product.ProductImages.FirstOrDefault(i => i.IsMain)?.Name,
                ExistingImage = product.ProductImages.Where(i => !i.IsMain).Select(i => i.Name).ToList(),
                DiscountIds = product.ProductDiscounts.Select(pd => pd.DiscountId).ToList() 
            });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditVM request)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDiscounts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists)
            {
                ModelState.AddModelError("CategoryId", "Invalid category");
                return View(request);
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

           
            var existingDiscounts = product.ProductDiscounts.Select(pd => pd.DiscountId).ToList();

            
            var discountsToRemove = existingDiscounts.Except(request.DiscountIds).ToList();
            foreach (var discountId in discountsToRemove)
            {
                var discountToRemove = product.ProductDiscounts.FirstOrDefault(pd => pd.DiscountId == discountId);
                if (discountToRemove != null)
                {
                    _context.ProductDiscounts.Remove(discountToRemove);
                }
            }

            
            var discountsToAdd = request.DiscountIds.Except(existingDiscounts).ToList();
            foreach (var discountId in discountsToAdd)
            {
                product.ProductDiscounts.Add(new ProductDiscount { ProductId = product.Id, DiscountId = discountId });
            }

           
            request.ExistingImage ??= new List<string>();
            var imagesToDelete = product.ProductImages
                .Where(i => !i.IsMain && i.Name != null && !request.ExistingImage.Contains(i.Name))
                .ToList();
            foreach (var image in imagesToDelete)
            {
                string imagePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", image.Name);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                _context.ProductImages.Remove(image);
            }

            if (request.MainImage != null)
            {
                var mainImage = product.ProductImages.FirstOrDefault(i => i.IsMain);
                if (mainImage != null)
                {
                    string oldMainImagePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", mainImage.Name);
                    if (System.IO.File.Exists(oldMainImagePath))
                    {
                        System.IO.File.Delete(oldMainImagePath);
                    }
                    _context.ProductImages.Remove(mainImage);
                }

                string newMainImageFileName = Guid.NewGuid().ToString() + "-" + request.MainImage.FileName;
                string newMainImageFilePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", newMainImageFileName);

                using (FileStream stream = new FileStream(newMainImageFilePath, FileMode.Create))
                {
                    await request.MainImage.CopyToAsync(stream);
                }

                product.ProductImages.Add(new ProductImage { Name = newMainImageFileName, IsMain = true });
            }

            if (request.ProductImages != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                foreach (var item in request.ProductImages)
                {
                    var fileExtension = Path.GetExtension(item.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("ProductImages", "Only JPG, JPEG, PNG, and WEBP formats are allowed.");
                        return View(request);
                    }

                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                    string filePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", fileName);

                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    product.ProductImages.Add(new ProductImage { Name = fileName, IsMain = false });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDiscounts)
                .ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            decimal discountedPrice = product.Price;
            List<string> appliedDiscounts = new List<string>();

            
            foreach (var productDiscount in product.ProductDiscounts)
            {
                var discount = productDiscount.Discount;
                decimal discountAmount = discountedPrice * discount.DiscountPercentage / 100;
                discountedPrice -= discountAmount;

                
                appliedDiscounts.Add($"{discount.Name} ({discount.DiscountPercentage}%)");
            }

            var productDetailsVM = new ProductDetailVM
            {
                Name = product.Name,
                Description = product.Description,
                CategoryName = product.Category.Name,
                Price = product.Price,
                DiscountedPrice = discountedPrice,
                MainImage = product.ProductImages.FirstOrDefault(i => i.IsMain)?.Name,
                OtherImages = product.ProductImages.Where(i => !i.IsMain).Select(i => i.Name).ToList(),
                AppliedDiscounts = appliedDiscounts
            };

            return View(productDetailsVM);
        }



        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            ProductImage mainImage = product.ProductImages.FirstOrDefault(m => m.IsMain);
            string mainImageFilePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", mainImage.Name);

            if (System.IO.File.Exists(mainImageFilePath))
            {
                System.IO.File.Delete(mainImageFilePath);
            }

            IEnumerable<ProductImage> additionalImages = product.ProductImages.Where(m => !m.IsMain);

            foreach (var image in additionalImages)
            {
                string filePath = Path.Combine(_environment.WebRootPath, "assets/images/products/", image.Name);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
