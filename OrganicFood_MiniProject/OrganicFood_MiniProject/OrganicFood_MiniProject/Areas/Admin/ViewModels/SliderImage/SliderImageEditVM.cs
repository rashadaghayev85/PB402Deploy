namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.SliderImage
{
    public class SliderImageEditVM
    {
        public int Id { get; set; }
        public string ExistingImage { get; set; }
        public IFormFile? UploadImage { get; set; }
    }
}
