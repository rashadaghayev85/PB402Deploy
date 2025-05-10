namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Slider
{
	public class SliderEditVM
	{
		public int Id { get; set; }
		public string FirstTitle { get; set; }
		public string SecondTitle { get; set; }

		public string Description { get; set; }

		public IFormFile UploadImage { get; set; }

		public string CurrentImagePath { get; set; }
	}
}
