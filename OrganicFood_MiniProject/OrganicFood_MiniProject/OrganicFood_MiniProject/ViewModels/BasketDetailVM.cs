namespace OrganicFood_MiniProject.ViewModels
{
    public class BasketDetailVM
    {
        public Dictionary<ProductVM, int> Products { get; set; }
        public decimal Total { get; set; }
    }
}
