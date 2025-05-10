using Organic_Food_MVC_Project.Models.Home;

namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int ProductCategoryId { get; set; }
        public List<ProductImageVM> ProductImages { get; set; }
        public ICollection<Discount> Discounts { get; set; }
    }
}
