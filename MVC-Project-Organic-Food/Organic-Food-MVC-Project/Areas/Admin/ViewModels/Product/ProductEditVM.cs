using Organic_Food_MVC_Project.ViewModels.Home;
using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Product
{
    public class ProductEditVM
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public IEnumerable<IFormFile> UploadImages { get; set; }
        public IEnumerable<ProductImageVM> ProductImages { get; set; }
        public int? NewDiscount { get; set; }
        public int? ExistDiscounts { get; set; }
    }
}
