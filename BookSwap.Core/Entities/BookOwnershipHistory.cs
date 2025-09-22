using BookSwap.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Core.Entities
{
    public class BookOwnershipHistory
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PreviousOwnerId { get; set; }
        public int NewOwnerId { get; set; }
        public int ExchangeOfferId { get; set; }
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;
        public Book Book { get; set; } = null!;
        [ForeignKey(nameof(PreviousOwnerId))]
        public User PreviousOwner { get; set; } = null!;

        [ForeignKey(nameof(NewOwnerId))]
        public User NewOwner { get; set; } = null!;

        [ForeignKey(nameof(ExchangeOfferId))]
        public ExchangeOffer ExchangeOffer { get; set; } = null!;
    }
}
