using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.ViewModels.Home;
using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Product
{
    public class ProductCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public IEnumerable<IFormFile> Images { get; set; }
    }
}
