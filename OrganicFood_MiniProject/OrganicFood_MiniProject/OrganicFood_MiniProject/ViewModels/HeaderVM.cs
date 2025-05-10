namespace OrganicFood_MiniProject.ViewModels
{
    public class HeaderVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<CategoryVM> Categories { get; set; }
        public List<string> CurrencyOptions { get; set; }
        public List<string> LanguageOptions { get; set; }
        public string UserFullName;
    }
}
