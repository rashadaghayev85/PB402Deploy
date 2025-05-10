namespace Organic_Food_MVC_Project.Models.Blog
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }= DateTime.Now;
        public string Author { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Like { get; set; }
        public int Comment { get; set; }
        public int BlogCategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }
    }
}
