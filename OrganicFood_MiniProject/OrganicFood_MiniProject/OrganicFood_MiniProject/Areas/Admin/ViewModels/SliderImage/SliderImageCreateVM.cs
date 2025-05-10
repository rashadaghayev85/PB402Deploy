using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.SliderImage
{
    public class SliderImageCreateVM
    {
        [Required]
        public IFormFile UploadImage { get; set; }
    }
}
