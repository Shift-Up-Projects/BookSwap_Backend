using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BookSwap.Core.Entities.Identity;

namespace BookSwap.Infrastructure.Context
{
    public class BookSwapDbContext :  IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>
                                                         , IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
   
    {
        public BookSwapDbContext(DbContextOptions options)  : base(options)
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
