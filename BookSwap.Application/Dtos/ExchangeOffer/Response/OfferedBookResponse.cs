using BookSwap.Core.Enums;
namespace BookSwap.Application.Dtos.ExchangeOffer.Response
{
    public class OfferedBookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public BookCondition Condition { get; set; }
        public bool IsSelected { get; set; }
    }
}