using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Slider
{
    public class SliderCreateVM
    {
        [Required]
        public string Product { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
