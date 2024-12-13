using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;

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
        public IActionResult BuyVoucher(int points,int restaurantId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Users.FirstOrDefault(u => u.UserName == userName);
            if (user == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
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
                if (RestaurantPoints.PointsForEachRestaurant<points)
                {
                    return BadRequest($"Sorry {user.UserName} you can't afford this voucher");
                }
                SetVoucher = new Vouchers
                {
                    voucherType = "0.15 off",
                    UserId = user.Id,
                    RestaurantId = restaurantId,
                    CreatedAt = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddDays(30),
                    voucherCode = formattedCode
                };
            }
            else if (points == 100)
            {
                if (RestaurantPoints.PointsForEachRestaurant<points)
                {
                    return BadRequest($"Sorry {user.UserName} you can't afford this voucher");
                }
                SetVoucher = new Vouchers
                {
                    voucherType = "2 JD",
                    UserId = user.Id,
                    RestaurantId = restaurantId,
                    CreatedAt = DateTime.UtcNow,
                    ExpirationDate = DateTime.UtcNow.AddDays(30),
                    voucherCode = formattedCode
                };
            }
            else if (points == 200)
            {
                if (RestaurantPoints.PointsForEachRestaurant<points)
                {
                    return BadRequest($"Sorry {user.UserName} you can't afford this voucher");
                }
                SetVoucher = new Vouchers
                {
                    voucherType = "5 JD",
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
            _db.vouchers.Add(SetVoucher);
            _db.SaveChanges();
            user.TotalPoints-=points;
            RestaurantPoints.AllPoints-=points;
            RestaurantPoints.PointsForEachRestaurant-=points;
            _db.SaveChanges();
            return Ok($"{SetVoucher.voucherCode}");

        }

    }
}
