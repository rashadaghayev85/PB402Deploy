using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.BlogCategory;
using OrganicFood_MiniProject.Areas.Admin.ViewModels.Discount;
using OrganicFood_MiniProject.Data;
using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {
        public readonly AppDbContext _context;
        public DiscountController(AppDbContext context)
        {
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            IEnumerable<DiscountVM> discounts = await _context.Discounts.Select(discount => new DiscountVM { Id = discount.Id, Name = discount.Name, DiscountPercent = discount.DiscountPercentage }).ToListAsync();
            return View(discounts);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountCreateVM request)
        {
            if (!ModelState.IsValid) return View(request);

            var discount = new Discount
            {
                Name = request.Name,
                DiscountPercentage = request.DiscountPercent,
            };

            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return BadRequest();
            Discount discount = await _context.Discounts.FirstOrDefaultAsync(m => m.Id == id);
            if (discount == null) return NotFound();

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }

            var discountVM = new DiscountEditVM
            {
                Id = discount.Id,
                Name = discount.Name,
                DiscountPercent = discount.DiscountPercentage
            };

            return View(discountVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, DiscountEditVM request)
        {
            if (id != request.Id) return BadRequest();

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null) return NotFound();

            discount.Name = request.Name;
            discount.DiscountPercentage = request.DiscountPercent;

            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
