namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Brand
{
    public class BrandEditVM
    {
        public int Id { get; set; }
        public string ExistingImage { get; set; }
        public IFormFile? UploadImage { get; set; }
    }
}
