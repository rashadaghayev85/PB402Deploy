using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Blog
{
	public class BlogCreateVM
	{
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; } 

        [Required]
        public int BlogCategoryId { get; set; }
        [Required]
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
