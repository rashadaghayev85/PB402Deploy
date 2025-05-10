using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Slider
{
    public class SliderEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Product { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public IFormFile? NewImage { get; set; }
        public string? OldImage { get; set; }
    }
}
