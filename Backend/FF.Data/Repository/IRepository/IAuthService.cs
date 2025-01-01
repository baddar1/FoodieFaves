using FF.Models.Secuirty;
using FF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


using FF.Models.Dto;
using System.Security.Claims;
using FF.Models.Dto.UserDto;
namespace FF.Data.Repository.IRepository
{
    public interface IAuthService
    {
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task<AuthModel> ConfirmEmailAsync(string code);
        Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto model);
        Task<IdentityResult> UpdateUserAsync(UserUpdateDto user, ClaimsPrincipal userClaims);
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<AuthModel> RegisterAsync(RegisterModel1 model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<string> AddAdmin(ApplicationUser user);

    }
}
