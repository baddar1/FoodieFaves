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
                return Ok($"The {restaurant.Name} has been successfully added to your favorites list !!");
            }
            else
            {
                _db.FavoriteRestaurants.Remove(FavoriteRes);
                _db.SaveChanges();
                return Ok($"The {restaurant.Name} has been successfully removed from your favorites list !!");
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
                    IsRead = false
                };

                _db.Notifications.Add(notification);
                _db.SaveChanges();

                return Ok($"The {blogger.UserName} has been successfully added to your favorites list!");
            }
            else
            {
                _db.FavoriteBloggers.Remove(favoriteBloggers);
                _db.SaveChanges();
                return Ok($"The {blogger.UserName} has been successfully removed from your favorites list!");
            }
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
                Select(r => r.Restaurant.Name)
                .ToList();
            if (FavoriteList.Count == 0) {
                return Ok("The favorite List is empty !!");
            }


            return Ok(FavoriteList);
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

    }
}
