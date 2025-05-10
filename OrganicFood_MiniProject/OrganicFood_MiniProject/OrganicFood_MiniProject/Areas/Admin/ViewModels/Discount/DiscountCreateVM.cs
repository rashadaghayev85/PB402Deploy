using System.ComponentModel.DataAnnotations;

namespace OrganicFood_MiniProject.Areas.Admin.ViewModels.Discount
{
    public class DiscountCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal DiscountPercent { get; set; }
    }
}
