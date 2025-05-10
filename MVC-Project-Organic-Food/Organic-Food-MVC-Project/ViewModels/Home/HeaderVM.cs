using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.ViewModels.Home
{
    public class HeaderVM
    {
       public Dictionary<string, string> Settings { get; set; }
        public int BasketProductCount { get; set; }
        public int BasketTotal { get; set; }
    }
}
