using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.BlogCategory
{
    public class BlogCategoryEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
