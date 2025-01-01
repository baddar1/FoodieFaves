using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using FF.Models.Dto.RestaurantDto;
using FF.Models.Dto.UserDto;
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
            if (FavoriteRes!=null) 
            {
                return BadRequest($"{restaurant.Name} is already in the favorite list");
            }
        
                var Favorite = new FavoriteRestaurants
                {
                    RestaurantId = RestaurantId,
                    UserId = user.Id
                };
                _db.FavoriteRestaurants.Add(Favorite);
           
                user.UnReadNotiNum++;
                _db.SaveChanges();
                return Ok();
            
        }
        [HttpGet("Get-Favorite-Restaurants")]
        public async Task<IActionResult> GetFavoriteRestaurants()
        {
            
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            // Fetch the favorite restaurants 
            var favoriteRestaurants = await _db.FavoriteRestaurants
                .Where(fr => fr.UserId == user.Id) 
                .Include(fr => fr.Restaurant) 
                .Select(fr => new
                {
                    fr.Restaurant.Id,
                    fr.Restaurant.ReviewCount,
                    fr.Restaurant.Name,    
                    fr.Restaurant.Open,
                    fr.Restaurant.Close,
                    fr.Restaurant.Description,
                    fr.Restaurant.ImgUrl,     
                    fr.Restaurant.Location, 
                    fr.Restaurant.Cuisine,    
                    fr.Restaurant.Rating      
                })
                .ToListAsync();

            if (!favoriteRestaurants.Any())
            {
                return NotFound("No favorite restaurants found for the user.");
            }

            return Ok(favoriteRestaurants); // Return the favorite restaurants list
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
            if (favoriteBloggers!=null)
            {
                return BadRequest($"{blogger.UserName} Is already in the list");
            }
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
                    BloggertId = BloggerId,
                    NotificationType="Favorite Blogger"
                };
            blogger.UnReadNotiNum = blogger.UnReadNotiNum ?? 0;
            blogger.UnReadNotiNum++;

            user.UnReadNotiNum++;
                _db.Notifications.Add(notification);
                _db.SaveChanges();

                return Ok($"{blogger.UserName} has been successfully added to your favorites list!");
                
        }
        [HttpGet("Get-Favorite-Bloggers")]
        public async Task<IActionResult> GetFavoriteBloggers()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the user's ID or name from the claims
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return Unauthorized("User is not authenticated.");
            }

            var favoriteBloggers = await _db.FavoriteBloggers
                .Where(fb => fb.UserId == user.Id) 
                .Include(fb => fb.Blogger) 
                .Select(fb => new
                {
                    fb.Blogger.Id,
                    fb.Blogger.UserName,
                    fb.Blogger.ReviewCount,
                    fb.Blogger.ImgUrl,
                    ReviewInfo = fb.Blogger.TopReviews
                        .OrderByDescending(tr => tr.TopRate)
                        .Select(tr => new
                        {
                            tr.ReviewId,
                            tr.ReviewNav.Comment,
                            tr.ReviewNav.Rating,
                            tr.ReviewNav.CreatedAt,
                            tr.ReviewNav.Likes,
                            
                        }).FirstOrDefault(),

                    // Select the restaurant info
                    RestaurantInfo = fb.Blogger.TopReviews
                        .OrderByDescending(tr => tr.TopRate)
                        .Select(tr => new
                        {
                            tr.RestaurantId,
                            tr.RestaurantNav.Name,
                            tr.RestaurantNav.Cuisine,
                            tr.RestaurantNav.Location,
                            tr.RestaurantNav.ImgUrl
                        }).FirstOrDefault()
                })
                .ToListAsync();

            if (!favoriteBloggers.Any())
            {
                return NotFound("No favorite bloggers found.");
            }

            return Ok(favoriteBloggers); // Return the favorite bloggers list
        }

        [HttpGet("Get-Top-Bloggers")]
        public async Task<IActionResult> TopReviewers()
        {
            // Fetch top 10 
            var topReviewers = await _db.Users
                .OrderByDescending(u => u.ReviewCount) 
                .Where(u=>u.ReviewCount>10)
                .Take(10) // Get top 10 users
                // Select the User info
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.ReviewCount,
                    u.ImgUrl,
                    // Select the review info
                    ReviewInfo = u.TopReviews
                        .OrderByDescending(tr => tr.TopRate)
                        .Select(tr => new
                        {
                            tr.ReviewId,
                            tr.ReviewNav.Comment,
                            tr.ReviewNav.Rating,
                            tr.ReviewNav.CreatedAt,
                            tr.ReviewNav.Likes
                        }).FirstOrDefault(),

                    // Select the restaurant info
                    RestaurantInfo = u.TopReviews
                        .OrderByDescending(tr => tr.TopRate)
                        .Select(tr => new
                        {
                            tr.RestaurantId,
                            tr.RestaurantNav.Name,
                            tr.RestaurantNav.Cuisine,
                            tr.RestaurantNav.Location,
                            tr.RestaurantNav.ImgUrl
                        }).FirstOrDefault()
                })
                .ToListAsync(); 

            return Ok(topReviewers);
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
        [HttpGet("Get-All-User-Reviews")]
        public async Task<IActionResult> GetAllUserReviews()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user==null) 
            {
                return BadRequest();
            }
            var reviews = _db.Reviews.FirstOrDefault(r => r.UserId==user.Id);
            if (reviews == null)
            {
                var userReviews = _db.Users
               .Where(u => u.Id==user.Id)
               .Select(u => new
               {
                   u.Id,
                   u.UserName,
                   u.ReviewCount,
                   u.TotalLikes,
                   u.TotalPoints,
                   u.ImgUrl,
               }).FirstOrDefault();
                return Ok(userReviews);
            }
            else
            {
                var userReviews = _db.Users
                   .Where(u => u.Id==user.Id)
                   .Select(u => new
                   {
                       u.Id,
                       u.UserName,
                       u.ReviewCount,
                       u.TotalLikes,
                       u.TotalPoints,
                       u.ImgUrl,
                       Reviews = u.Reviews.Select(review => new
                       {
                           review.Id,
                           review.RestaurantId,
                           review.RestaurantNav.Name,
                           review.RestaurantNav.ImgUrl,
                           review.RestaurantNav.Location,
                           review.Rating,
                           review.Comment,
                           review.CreatedAt,
                           review.Likes,

                       }).ToList()
                   }).FirstOrDefault();
                return Ok(userReviews);
            }
            
        }
        [HttpPost("upload-Users-images")]
        public async Task<IActionResult> UploadUserImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);

            //unique file name 
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            //Define the folder name
            var UserFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UsersImgs", $"User.{user.UserName}");

            //Check if the directory exists if not create it
            if (!Directory.Exists(UserFolder))
            {
                Directory.CreateDirectory(UserFolder);
            }

            //save the image
            var filePath = Path.Combine(UserFolder, fileName);

            //Save the image to folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var relativePath = $"/UsersImgs/User.{user.UserName}/{fileName}";
      
            user.ImgUrl = relativePath; 
        

            await _db.SaveChangesAsync();

            return Ok(new { ImageUrl = relativePath });
        }
        [HttpGet("Get-User-Reviews-ById")]
        public IActionResult GetUserReviews(string Id)
        {

            var user = _db.Users.FirstOrDefault(u => u.Id == Id);
            if (user == null)
            {
                return BadRequest();
            }
            var userReviews = _db.Users
               .Where(u => u.Id==user.Id)
               .Select(u => new
               {
                   u.Id,
                   u.UserName,
                   u.ReviewCount,
                   u.TotalLikes,
                   u.TotalPoints,
                   u.ImgUrl,
                   Reviews = u.Reviews.Select(review => new
                   {
                       review.Id,
                       review.RestaurantId,
                       review.RestaurantNav.Name,
                       review.RestaurantNav.ImgUrl,
                       review.Rating,
                       review.Comment,
                       review.CreatedAt,
                       review.Likes,

                   }).ToList()
               }).FirstOrDefault();

            if (userReviews == null)
                return NotFound();

            return Ok(userReviews);


        }
        [HttpGet("Reviews-ILiked")]
        public IActionResult GetUserLikedReviews()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            var likedReviews = _db.Likes
               .Where(l => l.UserId == user.Id)
               .Select(l => new
               {
                   ReviewId=l.ReviewId
               });
            if (likedReviews==null)
            {
                return BadRequest("No Liked Review");
            }
            return Ok(likedReviews);

       }
        [HttpGet("user-count")]
        public ActionResult<int> UserCount()
        {
            var UserCount = _db.Users.Select(u => u.Id).Count();
            return UserCount;
        }
        [HttpGet("All-Users")]
        public IActionResult GetAllUsers() 
        {
            var Users=_db.Users.ToList();
            if (Users==null) 
            {
                return BadRequest("NO Users");
            }
            return Ok(Users);
        }



    }
}
