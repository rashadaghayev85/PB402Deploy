namespace Organic_Food_MVC_Project.Models.Blog
{
    public class BlogCategory:BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Blog> Blogs { get; set; }
    }
}
