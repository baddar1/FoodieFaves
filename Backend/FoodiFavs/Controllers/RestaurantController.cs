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
        [HttpGet("Get-Restaurant-Info")] //End point
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            var restaurantsWithReviews = await _db.Restaurants
                .Include(r => r.ReviweNav)
                .ThenInclude(r=>r.UserNav)
                .ToListAsync();

            return Ok(restaurantsWithReviews);
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
            var GetRestaurant = _db.Restaurants.FirstOrDefault(r => r.Id==Id);
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

            _db.SaveChanges();
            return NoContent();
        }
       
       

    }
}
