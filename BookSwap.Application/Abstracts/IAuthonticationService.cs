using BookSwap.Application.Dtos.Authontication.Request;
using BookSwap.Application.Dtos.Authontication.Response;
using BookSwap.Core.Entities.Identity;
using BookSwap.Core.Results;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using ForgotPasswordRequest = BookSwap.Application.Dtos.Authontication.Request.ForgotPasswordRequest;
using ResetPasswordRequest = BookSwap.Application.Dtos.Authontication.Request.ResetPasswordRequest;
namespace BookSwap.Application.Abstracts
{
    public interface IAuthonticationService
    {
        Task<Result<SignInResponse>> SignInAsync(SignInRequest request);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result<JwtAuthResult>> RefreshAccessTokenAsync(RefreshAccessTokenRequest request);
        Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
        Task<Result> LogoutAsync(LogoutRequest request);
        Task<Result<string>> SendConfirmEmailCode(string email);
    }
}
