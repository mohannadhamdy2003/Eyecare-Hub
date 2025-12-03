using EyeCareHub.DAL.Entities.Content_Education;
using EyeCareHub.DAL.Entities.OrderAggregate;
using EyeCareHub.DAL.Entities.ProductInfo;
using EyeCareHub.DAL.Entities.ProductInfo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EyeCareHub.DAL.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> Options):base(Options)
        {
            
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<ProductTypes> ProductTypes { get; set; }
        public DbSet<ProductBrands> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }


        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }


        public DbSet<Article> Articles { get; set; }
        public DbSet<EducationalCategory> EducationalCategories { get; set; }
        public DbSet<SavedArticle> SavedByUsers { get; set; }
        public DbSet<ArticleLove> Loves { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

    

}
