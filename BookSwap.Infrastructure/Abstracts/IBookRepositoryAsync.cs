using BookSwap.Core.Entities;
using BookSwap.Infrastructure.InfrastructureBases;

namespace BookSwap.Infrastructure.Abstracts
{
    public interface IBookRepositoryAsync : IGenericRepositoryAsync<Book>
    {
        Task<IEnumerable<Book>> GetPendingApprovalBooksAsync();
        Task<IEnumerable<Book>> GetApprovedBooksAsync();
        Task<IEnumerable<Book>> GetBooksByOwnerAsync(int ownerId);
        Task<IEnumerable<Book>> SearchBooksAsync(string? searchTerm);
        Task<IEnumerable<Book>> GetRejectedBooksAsync(); // للإدارة
        Task<IEnumerable<Book>> GetRejectedBooksByOwnerAsync(int ownerId); // للمستخدم
        Task<List<Book>> GetBooksByIdsAsync(IEnumerable<int> bookIds);
    }
}
