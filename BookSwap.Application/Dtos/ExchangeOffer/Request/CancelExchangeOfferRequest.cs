using System.ComponentModel.DataAnnotations;
namespace BookSwap.Application.Dtos.ExchangeOffer.Request
{
    public class CancelExchangeOfferRequest
    {
        [Required]
        public int ExchangeOfferId { get; set; }
    }
}