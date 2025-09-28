
using BookSwap.Application.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BookSwap.Core.Helping;
using BookSwap.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
namespace BookSwapImplementations
{
    public class CurrentUserService : ICurrentUserService
    {

        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        #endregion
        #region Constructors
        public CurrentUserService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        #endregion
        #region Functions
        public int GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(nameof(UserClaimModel.Id));
            if (userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            return int.Parse(userId);
        }

        public async Task<User> GetUserAsync()
        {
            var userId = GetUserId();
            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            { throw new UnauthorizedAccessException(); }
            return user;
        }

        public async Task<List<string>> GetCurrentUserRolesAsync()
        {
            var user = await GetUserAsync();
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }
        #endregion
    }
}
