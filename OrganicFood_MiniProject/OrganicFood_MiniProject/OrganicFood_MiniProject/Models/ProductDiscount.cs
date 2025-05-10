using System.Text.RegularExpressions;

namespace OrganicFood_MiniProject.Models
{
    public class ProductDiscount : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
