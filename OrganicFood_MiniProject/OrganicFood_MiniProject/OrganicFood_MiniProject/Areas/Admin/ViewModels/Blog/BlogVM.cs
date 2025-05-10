namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Blog
{
	public class BlogVM
	{
		public int Id { get; set; }
		public string Image { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
	}
}
