namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Promotion
{
	public class PromotionEditVM
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public IFormFile UploadImage { get; set; }
		public string CurrentImagePath { get; set; }
	}
}
