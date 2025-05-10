using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.ViewModels.Home;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Product
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<ProductImageVM> Images { get; set; }
        public IEnumerable<Discount> Discounts { get; set; }
    }
}
