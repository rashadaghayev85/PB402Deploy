using Organic_Food_MVC_Project.Models.Home;

namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class BasketDetailVM
    {
        public Dictionary<Product,int> Products { get; set; }
        public decimal Total { get; set; }
    }
}
