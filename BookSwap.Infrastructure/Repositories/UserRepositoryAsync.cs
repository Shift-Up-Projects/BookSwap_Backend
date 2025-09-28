using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Infrastructure.Repositories
{
    public class UserRepositoryAsync : IUserRepositoryAsync
    {
        private readonly BookSwapDbContext _context;

        public UserRepositoryAsync(BookSwapDbContext context)
        {
            _context = context;
        }

       
        public  async Task<string?> GetUserNameByUserIdAsync(int userId)
        {
            var user= await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Id == userId);
            if (user == null)
            {
                return null;
            }
           
            return   user.FirstName+" "+user.LastName;
        }
    }
}
