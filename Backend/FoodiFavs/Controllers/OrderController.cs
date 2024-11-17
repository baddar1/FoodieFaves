using FF.Data.Access.Data;
using FF.Data.Access.Repository.IRepository;
using FF.Models;
using FF.Models.Dto.OrderDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RestaurantController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(ILogger<RestaurantController> logger, ApplicationDbContext db, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _db = db;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("Get-Order-Info")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            Order order = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                return Ok(order);
            }
            return BadRequest();
        }
        [HttpPost("New-Order")]
        public async Task<IActionResult> NewOrder([FromBody] string RestaurantName)
        {
            if (RestaurantName == null)
            {
                return BadRequest();
            }
            Restaurant Restaurant = _db.Restaurants.FirstOrDefault(r => r.Name == RestaurantName);
            int RestaurantId = Restaurant.Id;
            Order order = new()
            {
                ReviewCode = Guid.NewGuid().ToString().ToUpper().Substring(0, 6),
                RestaurantId = RestaurantId,
                RestaurantName = RestaurantName,
            };
            
            _db.Orders.Add(order);
            _db.SaveChanges();
            
            return RedirectToAction("GetCode",new { OrderId = order.Id});
        }
        [HttpGet("Code")]
        public async Task<IActionResult> GetCode(int OrderId)
        {
            Order order = _db.Orders.FirstOrDefault(o => o.Id == OrderId);
            var Code = order.ReviewCode;
            // string code = Code.ToString().ToUpper().Substring(0, 8);
            if (order == null)
            {
                return BadRequest("Invalid Order");
            }
            return Ok(Code);
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteOrder-ById ")]
        //[Authorize(Roles ="Admin")]
        public IActionResult DeleteOrder(int Id)
        {

            if (Id == 0)
            {
                return BadRequest();
            }
            var Order = _db.Orders.FirstOrDefault(o => o.Id == Id);
            if (Order == null)
            {
                return NotFound();
            }
            _db.Orders.Remove(Order);
            _db.SaveChanges();
            return NoContent();
        }

    }
}
