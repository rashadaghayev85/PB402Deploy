using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Banner
{
    public class BannerCreateVM
    {
		[Required]
		public string Name { get; set; }
		[Required]
		public string Page { get; set; }
		[Required]
		public IFormFile UploadImage { get; set; }
	}
}
