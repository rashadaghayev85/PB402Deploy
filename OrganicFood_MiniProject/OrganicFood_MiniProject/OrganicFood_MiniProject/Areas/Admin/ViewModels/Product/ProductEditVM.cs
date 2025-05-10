using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Product
{
    public class ProductEditVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IFormFile? MainImage { get; set; }
        public string ExistingMainImage { get; set; }
        public IEnumerable<IFormFile>? ProductImages { get; set; }
        public List<string> ExistingImage { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<int> DiscountIds { get; set; } = new List<int>();
        public List<SelectListItem> Discounts { get; set; }

    }
}
