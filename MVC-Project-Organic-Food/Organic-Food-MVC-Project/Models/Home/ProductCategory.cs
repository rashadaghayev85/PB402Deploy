namespace Organic_Food_MVC_Project.Models.Home
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; set; }
        public string? Logo { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
