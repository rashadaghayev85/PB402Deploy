using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Product
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public List<SelectListItem> Categories { get; set; }
        [Required]
        public IFormFile MainImage { get; set; }
        [Required]
        public IEnumerable<IFormFile> ProductImages { get; set; }
        [Required]
        public List<SelectListItem> Discounts { get; set; }

        public List<int> SelectedDiscountIds { get; set; }
    }
}
