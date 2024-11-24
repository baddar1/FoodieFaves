using FF.Models.Secuirty;
using FF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FF.Data.Repository.IRepository;
using FoodiFavs.Helper;
using Microsoft.EntityFrameworkCore;
using FF.Data.Access.Data;
using FF.Models.Dto;
using FF.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using FF.Models.Dto.UserDto;

namespace FF.Data.Repository
{
    public class AuthServices : IAuthService
    {
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        private readonly ApplicationDbContext _db;
        private readonly JWT _jwt;
        private readonly IEmailSender _emailSender;
        public AuthServices(IEmailSender emailSender, RoleManager<IdentityRole> roleManager, ApplicationDbContext db ,UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        {
            _emailSender = emailSender;
            _jwt = jwt.Value;
            UserManager = userManager;
            RoleManager = roleManager;
            _db = db;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel1 model)
        {
            if (await UserManager.FindByEmailAsync(model.Email) is not null)
            {
                return new AuthModel { Message = "Email is already registered" };
            }
            if (await UserManager.FindByNameAsync(model.UserName) is not null)
            {
                return new AuthModel { Message = "User name is already registered" };
            }
            var confirmationCode = Guid.NewGuid().ToString().ToUpper().Substring(0, 6); // Generate a unique confirmation code
            var pendingUser = new PendingUser
            {
                Password = model.Password,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                Email = model.Email,
                ConfirmationCode = confirmationCode,
                CreatedAt = DateTime.UtcNow
            };

            //var applicationUser = new ApplicationUser
            //{
            //    UserName = model.UserName,
            //    Email = model.Email,
            //    PhoneNumber = model.PhoneNumber,
            //};
            _db.PendingUsers.Add(pendingUser);
            await _db.SaveChangesAsync();

            var callbackUrl = $"https://localhost:7063/api/auth/confirm-email?code={confirmationCode}"; // Update with your actual URL
            await _emailSender.SendEmailAsync(model.Email, "Confirm your identity",
                $"Please confirm your identity by clicking here: <a href='{callbackUrl}'>link</a>");

            return new AuthModel { Message = "Please check your email to confirm your identity." };

            //var result = await UserManager.CreateAsync(applicationUser, model.Password);
            
            //if (!result.Succeeded)
            //{
            //    string errors = string.Empty;
            //    foreach (var error in result.Errors)
            //    {
            //        errors += $"{error.Description} ,";
            //    }
            //    return new AuthModel { Message = errors };
            //}
            //if (result.Succeeded)
            //{
            //    // User creation was successful
            //    User user = new User
            //    {
            //        UserName = applicationUser.UserName,
            //        Email = applicationUser.Email,
            //        Password = applicationUser.PasswordHash,
            //        PhoneNumber = applicationUser.PhoneNumber,
            //        Id = applicationUser.Id,
            //        Points = 0
            //    };

            //    _db.Users.Add(user);
            //    await _db.SaveChangesAsync();

            //}

            //    await UserManager.AddToRoleAsync(applicationUser, "User");
            //var JwtSecurityToken = await CreateJwtToken(applicationUser);
            //return new AuthModel
            //{
            //    Email = applicationUser.Email,
            //    ExpiresOn = JwtSecurityToken.ValidTo,
            //    IsAuthenticated = true,
            //    Roles = new List<string> { "User" },
            //    Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
            //    UserName = applicationUser.UserName,
            //    PhoneNumber = applicationUser.PhoneNumber
            //};
        }
        public async Task<AuthModel> ConfirmEmailAsync(string code)
        {
            if (code == null)
            {
                return new AuthModel {  Message = "Invalid confirmation code." };
            }
            var pendingUser = await _db.PendingUsers
            .FirstOrDefaultAsync(u => u.ConfirmationCode == code && !u.IsConfirmed);

            if (pendingUser == null)
            {
                return new AuthModel { Message = "Pending user not found or already confirmed." };
            }
            var applicationUser = new ApplicationUser
            {
                UserName = pendingUser.UserName,
                Email = pendingUser.Email,
                PhoneNumber = pendingUser.PhoneNumber,
            };
            var result = await UserManager.CreateAsync(applicationUser, pendingUser.Password);
            if (!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} ,";
                }
                return new AuthModel { Message = errors };
            }
            if (result.Succeeded)
            {
                pendingUser.IsConfirmed = true;
                User user = new User
                {
                    UserName = applicationUser.UserName,
                    Email = applicationUser.Email,
                    Password = applicationUser.PasswordHash,
                    PhoneNumber = applicationUser.PhoneNumber,
                    Id = applicationUser.Id,
                   
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                // User creation was successful
            }

            await UserManager.AddToRoleAsync(applicationUser, "User");
            var JwtSecurityToken = await CreateJwtToken(applicationUser);
            return new AuthModel
            {
                Message = "Your identity has been confirmed, and your account has been created.",
                Email = applicationUser.Email,
                ExpiresOn = JwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
                UserName = applicationUser.UserName,
                PhoneNumber = applicationUser.PhoneNumber
            };

        }
        public async Task<AuthModel> GetTokenAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null || !await UserManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Passwor is incorrect";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await UserManager.GetRolesAsync(user);
            
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.PhoneNumber = user.PhoneNumber;

            
            authModel.Roles = rolesList.ToList();
            return authModel;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await UserManager.GetClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await UserManager.FindByIdAsync(model.UserId);
            // اليوزر موجود بالداتابيس و الرول موجود بالداتا بيس
            if (user == null || !await RoleManager.RoleExistsAsync(model.Role))
            {
                return ("Invalid user ID or Role");
            }
            // اذا اليوزر موجود فعلا بهاي الرول 
            if (await UserManager.IsInRoleAsync(user, model.Role))
            {
                return ("User already assigned to this role");
            }
            var result = await UserManager.AddToRoleAsync(user, model.Role);

            if (result.Succeeded)
            {
                return string.Empty;
            }
            return "Somehhing went wrong";

        }
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await UserManager.FindByIdAsync(id);
        }
        public async Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            return await UserManager.FindByNameAsync(userName);
        }
        public async Task<IdentityResult> UpdateUserAsync(UserUpdateDto model , ClaimsPrincipal userClaims)
        {

            // Retrieve the user ID from the claims
            string userId = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                // Handle case where user ID is not found in claims
                return IdentityResult.Failed(new IdentityError { Description = "User not authenticated." });
            }
            var user = await GetUserByNameAsync(userId);
            User dbUser = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userId); 
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }
            if (dbUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User DB not found." });
            }


            // Update user properties
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.NormalizedEmail = (model.Email).ToUpper();
            user.NormalizedUserName = (model.UserName).ToUpper();
            
           
            // Attempt to update the user in the database
            return await UserManager.UpdateAsync(user);
        }
        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null) return null;

            // Generate the reset token
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }
        public async Task SendResetPasswordEmailAsync(string email, string token)
        {
            var callbackUrl = $"https://yourapp.com/reset-password?token={WebUtility.UrlEncode(token)}&email={WebUtility.UrlEncode(email)}";
            var message = $"Reset your password by clicking here: <a href='{callbackUrl}'>link</a>";

            await _emailSender.SendEmailAsync(email, "Reset Password", message);
        }
        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            return await UserManager.ResetPasswordAsync(user,model.Token , model.NewPassword);
        }
    }
}
