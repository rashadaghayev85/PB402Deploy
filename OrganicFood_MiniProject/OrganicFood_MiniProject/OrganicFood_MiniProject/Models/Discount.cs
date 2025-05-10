namespace OrganicFood_MiniProject.Models
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
    }
}
