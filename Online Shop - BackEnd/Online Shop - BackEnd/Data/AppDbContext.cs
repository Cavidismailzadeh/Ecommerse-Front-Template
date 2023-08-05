
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_Shop___BackEnd.Models;

namespace Online_Shop___BackEnd.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<PageHeader> PageHeaders { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Login_Register> Login_Registers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogImage> BlogImages { get; set; }
        public DbSet<BlogSubCategory> BlogSubCategories { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<HomeCategory> HomeCategories { get; set; }
    }
}
