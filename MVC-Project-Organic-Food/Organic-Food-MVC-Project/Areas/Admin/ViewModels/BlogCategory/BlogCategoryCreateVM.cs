using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.BlogCategory
{
    public class BlogCategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
