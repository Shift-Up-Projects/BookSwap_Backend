using System.ComponentModel.DataAnnotations;

namespace BookSwap.Application.Dtos.Authontication.Request
{
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
