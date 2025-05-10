using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.ProductCategory
{
    public class ProductCategoryEditVM
    {
        [Required]
        public string Name { get; set; }
    }
}
