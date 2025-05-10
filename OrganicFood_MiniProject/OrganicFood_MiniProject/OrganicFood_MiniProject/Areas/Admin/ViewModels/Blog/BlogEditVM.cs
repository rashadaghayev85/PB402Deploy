using Microsoft.AspNetCore.Mvc.Rendering;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Blog
{
	public class BlogEditVM
	{
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; } 

        public IFormFile? ImageFile { get; set; }
        public int BlogCategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
    }
}
