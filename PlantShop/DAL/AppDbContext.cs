using PlantShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PlantShop.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace PlantShop.DAL
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole , string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }


        public DbSet<Header>? Headers { get; set; }

        public DbSet<HeaderInfo>? HeaderInfos { get; set; }
        public DbSet<Category>? Category { get; set; }
        public DbSet<Product>? Product { get; set; }
        public DbSet<Garden>? Gardens { get; set; }
        public DbSet<Plant>? Plants { get; set; }
        public DbSet<Shopping>? Stoppings { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<GoogleMap>? GoogleMaps { get; set; }
        public DbSet<Footer>? Footers { get; set; }

        public DbSet<Basket> Baskets { get; set; }  

        public DbSet<UserBasket> UserBaskets { get; set; }



        public DbSet<Files> Files { get; set; }  
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

        }
    }
}
