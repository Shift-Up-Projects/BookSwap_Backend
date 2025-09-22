using BookSwap.Core.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BookSwap.Application.Dtos.Book.Request
{
    public class UpdateBookRequest
    { 
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public BookCondition Condition { get; set; }
        public BookStatus BookStatus { get; set; } = BookStatus.Available;
        public int CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}