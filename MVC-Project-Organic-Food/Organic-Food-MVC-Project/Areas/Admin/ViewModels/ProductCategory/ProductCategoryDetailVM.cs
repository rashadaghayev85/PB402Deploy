namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.ProductCategory
{
    public class ProductCategoryDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
        public bool HasProduct { get; set; }
    }
}
