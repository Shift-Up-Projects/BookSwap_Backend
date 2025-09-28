using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Application.Dtos.ExchangeOffer.Response
{
   public class AcceptExchangeOfferResponse
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public int RequestedBookId { get; set; } 
        public int OfferBookId {  get; set; }
        public string? RequestedBookTitle { get; set; }
        public string? OfferedBookTitle {  get; set; }
        public string? RequestedBookImage { get; set; }
        public string? OfferedBookImage {  get; set; }

       
    }
}
