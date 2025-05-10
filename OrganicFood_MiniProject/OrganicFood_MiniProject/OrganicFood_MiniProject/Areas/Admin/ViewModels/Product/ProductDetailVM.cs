namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Product
{
    public class ProductDetailVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string MainImage { get; set; }
        public List<string> OtherImages { get; set; }
        public List<string> AppliedDiscounts { get; set; } = new List<string>();
    }
}
