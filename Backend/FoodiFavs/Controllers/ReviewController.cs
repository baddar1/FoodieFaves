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
            return Ok(new
            {
                restaurant.Id,
                restaurant.Name,
            });
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
                    PointsForEachRestaurant = 5,
                };


                notification.Message = $"{user.UserName} Congratulations on posting your first review on {restaurant.Name} You've earned 5 points for your contribution!";
                notification.ReviewId=model.Id;
                notification.RestaurantId=model.RestaurantId;
                _db.Points.Add(userRestaurantPoints);
                await _db.SaveChangesAsync();
            }
            else
            {
                // If points already exist, update the points
                userRestaurantPoints.PointsForEachRestaurant += 5;

                notification.Message = $"{user.UserName} You've earned 5 points on {restaurant.Name} for your contribution y!";
                notification.ReviewId=model.Id;
                notification.RestaurantId=model.RestaurantId;
                //To find top review for each user
                
            }
            var existingTopReview = _db.TopReviewForUsers.FirstOrDefault(tr => tr.UserId == user.Id);
            if (existingTopReview!=null)
            {
                if (model.Rating>user.TopRateReview)
                {
                    existingTopReview.ReviewId = model.Id;
                    existingTopReview.RestaurantId = restaurant.Id;
                    existingTopReview.TopRate = model.Rating;

                    user.TopRateReview = model.Rating;
                    await _db.SaveChangesAsync();

                }
            }
            else
            {
                    TopReview.UserId=user.Id;
                    TopReview.ReviewId=model.Id;
                    user.TopRateReview=model.Rating;
                    TopReview.TopRate=model.Rating;
                    TopReview.RestaurantId=restaurant.Id;
                    _db.TopReviewForUsers.Add(TopReview);

            }
            
            userRestaurantPoints.AllPoints=user.TotalPoints;
            user.UnReadNotiNum = user.UnReadNotiNum ?? 0;
            user.UnReadNotiNum++;
            _db.Notifications.Add(notification);
            await _db.SaveChangesAsync();

            var allRatings = await _db.Reviews
             .Where(r => r.RestaurantId == obj.RestaurantId)
             .Select(r => r.Rating)
             .ToListAsync();

            restaurant.Rating = Math.Floor(allRatings.Average() * 10) / 10;

            await _db.SaveChangesAsync();

            //Find all the Users whose following the blogger 
            var followers = _db.FavoriteBloggers
               .Where(f => f.BloggerId == user.Id) //Make suer that we search in the same blogger
               .Select(f => f.User) //Get the followers Id
               .ToList();

            //Loob to notify to all Followers
            foreach (var follower in followers)
            {
                var notificationReview = new Notification
                {
                    UserId = follower.Id,
                    Message = $"your favorite blogger {user.UserName} , has written a new review for {restaurant.Name}!",
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    BloggertId= follower.Id,
                    ReviewId=model.Id,
                    RestaurantId=restaurant.Id,
                    NotificationType="Review",
                };
                follower.UnReadNotiNum = follower.UnReadNotiNum ?? 0;
                follower.UnReadNotiNum++;
                _db.Notifications.Add(notificationReview);
            }
            await _db.SaveChangesAsync();
            return Ok();
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteReview-ById/{Id}")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteReview(int Id)
        {
            if (Id == 0)
            {
                return BadRequest("Review ID cannot be zero.");
            }

            var review = _db.Reviews.FirstOrDefault(r => r.Id == Id);
            if (review == null)
            {
                return NotFound("Review not found.");
            }

            var userId = review.UserId;
            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            var points = _db.Points.FirstOrDefault(p => p.UserId == userId);
            var restaurant = _db.Restaurants.FirstOrDefault(r => r.Id == review.RestaurantId);
            var notifications = _db.Notifications.Where(n => n.ReviewId == review.Id).ToList();
            var order = _db.Orders.FirstOrDefault(o => o.ReviewId == review.Id);
            var likes = _db.Likes.Where(l => l.ReviewId == review.Id).ToList();

            if (order != null)
            {
                _db.Orders.Remove(order);
            }
            if (likes.Any())
            {
                _db.Likes.RemoveRange(likes);
            }
            if (notifications.Any())
            {
                _db.Notifications.RemoveRange(notifications);
            }

            var topReviews = _db.TopReviewForUsers.Where(tr => tr.ReviewId == Id).ToList();
            if (topReviews.Any())
            {
                _db.TopReviewForUsers.RemoveRange(topReviews);
            }

            _db.Reviews.Remove(review);
            _db.SaveChanges();

            //update restaurant review count and rating
            if (restaurant != null)
            {
                restaurant.ReviewCount = restaurant.ReviewCount > 0 ? restaurant.ReviewCount - 1 : 0;

                var allRatings = await _db.Reviews
                    .Where(r => r.RestaurantId == restaurant.Id)
                    .Select(r => r.Rating)
                    .ToListAsync();

                restaurant.Rating = allRatings.Any()
                    ? Math.Floor(allRatings.Average() * 10) / 10
                    : 0;
            }

            //update user points and likes
            if (points != null)
            {
                if (points.PointsForEachRestaurant > 0)
                {
                    points.PointsForEachRestaurant -= 5;
                }
                else
                {
                    points.PointsForEachRestaurant = 0;
                }

                if (points.AllPoints > 0)
                {
                    points.AllPoints -= 5;
                }
                else
                {
                    points.AllPoints = 0;
                }
            }

            if (user != null)
            {
                if (user.TotalLikes > 0)
                {
                    user.TotalLikes -= review.Likes;
                }
                else
                {
                    user.TotalLikes = 0;
                }

                if (user.TotalPoints > 0)
                {
                    user.TotalPoints -= 5;
                }
                else
                {
                    user.TotalPoints = 0;
                }

                if (user.ReviewCount > 0)
                {
                    user.ReviewCount--;
                }
                else
                {
                    user.ReviewCount = 0;
                }

                //update top review for the user
                var topRatedReview = _db.Reviews
                    .Where(r => r.UserId == userId)
                    .OrderByDescending(r => r.Rating)
                    .FirstOrDefault();

                var topRate = _db.TopReviewForUsers.FirstOrDefault(tr => tr.UserId == userId);
                if (topRatedReview != null)
                {
                    user.TopRateReview = topRatedReview.Rating;

                    if (topRate != null)
                    {
                        topRate.ReviewId = topRatedReview.Id;
                        topRate.TopRate = topRatedReview.Rating;
                        topRate.RestaurantId = topRatedReview.RestaurantId;
                    }
                    else
                    {
                        var newTopReview = new TopReviewForUser
                        {
                            UserId = user.Id,
                            ReviewId = topRatedReview.Id,
                            TopRate = topRatedReview.Rating,
                            RestaurantId = topRatedReview.RestaurantId,
                        };
                        _db.TopReviewForUsers.Add(newTopReview);
                    }
                }
                else if (topRate != null)
                {
                    _db.TopReviewForUsers.Remove(topRate);
                }
            }

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
        [HttpPost("LikeReview")]
        public IActionResult LikeReview(int ReviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userId);

            var Review = _db.Reviews.FirstOrDefault(r => r.Id == ReviewId);
            if (Review == null)
            {
                return BadRequest("No Review with this Id");
            }
            var existingLike = _db.Likes.FirstOrDefault((l => l.ReviewId == ReviewId && l.UserId==user.Id));
            var reviewer = _db.Users.FirstOrDefault(u => u.Id == Review.UserId);
            if (reviewer == null)
            {
                return BadRequest("Sign Up First");
            }
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
                        UserId = Review.UserId, //notify the Blogger
                        Message = $"{user.UserName} liked your review.",
                        CreatedAt = DateTime.UtcNow,
                        IsRead = false,
                        ReviewId=ReviewId,
                        NotificationType="Like",
                        RestaurantId=Review.RestaurantId

                    };
                    var Blogger = _db.Users.FirstOrDefault(u => u.Id==Review.UserId);
                    if (Blogger == null) 
                    {
                        return BadRequest("No User with this review");
                    }
                    Blogger.UnReadNotiNum++;
                    _db.Notifications.Add(notification);
                }

            }
            else
            {
                _db.Likes.Remove(existingLike);
                Review.Likes--;
                if (reviewer.UserName==user.UserName)
                {
                    user.TotalLikes--;
                }
                else
                {
                    reviewer.TotalLikes--;
                }
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
            }
            else
            {
                return Ok("The review is clean no comment detected.");
            }

            await _db.SaveChangesAsync();
            return Ok("Review reported successfully.");
        }
        [HttpPost("Sort-By-Likes")]
        public async Task<IActionResult> SortedReviewsByLikes(int restaurantId)
        {
            if (restaurantId <= 0)
            {
                return BadRequest("Invalid Restaurant ID");
            }

            var sortedReviews = _db.Restaurants
                 .Where(r => r.Id == restaurantId)
                 .Select(r => new
                 {
                     Reviews = r.ReviweNav.Select(review => new
                     {
                         review.Id,
                         review.Rating,
                         review.Comment,
                         review.CreatedAt,
                         UserName = review.UserNav.UserName,
                         UserId = review.UserNav.Id,
                         UserImg = review.UserNav.ImgUrl,
                         TotalLikes = review.Likes,

                     }).ToList()

                 })
                 .FirstOrDefault();

            if (sortedReviews==null)
            {
                return NotFound("No reviews found for the given restaurant.");
            }

            return Ok(sortedReviews);
        }

        [HttpGet("review-count")]
        public ActionResult<int> GetReviewesNumber()
        {
            var reviewNumber = _db.Reviews.Select(r => r.Id).Count();
            return reviewNumber;
        }
        [HttpGet("All-Reported-Review")]
        public async Task<IActionResult> ReportedReview()
        {
            var reportedReviews = _db.Reviews
                .Where(r => r.IsReported == true)
                .Select(r => new
                {
                    ReviewId = r.Id,
                    ReviewContent = r.Comment,
                    ReviewRate=r.Rating,
                    ReviewLikes=r.Likes,
                    ReviewDate=r.CreatedAt,
                    Reviewer = new
                    {
                        UserId = r.UserNav.Id,
                        UserName = r.UserNav.UserName,
                        UserEmail = r.UserNav.ImgUrl
                    },
                    Restaurant = new
                    {
                        RestaurantId = r.RestaurantId,
                        RestaurantNav = r.RestaurantNav.Name
                    }
                    
                })
                .ToList();

            if (!reportedReviews.Any())
            {
                return BadRequest("No reported reviews.");
            }

            return Ok(new
            {
                ReportedReviews = reportedReviews
            });
        }
    }
}
