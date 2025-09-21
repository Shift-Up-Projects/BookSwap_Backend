using System.ComponentModel.DataAnnotations;

namespace BookSwap.Application.Dtos.Authontication.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
