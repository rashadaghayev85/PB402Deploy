using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.BlogCategory
{
    public class BlogCategoryCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
