using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Organic_Food_MVC_Project.Data;
using Organic_Food_MVC_Project.Helpers;
using Organic_Food_MVC_Project.Models.User;
using Organic_Food_MVC_Project.Services;
using Organic_Food_MVC_Project.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var conString = builder.Configuration.GetConnectionString("Default") ??
     throw new InvalidOperationException("Connection string 'Default'" +
    " not found.");
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(conString));

builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Smtp"));

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IBlogCategoryService, BlogCategoryService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
