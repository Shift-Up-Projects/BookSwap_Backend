using BookSwap.Core.Entities.Identity;
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
        public string Condition { get; set; } = string.Empty; // e.g., New, Used, Damaged
        public bool IsAvailable { get; set; } = true;
        public int OwnerId { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public virtual User Owner { get; set; } = null!; }
}
