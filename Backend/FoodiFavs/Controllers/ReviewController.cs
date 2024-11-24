using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using FF.Models.Dto.RestaurantDto;
using FF.Models.Dto.ReviewDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ReviewController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ReviewController(ILogger<ReviewController> logger, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("Get-Review-Info")] //End point
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetReviews()
        {
            var reviews = await _db.Reviews
               .Include(r => r.UserNav)
               .Include(r => r.RestaurantNav)
               .ToListAsync();

            return Ok(reviews);
        }

        [HttpGet("GetReview-ById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ReviewDto> GetReview(int Id)//Get Review By Id
        {
            if (Id==0)
            {
                return BadRequest();
            }
            var GetReview = _db.Reviews.FirstOrDefault(u => u.Id==Id);
            if (GetReview!=null)
            {
                return Ok(GetReview);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost("Add-Review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[Authorize]
        public async Task<ActionResult> AddReview([FromBody] ReviewDto obj)
        {
            if (obj == null)
            {
                return BadRequest(obj);
            }

            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.Id == obj.RestaurantId);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == obj.UserId);
            if (user == null) 
            {
                return NotFound("User not found.");
            }
            if (restaurant == null) 
            {
                return NotFound("Restaurant not found.");
            }
     
            Review model = new()
            {
                Rating=obj.Rating,
                Comment=obj.Comment,
                UserId=user.Id,
                RestaurantId=obj.RestaurantId,
            };

            _db.Reviews.Add(model);
            var userRestaurantPoints = await _db.Points
            .FirstOrDefaultAsync(p => p.UserId == user.Id && p.RestaurantId == obj.RestaurantId);

            if (userRestaurantPoints == null)
            {
                // If no points exist, create a new record for the user-restaurant pair
                userRestaurantPoints = new Points
                {
                    UserId = user.Id,
                    RestaurantId = obj.RestaurantId,
                    PointsForEachRestaurant = 5 
                };
                _db.Points.Add(userRestaurantPoints);
            }
            else
            {
                // If points already exist, update the points
                userRestaurantPoints.PointsForEachRestaurant += 5; 
            }
            await _db.SaveChangesAsync();

            var allRatings = await _db.Reviews
             .Where(r => r.RestaurantId == obj.RestaurantId)
             .Select(r => r.Rating)
             .ToListAsync();

            

            restaurant.Rating = allRatings.Average();

            _db.Restaurants.Update(restaurant);
            await _db.SaveChangesAsync();
            return Ok(obj);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteReview-ById ")]
        //[Authorize(Roles ="Admin")]
        public IActionResult DeleteReview(int Id)
        {

            if (Id==0)
            {
                return BadRequest();
            }
            var Review = _db.Reviews.FirstOrDefault(u => u.Id==Id);
            if (Review == null)
            {
                return NotFound();
            }
            _db.Reviews.Remove(Review);
            _db.SaveChanges();
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("UpadteReview-ById")]
        //[Authorize(Roles ="Admin")]
        public IActionResult UpdateReview(int Id, [FromBody] ReviewDto obj)
        {

            var Review = _db.Reviews.Find(Id);

            if (Review == null)
            {
                return NotFound();
            }

           Review.Comment = obj.Comment;

            _db.SaveChanges();
            return NoContent();
        }
        [HttpPost("LikeReview/{ReviewId}")]
        public IActionResult LikeReview (int ReviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userId);
            var existingLike = _db.Likes.FirstOrDefault((l => l.UserId == user.Id && l.ReviewId == ReviewId));
            var Review = _db.Reviews.FirstOrDefault(r => r.Id == ReviewId);
            if (existingLike == null)
            {
                var Like = new Like
                {
                    UserId = user.Id,
                    ReviewId = ReviewId,
                    CreatedAt = DateTime.UtcNow,
                };
                Review.Likes++;
                _db.Likes.Add(Like);
     
            }
            else
            {
                _db.Likes.Remove(existingLike);
                Review.Likes--;
            }
            _db.SaveChanges();
            return Ok(new { success = true, Review.Likes });
        } 
    }
}
