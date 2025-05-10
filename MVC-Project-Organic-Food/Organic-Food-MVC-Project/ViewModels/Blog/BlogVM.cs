using Organic_Food_MVC_Project.Models.Blog;

namespace Organic_Food_MVC_Project.ViewModels.Blog
{
    public class BlogVM
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Like { get; set; }
        public int Comment { get; set; }
    }
}
