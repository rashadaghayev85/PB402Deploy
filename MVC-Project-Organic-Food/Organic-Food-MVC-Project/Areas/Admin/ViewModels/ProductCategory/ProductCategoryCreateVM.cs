using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.ProductCategory
{
    public class ProductCategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
