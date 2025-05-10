using OrganicFood_MiniProject.Models;
using OrganicFood_MiniProject.ViewModels;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Product
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ProductImageVM> ProductImages { get; set; }
    }
}
