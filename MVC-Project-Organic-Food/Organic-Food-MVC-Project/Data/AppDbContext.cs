using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Models;
using Organic_Food_MVC_Project.Models.Blog;
using Organic_Food_MVC_Project.Models.Home;
using Organic_Food_MVC_Project.Models.User;

namespace Organic_Food_MVC_Project.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> option) : base(option) { }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Service> Services { get; set; } 
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Advertisement> Advertisement { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<DiscountProduct> DiscountsProducts { get; set; }

    }
}
