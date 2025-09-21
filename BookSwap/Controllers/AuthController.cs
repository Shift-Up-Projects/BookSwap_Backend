// في BookExchange.API/Controllers/AuthController.cs
using Azure;
using BookSwap.Api.Extention;
using BookSwap.Api.Bases;
using BookSwap.Application.Abstracts;
using BookSwap.Application.Dtos.Authontication.Request;
using BookSwap.Application.Dtos.Authontication.Response;
using BookSwap.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BookSwap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthonticationService _authApiService;

        public AuthController(IAuthonticationService authApiService)
        {
            _authApiService = authApiService;
        }

        [HttpPost("SignIn")]
        public async Task<ApiResult<SignInResponse>> SignIn([FromBody] SignInRequest request)
        {
            var result = await _authApiService.SignInAsync(request);
            if (!result.IsSuccess)
                return this.ToApiResult(result);
            return this.ToApiResult(result);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<ApiResult> ConfirmEmail(string userId , string code)
        {
            var result = await _authApiService.ConfirmEmailAsync(new ConfirmEmailRequest
            {
                Code = code,
                UserId = userId
            });
            return this.ToApiResult(result);
        }

        [HttpPost("RefreshToken")]
        public async Task<ApiResult<AuthResponse>> RefreshToken([FromBody] RefreshAccessTokenRequest request)
        {
            var result = await _authApiService.RefreshAccessTokenAsync(request);
            if (!result.IsSuccess)
                return this.ToApiResult(Result<AuthResponse>.Failure(result.Message, result.Errors, result.FailureType));

            var response = new AuthResponse
            {
                AccessToken = result.Value!.AccessToken,
                RefreshToken = result.Value.RefreshToken.RefreshTokenString,
                ExpiresIn = result.Value.RefreshToken.ExpireIn
            };
            return this.ToApiResult(Result<AuthResponse>.Success(response, result.Message));


        }

        [HttpPost("ForgotPassword")]
        public async Task<ApiResult> ForgotPassword([FromBody] Application.Dtos.Authontication.Request.ForgotPasswordRequest request)
        {
            var result = await _authApiService.ForgotPasswordAsync(request);
            return this.ToApiResult(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<ApiResult> ResetPassword([FromBody] Application.Dtos.Authontication.Request.ResetPasswordRequest request)
        {
            var result = await _authApiService.ResetPasswordAsync(request);
            return this.ToApiResult(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ApiResult> Logout([FromBody] LogoutRequest request)
        {
            var result = await _authApiService.LogoutAsync(request);
            return this.ToApiResult(result);
        }
    }
}
