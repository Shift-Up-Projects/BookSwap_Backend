using Microsoft.AspNetCore.Http;
namespace BookSwap.Application.Abstracts
{
    public interface IMediaService
    {
        Task<string> UploadMediaAsync(string Location,IFormFile file);
        Task<bool> DeleteMediaAsync(string imageUrl);
    }
}
