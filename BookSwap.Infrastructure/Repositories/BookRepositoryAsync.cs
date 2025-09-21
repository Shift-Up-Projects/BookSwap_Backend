using BookSwap.Core.Entities;
using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using BookSwap.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace BookSwap.Infrastructure.Repositories
{
    public class BookRepositoryAsync : GenericRepositoryAsync<Book>, IBookRepositoryAsync
    {
        public BookRepositoryAsync(BookSwapDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Book>> GetPendingApprovalBooksAsync()
        {
            return await GetTableNoTracking()
                .Where(b => !b.IsApproved)
                .Include(b => b.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetApprovedBooksAsync()
        {
            return await GetTableNoTracking()
                .Where(b => b.IsApproved && b.IsAvailable)
                .Include(b => b.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByOwnerAsync(int ownerId)
        {
            return await GetTableNoTracking()
                .Where(b => b.OwnerId == ownerId)
                .Include(b => b.Owner)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> SearchBooksAsync(string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetApprovedBooksAsync();

            return await GetTableNoTracking()
                .Where(b => b.IsApproved && b.IsAvailable &&
                            (b.Title.Contains(searchTerm) ||
                             b.Author.Contains(searchTerm) ||
                             b.ISBN == null ? false : b.ISBN.Contains(searchTerm)))
                .Include(b => b.Owner)
                .ToListAsync();
        }
    }
}
