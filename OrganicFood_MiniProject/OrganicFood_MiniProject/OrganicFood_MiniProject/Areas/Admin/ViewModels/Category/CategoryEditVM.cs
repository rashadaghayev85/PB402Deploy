namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Category
{
    public class CategoryEditVM
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public IFormFile UploadImage { get; set; }
		public string CurrentImagePath { get; set; }
	}
}
