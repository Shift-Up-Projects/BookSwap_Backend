using System.ComponentModel.DataAnnotations;
namespace BookSwap.Application.Dtos.ExchangeOffer.Request
{
    public class RejectExchangeOfferRequest
    {
        [Required]
        public int ExchangeOfferId { get; set; }
    }
}
