using Organic_Food_MVC_Project.ViewModels.Blog;

namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<CategoryVM> Categories { get; set; }
        public IEnumerable<ProductVM> Products { get; set; }
        public IEnumerable<SliderVM> Sliders { get; set; }
        public IEnumerable<ServiceVM> Services { get; set; }
        public AdvertisementVM Advertisement { get; set; }
        public AboutVM About { get; set; }
        public IEnumerable<BrandVM> Brands { get; set; }
        public IEnumerable<BlogVM> Blogs { get; set; }
    }
}
