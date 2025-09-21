using System.ComponentModel.DataAnnotations;

namespace BookSwap.Application.Dtos.Authontication.Request
{
    public class ConfirmEmailRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
