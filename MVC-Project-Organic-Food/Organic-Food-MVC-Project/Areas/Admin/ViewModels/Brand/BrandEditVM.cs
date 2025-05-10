using System.ComponentModel.DataAnnotations;

namespace Organic_Food_MVC_Project.Areas.Admin.ViewModels.Brand
{
    public class BrandEditVM
    {
        public int Id { get; set; }
        [Required]
        public string NewName { get; set; }
        public IFormFile NewImage { get; set; }
        public string OldImage { get; set; }
    }
}
