

namespace Organic_Food_MVC_Project.Models.Home
{
    public class Advertisement : BaseEntity
    {
        public string Product { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
