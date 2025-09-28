using BookSwap.Api.Bases;
using BookSwap.Api.Extention;
using BookSwap.Application.Abstracts;
using BookSwap.Application.Dtos.ExchangeOffer.Request;
using BookSwap.Application.Dtos.ExchangeOffer.Response;
using BookSwap.Application.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BookSwap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeOfferController : ControllerBase
    {
       private readonly IExchangeOfferService _exchangeOfferService;

        public ExchangeOfferController(IExchangeOfferService exchangeOfferService)
        {
            _exchangeOfferService = exchangeOfferService;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ApiResult<ExchangeOfferResponse>> AddNewExchangeOffer([FromBody]CreateExchangeOfferRequest request,int senderId)
        {
           
            var result = await _exchangeOfferService.CreateExchangeOfferAsync(request, senderId);
                 
            return this.ToApiResult(result);
        }
        [HttpPut("accept/{reciverId}")]
        [Authorize]
        public async Task<ApiResult<AcceptExchangeOfferResponse>> AcceptedExchangeOffer([FromBody]AcceptExchangeOfferRequest request,int reciverId)
        {
            var result=await _exchangeOfferService.AccepteExchangeOfferRequest(request, reciverId);  
            return this.ToApiResult(result);
        }
        [HttpPut("cancel/{senderId}")]
        [Authorize]
        public async Task<ApiResult<bool>> CancelledExchangeOffer([FromBody]CancelExchangeOfferRequest request,int senderId)
        {
            var result=await _exchangeOfferService.CancelExchangeOfferRequest(request, senderId);
            return this.ToApiResult(result);
        }
        [HttpPut("rejected/{reciverId}")]
        [Authorize]
        public async Task<ApiResult<bool>> RejectedExchangeOffer([FromBody] CancelExchangeOfferRequest request, int reciverId)
        {
            var result = await _exchangeOfferService.RejectedExchangeOfferRequest(request, reciverId);
            return this.ToApiResult(result);
        }

    }
}
