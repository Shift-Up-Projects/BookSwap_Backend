using BookSwap.Application.Dtos.Book.Request;
using BookSwap.Application.Dtos.Book.Response;
using BookSwap.Application.Dtos.ExchangeOffer.Request;
using BookSwap.Application.Dtos.ExchangeOffer.Response;
using BookSwap.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSwap.Application.Abstracts
{
   public interface IExchangeOfferService
    {
      Task<Result<ExchangeOfferResponse>> CreateExchangeOfferAsync(CreateExchangeOfferRequest request,int senderId);
      Task<Result<AcceptExchangeOfferResponse>> AccepteExchangeOfferRequest(AcceptExchangeOfferRequest request, int reciverId);
        Task<Result<bool>> CancelExchangeOfferRequest(CancelExchangeOfferRequest request, int senderId);
        Task<Result<bool>> RejectedExchangeOfferRequest(CancelExchangeOfferRequest request, int reciverId);
    }
}
