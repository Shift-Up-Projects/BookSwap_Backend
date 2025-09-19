namespace BookSwap.Application.Abstracts
{
    public interface IEmailService 
    {
        public Task<string> SendEmailAsync(string email, string message, string? reason = null);

    }
}
