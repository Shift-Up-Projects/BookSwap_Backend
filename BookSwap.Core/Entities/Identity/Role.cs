using Microsoft.AspNetCore.Identity;

namespace BookSwap.Core.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {
            
        }
        public Role(string roleName):base(roleName)
        {           
        }
    }
}
