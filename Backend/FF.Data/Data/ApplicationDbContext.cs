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
        public DbSet<Vouchers> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TopReviewForUser>()
            .HasOne(tr => tr.ReviewNav)
            .WithMany(r => r.TopReviews)
            .HasForeignKey(tr => tr.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
            .HasOne(r => r.RestaurantNav)  // Assuming the Review entity has a Restaurant property
            .WithMany()  // Assuming no navigation property on the Restaurant entity
            .HasForeignKey(r => r.RestaurantId)  // Assuming the foreign key is named RestaurantId
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>()
               .HasOne(o => o.ReviewNav)
               .WithOne(r => r.OrderNav)
               .HasForeignKey<Order>(o => o.ReviewId)
               .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Restaurant>()
                   .HasMany(r => r.ReviweNav)
                   .WithOne(r => r.RestaurantNav)
                   .HasForeignKey(r => r.RestaurantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Like>()
                .HasOne(l => l.Review)
                .WithMany(r => r.LikesNav)
                .HasForeignKey(l => l.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Notification>()
                .HasOne(n => n.ReviewNav)
                .WithMany(r => r.NotificationNav)
                .HasForeignKey(n => n.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure one-to-many relationship between User and Review
            builder.Entity<User>()
                   .HasMany(u => u.Reviews)
                   .WithOne(r => r.UserNav)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
            // Configure relationship between User and TopReviewForUser
            builder.Entity<TopReviewForUser>()
                .HasOne(tr => tr.UserNav)
                .WithMany(u => u.TopReviews)
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure relationship between Review and TopReviewForUser
            builder.Entity<TopReviewForUser>()
                .HasOne(tr => tr.ReviewNav)
                .WithMany(r => r.TopReviews)
                .HasForeignKey(tr => tr.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
            // Relationship: Restaurant -> TopReviewForUser
            builder.Entity<TopReviewForUser>()
                .HasOne(tr => tr.RestaurantNav)
                .WithMany(r => r.TopReviews)
                .HasForeignKey(tr => tr.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);
            //restaurant entity
            builder.Entity<Review>()
               .HasOne(r => r.RestaurantNav)
               .WithMany(r => r.ReviweNav)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FavoriteRestaurants>()
                .HasOne(fr => fr.Restaurant)
                .WithMany(r => r.FavoriteRestaurants)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Order>()
                .HasOne(o => o.Restaurant)
                .WithMany(r => r.Orders)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Points>()
                .HasOne(p => p.Restaurant)
                .WithMany(r => r.UserRestaurantPoints)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Notification>()
                .HasOne(n => n.ReviewNav)
                .WithMany(r => r.NotificationNav)
                .HasForeignKey(n => n.ReviewId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Restaurant>().HasData(
                new Restaurant
                {
                    Id =1,
                    Name = "Nashville",
                    phoneNumber = "0799902599",
                    Email = string.Empty,
                    Location = "Abdllah Ghosheh Street",
                    Cuisine = ["Fried Chicken","shawarmah"],
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
                    Cuisine = ["Burger", "shawarmah"],
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
                    Cuisine = ["shawarmah"],
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
                     Cuisine = ["meet","chiken"],
                     Budget = 2,
                     ImgUrl = "Photo",


                 }
                );
            builder.Entity<FavoriteRestaurants>().
                HasKey(f => new { f.UserId, f.RestaurantId });


            builder.Entity<FavoriteRestaurants>()
             .HasOne(f => f.Restaurant)
             .WithMany(r => r.FavoriteRestaurants)
             .HasForeignKey(e => e.RestaurantId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FavoriteRestaurants>()
             .HasOne(f => f.User)
             .WithMany(r => r.FavoriteRestaurants)
             .HasForeignKey(e => e.UserId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FavoriteBlogger>().
                HasKey(f => new { f.UserId, f.BloggerId });

            builder.Entity<FavoriteBlogger>().
                HasOne(f => f.User)
                .WithMany(u => u.FavoriteBloggers)
                .HasForeignKey(e => e.UserId)
                 .OnDelete(DeleteBehavior.NoAction);



        }

    }
}
