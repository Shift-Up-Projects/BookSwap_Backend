using BookSwap.Core.Entities.Identity;
using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using BookSwap.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Infrastructure.Repositories
{
    public class RefreshTokenRepositoryAsync : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepositoryAsync
    {
        private readonly BookSwapDbContext _context;
        public RefreshTokenRepositoryAsync(BookSwapDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
        public async Task<UserRefreshToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.UserRefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);
        }

        public async Task<UserRefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.UserRefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<List<UserRefreshToken>> GetByUserIdAsync(int userId)
        {
            return await _context.UserRefreshTokens
                .Where(rt => rt.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken, int userId)
        {
            var token = await GetByRefreshTokenAsync(refreshToken);
            return token != null &&
                   token.UserId == userId &&
                   token.IsActive() &&
                   !token.IsRevoked;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await GetByRefreshTokenAsync(refreshToken);
            if (token != null)
            {
                token.Revoke();
                await UpdateAsync(token);
            }
        }

        public async Task RevokeAllRefreshTokensAsync(int userId)
        {
            var tokens = await GetByUserIdAsync(userId);
            foreach (var token in tokens.Where(t => t.IsActive()))
            {
                token.Revoke();
            }
            await UpdateRangeAsync(tokens);
        }

        public async Task CleanExpiredRefreshTokensAsync()
        {
            var expiredTokens = await _context.UserRefreshTokens
                .Where(rt => rt.ExpiryDate < DateTime.UtcNow || rt.IsRevoked)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.UserRefreshTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }
        }
    
}
}
