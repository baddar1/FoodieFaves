using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public NotificationController(ILogger<RestaurantController> logger, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitOfWork = unitOfWork;
        }
        

        [HttpGet("GetNotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            // Get the currently logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userId);
            // Retrieve all notifications for the user
            var notifications = await _db.Notifications
                .Where(n => n.UserId == user.Id)
                .OrderByDescending(n => n.CreatedAt) // Optional: Order by most recent
                .Select(n => new
                {
                    //n.Id,
                    n.Message,
                    n.CreatedAt,
                    //n.IsRead,
                    //n.ReviewId 
                })
                .ToListAsync();

            if (!notifications.Any())
            {
                return NotFound("No notifications found for the user.");
            }

            return Ok(notifications); // Return the notifications
        }

    }
}
