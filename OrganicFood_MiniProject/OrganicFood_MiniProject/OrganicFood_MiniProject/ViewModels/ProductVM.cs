using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<ProductImageVM> ProductImages { get; set; }
        public IEnumerable<BannerVM> Banners { get; set; }
        public List<ProductVM> RelatedProducts { get; set; }
    }
}
