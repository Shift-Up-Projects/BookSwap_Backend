using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Core.Entities
{
    public class OfferedBook
    {
        public int Id { get; set; }
        public int ExchangeOfferId { get; set; }
        public int BookId { get; set; }
        public bool IsSelected { get; set; }

        [ForeignKey(nameof(ExchangeOfferId))]
        public ExchangeOffer ExchangeOffer { get; set; } = null!;

        [ForeignKey(nameof(BookId))]
        public Book Book { get; set; } = null!;
    }
}
