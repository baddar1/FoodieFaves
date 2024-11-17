using FF.Models;
using FF.Models.Secuirty;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FF.Data.Access.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {
            
        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<PendingUser> PendingUsers { get; set; }
        public DbSet<Points> Points { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        builder.Entity<Restaurant>()
                   .HasMany(r => r.ReviweNav)
                   .WithOne(r => r.RestaurantNav)
                   .HasForeignKey(r => r.RestaurantId);

            // Configure one-to-many relationship between User and Review
            builder.Entity<User>()
                   .HasMany(u => u.Reviews)
                   .WithOne(r => r.UserNav)
                   .HasForeignKey(r => r.UserId);

            builder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id =1,
                    Name = "Nashville",
                    phoneNumber = "0799902599",
                    Email = string.Empty,
                    Location = "Abdllah Ghosheh Street",
                    Cuisine = "Fried Chicken",
                    Budget = 3.5,
                    ImgUrl = "Photo",

                    
                },
                new Restaurant
                {
                    Id =2,
                    Name = "X Burger",
                    phoneNumber = "0790067776",
                    Email = string.Empty,
                    Location = "Abdoun Circle",
                    Cuisine = "Burger",
                    Budget = 4,
                    ImgUrl = "Photo",
                }
                );
        builder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    UserId="1",
                    RestaurantId = 1, // Foreign key to Restaurant
                    Rating = 4.7,
                    Comment = "Nashville Fried Chicken, so so perfect !!",
                    Likes = 100
                },
                new Review
                {
                    Id = 2,
                    UserId="2",
                    RestaurantId = 1, // Foreign key to Restaurant
                    Rating = 4.5,
                    Comment = "Nashville Fried Chicken, so perfect !!",
                    Likes = 100
                }
                
            );
        builder.Entity<User>().HasData(
               new User
               {
                   Id = "1",
                   UserName = "YazeedNada",
                   Password= "Yazeed12.",
                   Email = "YazeedNada@gmail.com"
               },
               new User
               {
                   Id = "2",
                   UserName = "Mohammadbaddar",
                   Password ="Mohd12.",
                   Email = "Mohammadbaddar@gmail.com"
               }
           );

        }

    }
}
