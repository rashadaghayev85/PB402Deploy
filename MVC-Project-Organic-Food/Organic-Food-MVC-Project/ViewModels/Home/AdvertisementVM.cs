using Organic_Food_MVC_Project.Services.Interfaces;

namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class AdvertisementVM
    {
        public string Product { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
    }
}
