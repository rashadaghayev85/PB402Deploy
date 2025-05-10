namespace OrganicFood_MiniProject.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
