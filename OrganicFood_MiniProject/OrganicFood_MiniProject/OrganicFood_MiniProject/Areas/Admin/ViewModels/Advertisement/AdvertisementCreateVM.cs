using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Advertisement
{
    public class AdvertisementCreateVM
    {
        [Required]
        public IFormFile BackgroundImageUpload { get; set; }

        [Required]
        public IFormFile FirstImageUpload { get; set; }

        [Required]
        public IFormFile SecondImageUpload { get; set; }

        [Required]
        public IFormFile ThirdImageUpload { get; set; }

        [Required]
        public IFormFile FourthImageUpload { get; set; }
    }
}
