using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Slider
{
	public class SliderCreateVM
	{
		[Required]
		public string FirstTitle { get; set; }
		[Required]
		public string SecondTitle { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public IFormFile UploadImage { get; set; }
	}
}
