
namespace Organic_Food_MVC_Project.Models.Home
{
    public class DiscountProduct:BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product{ get; set; }
        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}
