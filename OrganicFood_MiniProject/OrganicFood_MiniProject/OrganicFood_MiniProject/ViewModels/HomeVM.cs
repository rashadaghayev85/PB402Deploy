using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.ViewModels
{
    public class HomeVM
    {
        public List<SliderVM> Sliders { get; set; }
        public SliderImage SliderImage { get; set; }
        public IEnumerable<FreshFruitVM> FreshFruits { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public IEnumerable<CategoryVM> Categories { get; set; }
        public AdvertisementVM Advertisement { get; set; }
        public PromotionVM Promotion { get; set; }
        public IEnumerable<DiscountVM> Discounts { get; set; }
        public IEnumerable<BrandVM> Brands { get; set; }
        public IEnumerable<BlogVM> Blogs { get; set; }
    }
}
