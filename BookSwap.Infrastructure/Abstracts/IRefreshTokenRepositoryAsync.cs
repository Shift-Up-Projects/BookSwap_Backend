using BookSwap.Core.Entities.Identity;
using BookSwap.Infrastructure.InfrastructureBases;

namespace BookSwap.Infrastructure.Abstracts
{
    public interface IRefreshTokenRepositoryAsync : IGenericRepositoryAsync<UserRefreshToken>
    {
        Task<UserRefreshToken?> GetByRefreshTokenAsync(string refreshToken);
        Task<UserRefreshToken?> GetByTokenAsync(string token);
        Task<List<UserRefreshToken>> GetByUserIdAsync(int userId);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken, int userId);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllRefreshTokensAsync(int userId);
        Task CleanExpiredRefreshTokensAsync();
    }
}
