using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using FF.Models.Dto.RestaurantDto;

namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<ReviewController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public VoucherController(ILogger<ReviewController> logger, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("BuyVoucher")]
        public IActionResult BuyVoucher(int points, int restaurantId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var restaurant = _db.Restaurants.FirstOrDefault(r => r.Id==restaurantId);
            if (restaurant == null) 
            {
                return BadRequest($"{restaurant.Name} is out of service");
            }
            var RestaurantPoints = _db.Points.FirstOrDefault(p => p.UserId==user.Id && p.RestaurantId==restaurantId);
            if (RestaurantPoints==null)
            {
                return BadRequest($"Sorry {user.UserName} try to Review restaurants to get points");
            }
            var guid = Guid.NewGuid().ToString("N").Substring(0, 12);

            var formattedCode = $"{guid.Substring(0, 4)}-{guid.Substring(4, 4)}-{guid.Substring(8, 4)}";
            Vouchers SetVoucher;
            if (points == 50)
            {
                if (RestaurantPoints.PointsForEachRestaurant>=points)
                {
                    return BadRequest($"Sorry {user.UserName} you can't afford this voucher");
                }
                SetVoucher = new Vouchers
                {
                    voucherType = "1 Jd",
                    UserId = user.Id,
                    RestaurantId = restaurantId,
                    CreatedAt = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddDays(30),
                    voucherCode = formattedCode
                };
                
            }
            else if (points == 100)
            {
                if (RestaurantPoints.PointsForEachRestaurant>=points)
                {
                    return BadRequest($"Sorry {user.UserName} you can't afford this voucher");
                }
                SetVoucher = new Vouchers
                {
                    voucherType = "2.5 JD",
                    UserId = user.Id,
                    RestaurantId = restaurantId,
                    CreatedAt = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddDays(30),
                    voucherCode = formattedCode
                };
            }
            else if (points == 200)
            {
                if (RestaurantPoints.PointsForEachRestaurant>=points)
                {
                    return BadRequest($"Sorry {user.UserName} you can't afford this voucher");
                }
                SetVoucher = new Vouchers
                {
                    voucherType = "6 JD",
                    UserId = user.Id,
                    RestaurantId = restaurantId,
                    CreatedAt = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddDays(30),
                    voucherCode = formattedCode
                };
            }
            else
            {
                return BadRequest("Please select a Voucher");
            }
            var notification = new Notification
            {
                UserId = user.Id, // Notify the Blogger
                Message = $"{user.UserName}, Enjoy your {SetVoucher.voucherType} off for your next order from {restaurant.Name} .",
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                RestaurantId = restaurantId,
                NotificationType="Voucher",
            };
            user.UnReadNotiNum++;
            _db.Notifications.Add(notification);
            _db.Vouchers.Add(SetVoucher);
            _db.SaveChanges();
            user.TotalPoints-=points;
            RestaurantPoints.AllPoints-=points;
            RestaurantPoints.PointsForEachRestaurant-=points;
            _db.SaveChanges();
            return Ok($"{SetVoucher.voucherCode}");

        }
        [HttpPost("Use-Voucher")]
        public async Task<IActionResult> UseVoucher(string voucherCode)
        {
            // Find the voucher by code
            var voucher = await _db.Vouchers
                .FirstOrDefaultAsync(v => v.voucherCode == voucherCode);

            if (voucher == null)
            {
                return BadRequest("Invalid voucher code.");
            }

            // Check if the voucher is expired
            if (voucher.ExpirationDate < DateTime.UtcNow)
            {
                return BadRequest("This voucher has expired.");
            }

            // Check if the voucher has already been used
            if (voucher.IsUsed)
            {
                return BadRequest("This voucher has already been used.");
            }

            // Apply the discount logic here (e.g., deduct from total price)
            voucher.IsUsed = true;  // Mark voucher as used
            _db.Vouchers.Update(voucher);
            await _db.SaveChangesAsync();

            return Ok("Voucher applied successfully.");
        }
        [HttpGet("Get-Vouchers")]
        public async Task<IActionResult> GetVouchers()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // User is not logged in
            }
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);

            var userVouchers = _db.Vouchers.Where(v => v.UserId == user.Id).ToList();

            //Filter expired vouchers
            var expiredVouchers = userVouchers.Where(v => v.ExpirationDate < DateTime.UtcNow).ToList();

            if (expiredVouchers.Any())
            {
                //Delete expired vouchers
                _db.Vouchers.RemoveRange(expiredVouchers);
                await _db.SaveChangesAsync();
            }

            //Return vouchers
            var activeVouchers = userVouchers.Where(v => v.ExpirationDate >= DateTime.UtcNow).ToList();

            return Ok(activeVouchers);
        }
        [HttpGet("Get-ResturantsAndPoints")]
        public IActionResult RestaurantsAndPoints()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // User is not logged in
            }

            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Fetch all points with associated restaurants for the user
            var restaurantsPoints = _db.Points
                .Include(p => p.Restaurant)
                .Where(p => p.UserId == user.Id)
                .Select(p => new RestaurantPointsDto
                {
                    RestaurantName = p.Restaurant.Name,
                    RestaurantId=p.RestaurantId,
                    Points = p.PointsForEachRestaurant
                })
                .ToList();

            if (!restaurantsPoints.Any())
            {
                return BadRequest("Try to review restaurants first.");
            }

            return Ok(new
            {

                Restaurants = restaurantsPoints

            });
        }







    }
}
