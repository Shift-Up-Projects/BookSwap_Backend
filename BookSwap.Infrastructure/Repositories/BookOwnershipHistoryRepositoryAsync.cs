using BookSwap.Core.Entities;
using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using BookSwap.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Infrastructure.Repositories
{
    public class BookOwnershipHistoryRepositoryAsync : GenericRepositoryAsync<BookOwnershipHistory>,IBookOwnershipHistoryRepositoryAsync
    {
        public BookOwnershipHistoryRepositoryAsync(BookSwapDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
