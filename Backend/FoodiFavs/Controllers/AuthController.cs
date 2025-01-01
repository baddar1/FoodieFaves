using FF.Data.Repository.IRepository;
using FF.Models.Secuirty;
using FF.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FF.Data.Access.Data;
using Microsoft.AspNetCore.Identity;
using FF.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FF.Data.Access.Repository.IRepository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Net;
using FF.Models.Dto.UserDto;
using FF.Models.Dto.RestaurantDto;
using Microsoft.AspNetCore.Authentication.Google;

namespace FoodiFavs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _db;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger, IEmailSender emailSender, IUserRepository userRepository, IAuthService authService, ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _emailSender = emailSender;
            _userRepository = userRepository;
            _userManager = userManager;
            _db = db;
            _authService = authService;
        }

        private string DecodeToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return "Invalid token.";
            }

            // Base64 decode the token
            var tokenBytes = Convert.FromBase64String(token);
            var decodedToken = Encoding.UTF8.GetString(tokenBytes);
            return decodedToken;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel1 registerModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.RegisterAsync(registerModel);

            if (result.Message != "Please check your email to confirm your identity.")
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return BadRequest("Invalid email confirmation request.");
            }

            var result = await _authService.ConfirmEmailAsync(code);
            if (result.IsAuthenticated == false)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequestModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.GetTokenAsync(model);
            if (result.IsAuthenticated == false)
            {
                return BadRequest(result.Message);
            }


            return Ok(result);
        }
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.AddRoleAsync(model);

            if (!String.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }

            return Ok(model);
        }
        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Log the userId to see if it's being retrieved correctly
            Console.WriteLine($"UserId from claims: {userId}");
            User dbUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userId);

            var result = await _authService.UpdateUserAsync(model, User);
            if (result.Succeeded)
            {
                dbUser.Email = model.Email;
                dbUser.UserName = model.UserName;
                dbUser.PhoneNumber = model.PhoneNumber;
                _db.SaveChanges();
                return Ok("User updated successfully.");
            }

            return BadRequest(result.Errors); // Return error messages if the update failed
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("Info")]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Log the ID for debugging
            Console.WriteLine($"User ID from claims: {userId}");

            var user = await _userManager.FindByNameAsync(userId);
            
            if (user == null)
            {
                return NotFound("User not found.");
            }
            //Adding Points to each restaurant
            var user1=_db.Users.FirstOrDefault(u => u.UserName == userId);
            var userInfo = new UserInfoDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UnReadNotiNum=user1.UnReadNotiNum,
                UserPoints=user1.TotalPoints,

            };

            return Ok(userInfo);
        }
       
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] RequestPasswordResetDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return BadRequest("Email address cannot be null or empty.");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token); // URL encode the token
            // Console.WriteLine($"Received token: {token}");
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Failed to generate password reset token.");
            }
            var callbackUrl = Url.Action("ResetPassword", "Auth", new { encodedToken, email = model.Email }, Request.Scheme);

            // Send email with the callback URL
            await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                $"Reset your password by this code: {encodedToken}");

            return Ok("Password reset code has been sent to your email.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Token))
            {
                return BadRequest("Token is required.");
            }
            //var decodedToken = DecodeToken(token);
            //_logger.LogInformation("Decoded token: {DecodedToken}", decodedToken);
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest("Invalid data.");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var decodedToken = WebUtility.UrlDecode(model.Token);
            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

            if (result.Succeeded)
            {
                User userDb = _db.Users.FirstOrDefault(u => u.Email == model.Email);
                userDb.Password = model.NewPassword;
                return Ok("Password reset successful.");
            }

            return BadRequest(result.Errors);
        }
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // This clears the authentication cookie.

            return Ok(new { message = "Logged out successfully." });
        }
        [HttpPost("Add-Admin")]
        [Authorize]
        public async Task<IActionResult> AddAdminAsync()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName == null)
            {
                return Unauthorized(); // Return 401 if user is not logged in
            }
            var user = await _userManager.FindByNameAsync(userName);
            var result = await _authService.AddAdmin(user);

            if (!String.IsNullOrEmpty(result))
            {
                return BadRequest(result);
            }
            return Ok($"User {user.UserName} has been added to the Admin role successfully.");

        }

    }
}
