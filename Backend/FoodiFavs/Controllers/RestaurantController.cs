using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FF.Models.Dto.RestaurantDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public RestaurantController(ILogger<RestaurantController> logger,ApplicationDbContext db,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitOfWork = unitOfWork;   
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
            return Ok(obj);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteRestaurant-ById ")]
        //[Authorize(Roles ="Admin")]
        public IActionResult DeleteRestaurant(int Id)
        {

            if (Id==0)
            {
                return BadRequest();
            }
            var Restaurant = _db.Restaurants.FirstOrDefault(u => u.Id==Id);
            if (Restaurant == null)
            {
                return NotFound();
            }
            _db.Restaurants.Remove(Restaurant);
            _db.SaveChanges();
            return NoContent();
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
        [HttpGet("Filter-By-Cuisine-Budget")]
        public async Task<IActionResult> Filter([FromQuery] List<string>? cuisine,[FromQuery] string? budget,[FromQuery] int? rating)
        {
            var query = _db.Restaurants.AsQueryable();

            // Filter by cuisine
            if (cuisine != null && cuisine.Count > 0)
            {
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

                var lowerBound = rating.Value;
                var upperBound = lowerBound + 0.9;
                query = query.Where(r => r.Rating >= lowerBound && r.Rating <= upperBound);
            }

            // Fetch and sort the data
            var sortedRestaurants = await query
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Rating,
                    r.Cuisine,
                    r.Budget,
                    r.Location,
                    r.ImgUrl
                })
                .OrderBy(r => r.Budget)
                .ThenBy(r => string.Join(", ", r.Cuisine))
                .ToListAsync();

            // Return 404 if no results
            if (!sortedRestaurants.Any())
            {
                return NotFound("No restaurants found matching the given criteria.");
            }

            return Ok(sortedRestaurants);
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
        [HttpPost("upload-Restaurant-images")]
        public async Task<IActionResult> UploadRestaurantImage(int restaurantId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            //Check if the restaurant exists
            var restaurant = await _db.Restaurants.FindAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound("Restaurant not found.");
            }

            //unique file name 
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            //Define the folder name
            var restaurantFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "RestaurantImgs", $"Restaurant_{restaurantId}");

            //Check if the directory exists if not create it
            if (!Directory.Exists(restaurantFolder))
            {
                Directory.CreateDirectory(restaurantFolder);
            }

            //save the image
            var filePath = Path.Combine(restaurantFolder, fileName);

            //Save the image to folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var relativePath = $"/RestaurantImgs/Restaurant_{restaurantId}/{fileName}";
            if (string.IsNullOrEmpty(restaurant.ImgUrl))
            {
                restaurant.ImgUrl = relativePath; // Add this as the main image if it's the first one
            }
            else
            {
                if (string.IsNullOrEmpty(restaurant.LogoImg))
                {
                    restaurant.LogoImg = relativePath;

                }
                else
                {

                    restaurant.AdditionalRestaurantImages = restaurant.AdditionalRestaurantImages ?? new List<string>();
                    restaurant.AdditionalRestaurantImages.Add(relativePath);
                }
            }

            await _db.SaveChangesAsync();

            return Ok(new { ImageUrl = relativePath });
        }




    }
}
