using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public UserController(ILogger<RestaurantController> logger, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("Add-Favorite-Restaurants")]
        public async Task<IActionResult> AddFavoriteRestaurant (int RestaurantId)
        {
            
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            var restaurant = _db.Restaurants.FirstOrDefault(r => r.Id == RestaurantId);

            var FavoriteRes = _db.FavoriteRestaurants.FirstOrDefault(f => f.RestaurantId == RestaurantId && f.UserId == user.Id);
            if (FavoriteRes is null)
            {
                var Favorite = new FavoriteRestaurants
                {
                    RestaurantId = RestaurantId,
                    UserId = user.Id
                };
                _db.FavoriteRestaurants.Add(Favorite);
                _db.SaveChanges();
                return Ok();
            }
            else
            {
                _db.FavoriteRestaurants.Remove(FavoriteRes);
                _db.SaveChanges();
                return Ok();
            }

        }
        [HttpPost("Add-Favorite-Blogger")]

        public async Task<IActionResult> AddFavoriteBlogger(string BloggerId)
        {
          
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }

            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            var blogger = _db.Users.FirstOrDefault(b => b.Id == BloggerId);

            if (blogger == null)
            {
                return BadRequest("Blogger not found");
            }

            var favoriteBloggers = _db.FavoriteBloggers.FirstOrDefault(f => f.BloggerId == BloggerId && f.UserId == user.Id);
            if (favoriteBloggers is null)
            {
                var favorite = new FavoriteBlogger
                {
                    BloggerId = BloggerId,
                    UserId = user.Id
                };
                _db.FavoriteBloggers.Add(favorite);
                _db.SaveChanges();
                
                // Create a notification for the blogger
               var notification = new Notification
                {
                    UserId = BloggerId, // Notify the blogger
                    Message = $"{user.UserName} has added you as a favorite blogger!",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    NotificationType="Favorite Blogger"
                };

                _db.Notifications.Add(notification);
                _db.SaveChanges();

                return Ok($"{blogger.UserName} has been successfully added to your favorites list!");
            }
            else
            {
                _db.FavoriteBloggers.Remove(favoriteBloggers);
                // Remove the message ****************
              
                var notification = _db.Notifications.FirstOrDefault(u =>
                    u.UserId == BloggerId && u.Message.Contains($"{user.UserName} has added you as a favorite blogger!"));
                if (notification != null)
                {
                    _db.Notifications.Remove(notification);
                }
                _db.SaveChanges();
                return Ok($"{blogger.UserName} has been successfully removed from your favorites list!");
                //return Ok("Removes");
            }
        }
        [HttpGet("Get-Favorite-Bloggers")]
        public async Task<IActionResult> GetFavoriteBloggers()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);

            if (string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized("User is not authenticated.");
            }

           
            var favoriteBloggers = await _db.FavoriteBloggers
                .Where(fb => fb.UserId == user.Id)
                .Include(fb => fb.Blogger) 
                .Select(fb => new
                {
                    BloggerId = user.Id,
                    BloggerName = fb.Blogger.UserName,
                    TopReview = fb.Blogger.Reviews
                .OrderByDescending(r => r.Rating) 
                .Select(r => new
                {
                    Restaurant = new
                    {
                        r.RestaurantId,
                        r.RestaurantNav.ImgUrl,
                        r.RestaurantNav.Name,
                    },
                    ReviewId = r.Id,
                    r.Rating,
                    r.Comment,
                    r.Likes,
                    r.CreatedAt
                })
                .FirstOrDefault()
                })
                .ToListAsync();

            if (!favoriteBloggers.Any())
            {
                return NotFound("No favorite bloggers found for the user.");
            }

            return Ok(favoriteBloggers);
        }
        [HttpGet("Get-Favorite-Restaurants")]
        public async Task<IActionResult> GetFavoriteRestaurants()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged inuser
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null) {
                return BadRequest("User not found");
            }
            
            var FavoriteList = _db.FavoriteRestaurants.
                Where(c => c.UserId == user.Id).
                Include(f => f.Restaurant).
                Select(r=> new
                    { 
                       r.Restaurant.ImgUrl,
                       r.Restaurant.Name,
                       r.Restaurant.Cuisine,
                       r.Restaurant.Rating,
                       r.Restaurant.Description

                    }
                
                )
                .ToList();
            if (FavoriteList.Count == 0) {
                return Ok("The favorite List is empty !!");
            }


            return Ok(FavoriteList);
        }
        [HttpDelete("Delete-Favorite-Blogger")]
        public async Task<IActionResult> DeleteFavoriteBlogger(string bloggerId)
        {
            
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Find the favorite blogger record
            var favoriteBlogger = await _db.FavoriteBloggers
                .FirstOrDefaultAsync(fb => fb.UserId == user.Id && fb.BloggerId == bloggerId);

            if (favoriteBlogger == null)
            {
                return NotFound("Favorite blogger not found for the user.");
            }

            // Remove the favorite blogger
            _db.FavoriteBloggers.Remove(favoriteBlogger);
            await _db.SaveChangesAsync();

            return Ok("Favorite blogger removed successfully." );
        }
        [HttpDelete("DeleteFavoriteRestaurant")]
        public async Task<IActionResult> DeleteFavoriteRestaurant(int restaurantId)
        {
           
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Find the favorite restaurant record
            var favoriteRestaurant = await _db.FavoriteRestaurants
                .FirstOrDefaultAsync(fr => fr.UserId == user.Id && fr.RestaurantId == restaurantId);

            if (favoriteRestaurant == null)
            {
                return NotFound("Favorite restaurant not found for the user.");
            }

            // Remove the favorite restaurant
            _db.FavoriteRestaurants.Remove(favoriteRestaurant);
            await _db.SaveChangesAsync();

            return Ok("Favorite restaurant removed successfully." );
        }
        [HttpGet("Get-Notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }

            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Fetch unread notifications for the user
            var notifications = _db.Notifications
                                   .Where(n => n.UserId == user.Id && !n.IsRead)
                                   .OrderByDescending(n => n.CreatedAt)
                                   .Select(n => new { n.Message, n.CreatedAt })
                                   .ToList();

            if (notifications.Count == 0)
            {
                return Ok("No new notifications.");
            }

            return Ok(notifications);
        }
        [HttpPost("Mark-Notification-As-Read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            var notification = _db.Notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification == null)
            {
                return NotFound("Notification not found.");
            }

            notification.IsRead = true;
            _db.SaveChanges();
            return Ok(); // Notification marked as read
        }
        [HttpGet("Get-All-Reviews")]
        public async Task<IActionResult> GetAllUserReviews()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
          

            var user1 = _db.Users
                .Where(u => u.Id==user.Id)
                .Select(u=> new 
                {
                    u.Id,
                    u.UserName,
                    u.ReviewCount,
                    u.TotalLikes,
                    u.TotalPoints,
                    Reviews = u.Reviews.Select(review => new
                    {
                        review.Id,
                        review.RestaurantNav.Name,
                        review.Rating,
                        review.Comment,
                        review.CreatedAt,

                    }).ToList()
                }).FirstOrDefault();

            if (user1 == null)
                return NotFound();

            return Ok(user1);


        }

    }
}
