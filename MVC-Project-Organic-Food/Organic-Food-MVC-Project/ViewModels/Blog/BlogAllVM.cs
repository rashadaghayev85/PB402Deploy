namespace Organic_Food_MVC_Project.ViewModels.Blog
{
    public class BlogAllVM
    {
        public IEnumerable<BlogVM> Blogs { get; set; }
        public IEnumerable<BlogCategoryVM> BlogCategories { get; set; }
    }
}
