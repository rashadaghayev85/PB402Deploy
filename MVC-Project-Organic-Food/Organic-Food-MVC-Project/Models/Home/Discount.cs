namespace Organic_Food_MVC_Project.Models.Home
{
    public class Discount:BaseEntity
    {
        public int Percent { get; set; }
        public List<DiscountProduct> DiscountProducts { get; set; }

    }
}
