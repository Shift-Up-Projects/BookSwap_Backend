using BookSwap.Core.Entities;
using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using BookSwap.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Infrastructure.Repositories
{
    public class OfferedBookRepositoryAsync : GenericRepositoryAsync<OfferedBook>,IOfferedBookRepositoryAsync
    {
        public OfferedBookRepositoryAsync(BookSwapDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<OfferedBook>> GetOfferedBooksByExchangeOfferIdAsync(int exchangeOfferId)
        {
            return await GetTableNoTracking()
          .Include(e => e.Book).Where(e => e.ExchangeOfferId == exchangeOfferId)
           .ToListAsync();
         
          
       
          

        }

        public async Task<IEnumerable<OfferedBook>> GetOfferedBooksWithDetailsByExchangeOfferIdAsync(int exchangeOfferId)
        {
            return await GetTableNoTracking()
        .Include(e => e.Book).ThenInclude(e => e.Category).Include(e => e.Book).ThenInclude(e => e.Owner)

        .Where(e => e.ExchangeOfferId == exchangeOfferId)
         .ToListAsync();

        }
    }
}
