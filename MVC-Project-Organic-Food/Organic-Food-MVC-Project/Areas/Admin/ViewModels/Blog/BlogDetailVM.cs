namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Blog
{
    public class BlogDetailVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; } 
        public string Author { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Like { get; set; }
        public int Comment { get; set; }
        public string CategoryName { get; set; }
    }
}
