using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.FreshFruit
{
    public class FreshFruitCreateVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public IFormFile UploadImage { get; set; }
    }
}
