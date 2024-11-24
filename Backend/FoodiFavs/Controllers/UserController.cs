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
            var Blogger = _db.Users.FirstOrDefault(b => b.Id == BloggerId);

            var FavoriteBloggers = _db.FavoriteBloggers.FirstOrDefault(f => f.BloggerId == BloggerId && f.UserId == user.Id);
            if (FavoriteBloggers is null)
            {
                var Favorite = new FavoriteBlogger
                {
                    BloggerId = BloggerId,
                    UserId = user.Id
                };
                _db.FavoriteBloggers.Add(Favorite);
                _db.SaveChanges();
                return Ok($"The {Blogger.UserName} has been successfully added to your favorites list !!");
            }
            else
            {
                _db.FavoriteBloggers.Remove(FavoriteBloggers);
                _db.SaveChanges();
                return Ok($"The {Blogger.UserName} has been successfully removed from your favorites list !!");
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
    }
}
