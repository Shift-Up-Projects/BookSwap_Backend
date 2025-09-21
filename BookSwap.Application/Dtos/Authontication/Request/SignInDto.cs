using System.ComponentModel.DataAnnotations;

namespace BookSwap.Application.Dtos.Authontication.Request
{
    public class SignInRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
