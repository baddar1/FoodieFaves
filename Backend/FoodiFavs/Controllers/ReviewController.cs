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
        [HttpPost("ValidateReviewCode")]
        [Authorize]
        public IActionResult ValidateReviewCode([FromBody] string reviewCode)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);

            if (string.IsNullOrWhiteSpace(reviewCode))
            {
                return BadRequest("Review code is required.");
            }
            
            var order = _db.Orders.FirstOrDefault(o => o.ReviewCode == reviewCode);
            if (order == null)
            {
                return NotFound("Invalid review code.");
            }
            var restaurant = _db.Restaurants.FirstOrDefault(r => r.Id==order.RestaurantId);
            if (restaurant == null)
            {
                return NotFound("No restaurant with this review code.");
            }
            var CodeUsed = _db.Orders.Any(o => o.Id==order.Id && o.IsUsed==true);
            if (CodeUsed)
            {
                return BadRequest("This review code has already been used.");
            }
            order.UserId=user.Id;
            _db.SaveChanges();
            return Ok($"Id = {restaurant.Id} Rsstaurant Name = {restaurant.Name}");
        }
        [HttpPost("Add-Review")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<ActionResult> AddReview([FromBody] ReviewDto obj)
        {
            if (obj == null)
            {
                return BadRequest("Bad input");
            }

            var restaurant = await _db.Restaurants.FirstOrDefaultAsync(r => r.Id == obj.RestaurantId);
            
            if (restaurant == null) 
            {
                return NotFound("Restaurant not found.");
            }
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            Review model = new()
            {
                
                Rating=obj.Rating,
                Comment=obj.Comment,
                UserId=user.Id,
                RestaurantId=obj.RestaurantId,
            };

            
            _db.Reviews.Add(model);
            await _db.SaveChangesAsync();

            var order = _db.Orders.FirstOrDefault(o => o.UserId == user.Id && o.RestaurantId == restaurant.Id && o.IsUsed == false);

            if (order!=null)
            {
                order.ReviewId=model.Id;
                order.IsUsed=true;
                model.OrderId=order.Id;

                //To count reviews number for each user
                user.ReviewCount++;
                user.TotalPoints+=5;
                restaurant.ReviewCount++;

                await _db.SaveChangesAsync();
            }
            else
            {
                _db.Reviews.Remove(model);
                await _db.SaveChangesAsync();
                return NotFound("The review code Is already used");
            }
            

            var TopReview = new TopReviewForUser();

            var userRestaurantPoints = await _db.Points
            .FirstOrDefaultAsync(p => p.UserId == user.Id && p.RestaurantId == obj.RestaurantId);
           
            var notification = new Notification
            {
                UserId = user.Id,
                Message ="",
                CreatedAt = DateTime.Now,
                IsRead = false,
                NotificationType="Points"

            };
            
            await _db.SaveChangesAsync();

            if (userRestaurantPoints == null)
            {
                // If no points exist, create a new record for the user-restaurant pair
                userRestaurantPoints = new Points
                {
                    UserId = user.Id,
                    RestaurantId = obj.RestaurantId,
                    PointsForEachRestaurant = 500,
                };

                TopReview.UserId=user.Id;
                TopReview.ReviewId=model.Id;
                user.TopRateReview=model.Rating;
                TopReview.TopRate=model.Rating;
                TopReview.RestaurantId=restaurant.Id;
                
                notification.Message = $"{user.UserName} Congratulations on posting your first review on {restaurant.Name} You've earned 5 points for your contribution!";
                notification.ReviewId=model.Id;
                notification.RestaurantId=model.RestaurantId;
                _db.TopReviewForUsers.Add(TopReview);
                _db.Points.Add(userRestaurantPoints);
                await _db.SaveChangesAsync();
            }
            else
            {
                // If points already exist, update the points
                userRestaurantPoints.PointsForEachRestaurant += 500;
                
                notification.Message = $"{user.UserName} You've earned 5 points for your contribution y!";
                notification.ReviewId=model.Id;
                notification.RestaurantId=model.RestaurantId;
                //To find top review for each user
                var existingTopReview = _db.TopReviewForUsers.FirstOrDefault(tr => tr.UserId == user.Id);

                if (model.Rating>user.TopRateReview) 
                {
                    existingTopReview.ReviewId = model.Id;
                    existingTopReview.RestaurantId = restaurant.Id;
                    existingTopReview.TopRate = model.Rating;

                    user.TopRateReview = model.Rating;
                    await _db.SaveChangesAsync();

                }
            }
            userRestaurantPoints.AllPoints=user.TotalPoints;
            
            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();
 
            var allRatings = await _db.Reviews
             .Where(r => r.RestaurantId == obj.RestaurantId)
             .Select(r => r.Rating)
             .ToListAsync();

            restaurant.Rating = allRatings.Average();
            await _db.SaveChangesAsync();

            //Find all the Users whose following the blogger 
            var followers = _db.FavoriteBloggers
               .Where(f => f.BloggerId == user.Id) //Make suer that we search in the same blogger
               .Select(f => f.UserId) //Get the followers Id
               .ToList();

            //Loob to notify to all Followers
            foreach (var followerId in followers)
            {
                var notificationReview = new Notification
                {
                    UserId = followerId, 
                    Message = $"{user.UserName}, your favorite blogger, has written a new review for {restaurant.Name}!",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    ReviewId=model.Id,
                    RestaurantId=restaurant.Id,
                    NotificationType="Review"
                };
                _db.Notifications.Add(notificationReview);
            }
            await _db.SaveChangesAsync();
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteReview-ById ")]
        [Authorize(Roles ="Admin")]
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
            Review.UserNav.ReviewCount--;
            _db.Reviews.Remove(Review);
            _db.SaveChanges();
            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("UpadteReview-ById")]
        [Authorize(Roles ="Admin")]
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
        [HttpPost("LikeReview")]
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
            var reviewer = _db.Users.FirstOrDefault(u => u.Id == Review.UserId);
            if (existingLike == null)
            {
                var Like = new Like
                {
                    UserId = user.Id,
                    ReviewId = ReviewId,
                    CreatedAt = DateTime.UtcNow,
                };

                Review.Likes++;

               
                if (reviewer != null)
                {
                    reviewer.TotalLikes++;
                }

                _db.Likes.Add(Like);
                if (Review.UserId != user.Id) 
                {
                    var notification = new Notification
                    {
                        UserId = Review.UserId, // Notify the Blogger
                        Message = $"{user.UserName} liked your review.",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false,
                        ReviewId=ReviewId,
                        NotificationType="Like"
                    };
                    _db.Notifications.Add(notification);
                }

            }
            else
            {
                _db.Likes.Remove(existingLike);
                Review.Likes--;
                reviewer.TotalLikes--;
                user.TotalLikes--;
                var notification = _db.Notifications
                .FirstOrDefault(n => n.UserId == Review.UserId
                 && n.Message == $"{user.UserName} liked your review.");
                if (notification != null)
                {
                    _db.Notifications.Remove(notification);
                }
            }
            _db.SaveChanges();
            return Ok(new { success = true, Review.Likes });
        }
        [HttpPost("Report-Review")]
        public async Task<IActionResult> ReportReview(int reviewId)
        {
            var review = await _db.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
            if (review == null)
            {
                return NotFound("Review not found.");
            }

            if (review.Comment != null)
            {

                review.IsReported = true;

                // notify the admin for reported review
                var notification = new Notification
                {
                    UserId = "1",
                    Message = $"Review by {review.UserId} has been reported for offensive content.",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    ReviewId=review.Id,
                    NotificationType="Report"
                };
                _db.Notifications.Add(notification);
            }
            else
            {
                return Ok("The review is clean no comment detected.");
            }

            await _db.SaveChangesAsync();
            return Ok("Review reported successfully.");
        }
        [HttpPost("Sort-By-Likes")]
        public async Task<IActionResult> SorteReviewsdByLikes(int restaurantId)
        {
            if (restaurantId == 0)
            {
                return BadRequest("Invalid Restaurant ID");
            }

            // Get all reviews for the restaurant
            var reviews = await _db.Reviews
                .Where(r => r.RestaurantId == restaurantId)
                .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                return NotFound("No reviews found for the given restaurant.");
            }

            
            var sortedReviews = reviews
                .OrderByDescending(r => r.Likes) 
                .Select(r => new
                {
                    r.Id,
                    r.Comment,
                    r.Rating,
                    r.UserId,
                    r.RestaurantId,
                    r.Likes,  
                    r.CreatedAt
                })
                .ToList();

            return Ok(sortedReviews);
        }



    }
}
