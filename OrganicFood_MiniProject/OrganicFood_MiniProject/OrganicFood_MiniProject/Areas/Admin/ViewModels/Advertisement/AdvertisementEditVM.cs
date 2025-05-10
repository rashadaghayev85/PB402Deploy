namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Advertisement
{
    public class AdvertisementEditVM
    {
        public int Id { get; set; }

        public string ExistingBackgroundImage { get; set; }
        public IFormFile BackgroundImageUpload { get; set; }

        public string ExistingFirstImage { get; set; }
        public IFormFile FirstImageUpload { get; set; }

        public string ExistingSecondImage { get; set; }
        public IFormFile SecondImageUpload { get; set; }

        public string ExistingThirdImage { get; set; }
        public IFormFile ThirdImageUpload { get; set; }

        public string ExistingFourthImage { get; set; }
        public IFormFile FourthImageUpload { get; set; }
    }
}
