namespace OrganicFood_MiniProject.Models
{
    public class Blog : BaseEntity
    {
        public string Image {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
		public int BlogCategoryId { get; set; }
		public BlogCategory Category { get; set; }
	}
}
