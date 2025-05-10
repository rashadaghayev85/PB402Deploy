using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Brand
{
    public class BrandCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
