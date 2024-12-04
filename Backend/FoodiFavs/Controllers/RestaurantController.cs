using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FF.Models.Dto.RestaurantDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
                    r.Rating,
                    r.Cuisine,
                    r.Budget,
                    r.Location,
                    r.ImgUrl,
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
                    r.Rating,
                    r.Cuisine,
                    r.Budget,
                    r.Location,
                    r.ImgUrl,
                    r.Description,
                    Reviews = r.ReviweNav.Select(review => new
                    {
                        review.Id,
                        review.Rating,
                        review.Comment,
                        review.CreatedAt,
                        UserName = review.UserNav.UserName,
                        UserId=review.UserNav.Id,
                        
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
        [Authorize(Roles ="Admin")]
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
                ImgUrl=obj.ImgUrl,

            };

            _db.Restaurants.Add(model);
            _db.SaveChanges();
            return Ok(obj);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteRestaurant-ById ")]
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles ="Admin")]
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
                    r.Description,
                    r.Rating,
                    r.Location,
                    r.Cuisine,
                    r.Budget,
                    r.ImgUrl
                })
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound(new { message = "No matching restaurants found." });
            }

            return Ok(results);
        }
        [HttpGet("Filter-By-Cuisine")]
        public async Task<IActionResult> Filter([FromQuery] string? cuisine, [FromQuery] string? budget)
        {
            // Build the query to filter restaurants
            var query = _db.Restaurants.AsQueryable();

            // Filter restaurants based on cuisine (if provided)
            if (!string.IsNullOrEmpty(cuisine))
            {
                query = query.Where(r => r.Cuisine.ToLower() == cuisine.ToLower());
            }
            // Filter restaurants based on budget (if provided)
            if (!string.IsNullOrEmpty(budget))
            {
                switch (budget.ToLower())
                {
                    case "low":
                        query = query.Where(r => r.Budget >= 1 || r.Budget < 5); // Assuming "low" budget is between 1 and 5
                        break;
                    case "mid":
                        query = query.Where(r => r.Budget >= 5 && r.Budget <10 ); // "mid" budget is between 5 and 10
                        break;
                    case "high":
                        query = query.Where(r => r.Budget >= 10); // "high" budget is 10 and above
                        break;
                    default:
                        return BadRequest("Invalid budget filter. Use 'low', 'mid', or 'high'.");
                }
            }

            // Sorting restaurants by Budget first and then by Cuisine
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
                .OrderBy(r => r.Budget)      // Sort by Budget (ascending)
                .ThenBy(r => r.Cuisine)      // Then sort by Cuisine (alphabetical order)
                .ToListAsync();

            // If no results found
            if (!sortedRestaurants.Any())
            {
                return NotFound("No restaurants found matching the given criteria.");
            }

            return Ok(sortedRestaurants);
        }


    }
}
