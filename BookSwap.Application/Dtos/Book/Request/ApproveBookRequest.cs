using System.ComponentModel.DataAnnotations;

namespace BookSwap.Application.Dtos.Book.Request
{
    public class ApproveBookRequest
    {
        public int BookId { get; set; }
        public bool IsApproved { get; set; }
        public string RejectionReason { get; set; } = string.Empty;
    }
}