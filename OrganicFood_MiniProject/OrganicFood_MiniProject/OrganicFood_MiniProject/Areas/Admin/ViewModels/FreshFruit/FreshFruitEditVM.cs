namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.FreshFruit
{
    public class FreshFruitEditVM
    {
		public int Id { get; set; }
		public string Title { get; set; }
		public IFormFile UploadImage { get; set; }
		public string CurrentImagePath { get; set; }
	}
}
