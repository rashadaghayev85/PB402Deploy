using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Promotion
{
	public class PromotionCreateVM
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public IFormFile UploadImage { get; set; }
		[Required]
		public string Description { get; set; }
	}
}
