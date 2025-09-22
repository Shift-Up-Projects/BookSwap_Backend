using BookSwap.Core.Entities.Identity;
using BookSwap.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Core.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public string? Description { get; set; }
        public string CoverImageUrl { get; set; }
        public BookCondition Condition { get; set; }
        public BookStatus Status { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int OwnerId { get; set; }
        public bool IsApproved { get; set; } = false;
        public string RejectionReason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual User Owner { get; set; } = null!;

        public IEnumerable<OfferedBook> OfferedBooks { get; set; } = new List<OfferedBook>();
        public IEnumerable<ExchangeOffer> ExchangeOffers { get; set; } = new List<ExchangeOffer>();

    }
}
