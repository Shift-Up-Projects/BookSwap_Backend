using System.ComponentModel.DataAnnotations;
namespace BookSwap.Application.Dtos.ExchangeOffer.Request
{
    public class AcceptExchangeOfferRequest
    {
        [Required]
        public int ExchangeOfferId { get; set; }
        [Required]
        public int SelectedBookId { get; set; } // الكتاب المختار من العرض
    }
}
