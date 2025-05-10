namespace Organic_Food_MVC_Project.Models.Home
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public ICollection<DiscountProduct> DiscountProducts { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}
