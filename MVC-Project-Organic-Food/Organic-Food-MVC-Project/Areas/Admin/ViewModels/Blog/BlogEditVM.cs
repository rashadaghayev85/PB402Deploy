using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Blog
{
    public class BlogEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile? NewImage { get; set; } 
        public string? OldImage { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
