using OrganicFood_MiniProject.Models;

namespace OrganicFood_MiniProject.ViewModels
{
    public class BlogVM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public IEnumerable<BannerVM> Banners { get; set; }
        public int CategoryId { get; set; }
        public BlogCategory BlogCategory { get; set; }
    }
}
