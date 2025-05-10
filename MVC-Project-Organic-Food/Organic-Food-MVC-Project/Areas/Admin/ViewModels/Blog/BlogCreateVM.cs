using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Blog
{
    public class BlogCreateVM
    {
        [Required]
        public string Title { get; set; }
        public DateTime Date { get; set; }=DateTime.Now;
        [Required]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
