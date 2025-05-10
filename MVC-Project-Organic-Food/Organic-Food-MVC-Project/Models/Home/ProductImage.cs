namespace Organic_Food_MVC_Project.Models.Home
{
    public class ProductImage:BaseEntity
    {
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
