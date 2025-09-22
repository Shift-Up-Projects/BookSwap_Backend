using BookSwap.Core.Enums;
namespace BookSwap.Application.Dtos.ExchangeOffer.Response
{
    public class ExchangeOfferResponse
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public int RequestedBookId { get; set; }
        public string RequestedBookTitle { get; set; } = string.Empty;
        public List<OfferedBookResponse> OfferedBooks { get; set; } = new List<OfferedBookResponse>();
        public ExchangeOfferStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
