using BookSwap.Core.Results;

namespace BookSwap.Application.Abstracts
{
    public interface IEmailService 
    {
        Task<bool> SendEmailAsync(string toEmail, string message, string? reason = null);
        Task<bool> SendEmailConfirmationAsync(string email, string userId, string confirmationCode);
        Task<bool> SendPasswordResetEmailAsync(string email, string resetCode);
    }
}
