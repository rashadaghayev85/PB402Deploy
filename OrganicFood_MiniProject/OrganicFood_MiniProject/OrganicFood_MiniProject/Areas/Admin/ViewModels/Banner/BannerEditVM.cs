namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Banner
{
    public class BannerEditVM
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public string Page { get; set; }
		public IFormFile UploadImage { get; set; }
		public string CurrentImagePath { get; set; }
	}
}
