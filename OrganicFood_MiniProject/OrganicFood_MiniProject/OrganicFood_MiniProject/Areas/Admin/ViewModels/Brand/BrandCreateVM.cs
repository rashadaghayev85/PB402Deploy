using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Brand
{
    public class BrandCreateVM
    {
        [Required]
        public IFormFile UploadImage { get; set; }
    }
}
