using FF.Models;
using FF.Models.Secuirty;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<Like> Likes { get; set; }
        public DbSet<TopReviewForUser>TopReviewForUsers { get; set; }
        public DbSet<FavoriteRestaurants> FavoriteRestaurants { get; set; }
        public DbSet<FavoriteBlogger> FavoriteBloggers { get; set; }

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
            // Configure relationship between User and TopReviewForUser
            builder.Entity<TopReviewForUser>()
                .HasOne(tr => tr.UserNav)
                .WithMany(u => u.TopReviews)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationship between Review and TopReviewForUser
            builder.Entity<TopReviewForUser>()
                .HasOne(tr => tr.ReviewNav)
                .WithMany(r => r.TopReviews)
                .HasForeignKey(tr => tr.ReviewId)
                .OnDelete(DeleteBehavior.Restrict);
            // Relationship: Restaurant -> TopReviewForUser
            builder.Entity<TopReviewForUser>()
                .HasOne(tr => tr.RestaurantNav)
                .WithMany(r => r.TopReviews)
                .HasForeignKey(tr => tr.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);

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
                },
                new Restaurant
                {
                    Id =3,
                    Name = "saj",
                    phoneNumber = "0799902599",
                    Email = string.Empty,
                    Location = "Jubiha",
                    Cuisine = "shawerma",
                    Budget = 4.1,
                    ImgUrl = "Photo",


                },
                new Restaurant
                 {
                     Id =4,
                     Name = "Reem",
                     phoneNumber = "0799902599",
                     Email = string.Empty,
                     Location = "Jubiha",
                     Cuisine = "shawerma",
                     Budget = 2,
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
                      Id = 3,
                      UserId="1",
                      RestaurantId = 3, // Foreign key to Restaurant
                      Rating = 4.1,
                      CreatedAt=DateTime.Now,
                      Comment = "so Juciyy !!",
                      Likes = 100
                  },
                new Review
                {
                    Id = 2,
                    UserId="2",
                    RestaurantId = 1, // Foreign key to Restaurant
                    Rating = 4.5,
                    CreatedAt=DateTime.Now,
                    Comment = "Nashville Fried Chicken, so perfect !!",
                    Likes = 100
                },
                new Review
                 {
                     Id = 4,
                     UserId="2",
                     RestaurantId = 1, // Foreign key to Restaurant
                     Rating = 4.5,
                     CreatedAt=DateTime.Now,
                     Comment = "Nashville Fried Chicken, so perfect !!",
                     Likes = 105
                 },
                new Review
                {
                     Id = 5,
                     UserId="2",
                     RestaurantId = 1, // Foreign key to Restaurant
                     Rating = 4.5,
                     CreatedAt=DateTime.Now,
                     Comment = "Nashville Fried Chicken, so perfect !!",
                     Likes = 99
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
            builder.Entity<FavoriteRestaurants>().
                HasKey(f => new { f.UserId, f.RestaurantId });


            builder.Entity<FavoriteRestaurants>()
             .HasOne(f => f.Restaurant)
             .WithMany(r => r.FavoriteRestaurants)
             .HasForeignKey(e => e.RestaurantId);

            builder.Entity<FavoriteRestaurants>()
             .HasOne(f => f.User)
             .WithMany(r => r.FavoriteRestaurants)
             .HasForeignKey(e => e.UserId);

            builder.Entity<FavoriteBlogger>().
                HasKey(f => new { f.UserId, f.BloggerId });

            builder.Entity<FavoriteBlogger>().
                HasOne(f => f.User)
                .WithMany(u => u.FavoriteBloggers)
                .HasForeignKey(e => e.UserId);


            
        }

    }
}
