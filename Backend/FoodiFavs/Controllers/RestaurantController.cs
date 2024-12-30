using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FF.Models.Dto.RestaurantDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Http;

namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly HttpClient _httpClient;
        public RestaurantController(ILogger<RestaurantController> logger,ApplicationDbContext db,IUnitOfWork unitOfWork, HttpClient httpClient)
        {
            _logger = logger;
            _db = db;
            _unitOfWork = unitOfWork;  
            _httpClient = httpClient;
        }
        [HttpGet("Get-All-Restaurants")] //End point
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRestaurants()
        {
            // Fetch all restaurants from the database
            var restaurants = await _db.Restaurants
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.phoneNumber,
                    r.Rating,
                    r.Cuisine,
                    r.ReviewCount,
                    r.Budget,
                    r.Location,
                    r.ImgUrl,
                    r.AdditionalRestaurantImages,
                    r.LogoImg,
                    r.Open,
                    r.Close,
                    r.LiveSite,
                    r.Description
                })
                .ToListAsync();

            // Check if no restaurants are found
            if (!restaurants.Any())
            {
                return NotFound("No restaurants found.");
            }

            // Return the list of restaurants
            return Ok(restaurants);
        }

        [HttpGet("GetRestaurants-ById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<RestaurantDto> GetRestaurant(int Id)//Get Restaurant By Id
        {
            if (Id==0)
            {
                return BadRequest();
            }
            var GetRestaurant = _db.Restaurants
                .Where(r => r.Id == Id)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.phoneNumber,
                    r.Rating,
                    r.ReviewCount,
                    Cuisine = r.Cuisine ?? new List<string>(),
                    r.Budget,
                    r.Location,
                    r.ImgUrl,
                    r.AdditionalRestaurantImages,
                    r.LogoImg,
                    r.Open,
                    r.Close,
                    r.LiveSite,
                    r.Description,
                    Reviews = r.ReviweNav.Select(review => new
                    {
                        review.Id,
                        review.Rating,
                        review.Comment,
                        review.CreatedAt,
                        UserName = review.UserNav.UserName,
                        UserId=review.UserNav.Id,
                        UserImg=review.UserNav.ImgUrl,
                        TotalLikes=review.Likes,
                        
                    }).ToList()
                    
                })
                .FirstOrDefault();
            if (GetRestaurant!=null)
            {
                return Ok(GetRestaurant);
            }
            else
            {
                return NotFound();
            }

        }
        [HttpPost("Add-Restaurant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize(Roles ="Admin")]
        public ActionResult AddRestaurant([FromBody] RestaurantDto obj)
        {

            if (_db.Restaurants.FirstOrDefault
                (u => u.Name.ToLower()==obj.Name.ToLower())!=null)
            {
                ModelState.AddModelError("CustomError", "Restaurant Name is already Taken");
                return BadRequest(ModelState);
            }
            if (obj == null)
            {
                return BadRequest(obj);
            }
            Restaurant model = new()
            {
                Name = obj.Name,
                phoneNumber=obj.phoneNumber,
                Email=obj.Email,
                Location=obj.Location,
                Cuisine=obj.Cuisine,
                Budget=obj.Budget,
                LiveSite=obj.LiveSite,
                Open=obj.Open,
                Close=obj.Close,
                Description=obj.Description,

            };

            _db.Restaurants.Add(model);
            _db.SaveChanges();

            return Ok((new { model.Id }));
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteRestaurant-ById")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteRestaurant(int restaurantId)
        {
            // Find the restaurant by its ID
            var restaurant = await _db.Restaurants
                                       .Include(r => r.ReviweNav)
                                       .Include(r => r.TopReviews)
                                       .Include(r => r.FavoriteRestaurants)
                                       .Include(r => r.UserRestaurantPoints)
                                       .Include(r => r.Orders)
                                       .FirstOrDefaultAsync(r => r.Id == restaurantId);

            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            // Delete images associated with the restaurant
            if (!string.IsNullOrEmpty(restaurant.ImgUrl))
            {
                DeleteImageFile(restaurant.ImgUrl);
            }

            if (!string.IsNullOrEmpty(restaurant.LogoImg))
            {
                DeleteImageFile(restaurant.LogoImg);
            }

            if (restaurant.AdditionalRestaurantImages != null)
            {
                foreach (var imageUrl in restaurant.AdditionalRestaurantImages)
                {
                    DeleteImageFile(imageUrl);
                }
            }

            // Call the DeleteReview API for each review associated with the restaurant
            if (restaurant.ReviweNav != null)
            {
                foreach (var review in restaurant.ReviweNav)
                {
                    // Make the API call to delete the review
                    var deleteReviewUrl = $"https://localhost:7063/api/Review/DeleteReview-ById/{review.Id}";

                    // Make the API call to delete the review
                    var response = await _httpClient.DeleteAsync(deleteReviewUrl);
                    var m = response.Content;
                    if (!response.IsSuccessStatusCode)
                    {
                        // Handle the error appropriately if needed
                        return StatusCode((int)response.StatusCode, "Failed to delete review.");
                    }
                }
            }

            // Remove top reviews related to this restaurant
            if (restaurant.TopReviews != null)
            {
                _db.TopReviewForUsers.RemoveRange(restaurant.TopReviews);
            }

            // Remove users' favorite entries for this restaurant
            if (restaurant.FavoriteRestaurants != null)
            {
                _db.FavoriteRestaurants.RemoveRange(restaurant.FavoriteRestaurants);
            }

            // Remove user points related to this restaurant
            if (restaurant.UserRestaurantPoints != null)
            {
                _db.Points.RemoveRange(restaurant.UserRestaurantPoints);
            }

            // Remove any orders related to this restaurant
            if (restaurant.Orders != null)
            {
                _db.Orders.RemoveRange(restaurant.Orders);
            }

            // Remove the restaurant itself
            _db.Restaurants.Remove(restaurant);

            // Save all changes
            await _db.SaveChangesAsync();

            return NoContent(); // Return a success response
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("UpadteRestaurant-ById")]
        //[Authorize(Roles ="Admin")]
        public IActionResult UpdateRestaurant(int Id,[FromBody]UpdateRestaurantDto obj)
        {
  
            var Restaurant = _db.Restaurants.Find(Id);

            if (Restaurant == null) 
            {
                return NotFound();
            }
        
            Restaurant.Name = obj.Name;
            Restaurant.Location = obj.Location;
            Restaurant.Cuisine = obj.Cuisine;
            Restaurant.phoneNumber=obj.phoneNumber;
            Restaurant.Email = obj.Email;
            Restaurant.Budget = obj.Budget;
            Restaurant.ImgUrl = obj.ImgUrl;
            Restaurant.LogoImg=obj.LogoImg;
            Restaurant.LiveSite=obj.LiveSite;
            Restaurant.Open=obj.Open;
            Restaurant.Close=obj.Close;
            Restaurant.Description=obj.Description;

            _db.SaveChanges();
            return NoContent();
        }
        [HttpGet("SearchRestaurants")]
        public async Task<IActionResult> SearchRestaurants(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest(new { message = "Search keyword cannot be empty." });
            }

            var normalizedKeyword = keyword.Replace(" ", "").ToLower();

            var query = _db.Restaurants
                .Where(r =>
                    (EF.Functions.Like(r.Name.Replace(" ", "").ToLower(), $"%{normalizedKeyword}%")));

            var results = await query
                .OrderBy(r => r.Name)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.phoneNumber,
                    r.Rating,
                    r.Cuisine,
                    r.ReviewCount,
                    r.Budget,
                    r.Location,
                    r.ImgUrl,
                    r.Open,
                    r.Close,
                    r.LiveSite,
                    r.Description
                })
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound(new { message = "No matching restaurants found." });
            }

            return Ok(results);
        }
        [HttpGet("Filter-By-Cuisine-Budget-Rating")]
        public async Task<IActionResult> Filter([FromQuery] List<string>? cuisine, [FromQuery] string? budget, [FromQuery] int? rating)
        {
            var query = _db.Restaurants.AsQueryable();

            // Filter by cuisine
            if (cuisine != null && cuisine.Count > 0)
            {
                // Search for restaurants that have any of the cuisines selected by the user
                query = query.Where(r => r.Cuisine.Any(c => cuisine.Contains(c)));
            }

            // Filter by budget
            if (!string.IsNullOrEmpty(budget))
            {
                switch (budget.ToLower())
                {
                    case "low":
                        query = query.Where(r => r.Budget >= 1 && r.Budget < 5);
                        break;
                    case "mid":
                        query = query.Where(r => r.Budget >= 5 && r.Budget < 10);
                        break;
                    case "high":
                        query = query.Where(r => r.Budget >= 10);
                        break;
                    default:
                        return BadRequest("Invalid budget filter. Use 'low', 'mid', or 'high'.");
                }
            }

            // Filter by rating
            if (rating.HasValue)
            {
                if (rating < 1 || rating > 5)
                {
                    return BadRequest("Rating must be between 1 and 5.");
                }

                query = query.Where(r => r.Rating >= rating);
            }

            // Fetch data from the database
            var restaurants = await query
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.phoneNumber,
                    r.Rating,
                    r.Cuisine,
                    r.ReviewCount,
                    r.Budget,
                    r.Location,
                    r.ImgUrl,
                    r.Open,
                    r.Close,
                    r.Description
                })
                .ToListAsync();

            // Return 404 if no results
            if (!restaurants.Any())
            {
                return NotFound("No restaurants found matching the given criteria.");
            }

            return Ok(restaurants);
        }

        [HttpGet("SorteRestaurantByRating")]
        public async Task<IActionResult> SorteRestaurantByRating()
        {
            // Get all reviews for the restaurant
            var restaurant = await _db.Restaurants.ToListAsync();

            if (restaurant == null || !restaurant.Any())
            {
                return NotFound("No reviews found for the given restaurant.");
            }


            var sortedRestaurant = restaurant
                .OrderByDescending(r => r.Rating)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.phoneNumber,
                    r.Rating,
                    r.Cuisine,
                    r.ReviewCount,
                    r.Budget,
                    r.Location,
                    r.ImgUrl,
                    r.Open,
                    r.Close,
                    r.LiveSite,
                    r.Description
                })
                .ToList();

            return Ok(sortedRestaurant);
        }
        [HttpPost("AddLogo")]
        public async Task<IActionResult> AddLogo(int restaurantId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
                return NotFound("Restaurant not found.");

           
            var relativePath = await SaveImage(restaurantId, file);

            
            restaurant.LogoImg = relativePath;

            await _db.SaveChangesAsync();
            return Ok(new { LogoUrl = relativePath });
        }
        [HttpDelete("DeleteLogo")]
        public async Task<IActionResult> DeleteLogo(int restaurantId)
        {
            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
                return NotFound("Restaurant not found.");

            if (string.IsNullOrEmpty(restaurant.LogoImg))
                return BadRequest("No logo image to delete.");

         
            DeleteImageFile(restaurant.LogoImg);
            restaurant.LogoImg = null;

            await _db.SaveChangesAsync();
            return Ok("Logo deleted successfully.");
        }   
        [HttpPost("AddProfileImage")]
        public async Task<IActionResult> AddProfileImage(int restaurantId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
                return NotFound("Restaurant not found.");

            
            var relativePath = await SaveImage(restaurantId, file);

        
            restaurant.ImgUrl = relativePath;

            await _db.SaveChangesAsync();
            return Ok(new { ProfileImageUrl = relativePath });
        }       
        [HttpDelete("DeleteProfileImage")]
        public async Task<IActionResult> DeleteProfileImage(int restaurantId)
        {
            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
                return NotFound("Restaurant not found.");

            if (string.IsNullOrEmpty(restaurant.ImgUrl))
                return BadRequest("No profile image to delete.");

         
            DeleteImageFile(restaurant.ImgUrl);
            restaurant.ImgUrl = null;

            await _db.SaveChangesAsync();
            return Ok("Profile image deleted successfully.");
        }

        [HttpPost("AddAdditionalImage")]
        public async Task<IActionResult> AddAdditionalImage(int restaurantId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
                return NotFound("Restaurant not found.");

            restaurant.AdditionalRestaurantImages ??= new List<string>();

            if (restaurant.AdditionalRestaurantImages.Count >= 5)
                return BadRequest("You can only have 5 additional images. Please delete an image before adding a new one.");

            var relativePath = await SaveImage(restaurantId, file);

            restaurant.AdditionalRestaurantImages.Add(relativePath);

            await _db.SaveChangesAsync();
            return Ok(new { AdditionalImageUrl = relativePath });
        }

        [HttpDelete("DeleteAdditionalImage")]
        public async Task<IActionResult> DeleteAdditionalImageByIndex(int restaurantId, int index)
        {
            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
                return NotFound("Restaurant not found.");

            if (restaurant.AdditionalRestaurantImages == null || restaurant.AdditionalRestaurantImages.Count == 0)
                return BadRequest("No additional images found.");

            if (index < 0 || index >= restaurant.AdditionalRestaurantImages.Count)
                return BadRequest($"Invalid index. Provide an index between 0 and {restaurant.AdditionalRestaurantImages.Count - 1}.");

            var imageUrl = restaurant.AdditionalRestaurantImages[index];

            restaurant.AdditionalRestaurantImages.RemoveAt(index);

            DeleteImageFile(imageUrl);

            await _db.SaveChangesAsync();
            return Ok($"Image at index {index} deleted successfully.");
        }

        private async Task<string> SaveImage(int restaurantId, IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var restaurantFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RestaurantImgs", $"Restaurant_{restaurantId}");

            if (!Directory.Exists(restaurantFolder))
                Directory.CreateDirectory(restaurantFolder);

            var filePath = Path.Combine(restaurantFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return $"/RestaurantImgs/Restaurant_{restaurantId}/{fileName}";
        }
        private void DeleteImageFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                throw new ArgumentException("File path is invalid.", nameof(relativePath));
                   
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                Console.WriteLine($"File deleted: {filePath}");
            }
            else
            {
                Console.WriteLine($"File not found: {filePath}");
            }
        }
        [HttpGet("restaurant-count")]
        public async Task<ActionResult<int>> GetReviewesNumber()
        {
            var RestaurantNumber = _db.Restaurants.Select(r => r.Id).Count();
            return RestaurantNumber;
        }
    }
}
