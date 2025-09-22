﻿using BookSwap.Api.Bases;
using BookSwap.Api.Extention;
using BookSwap.Application.Abstracts;
using BookSwap.Application.Dtos.Book.Response;
using BookSwap.Application.Dtos.Book;
using BookSwap.Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookSwap.Application.Dtos.Book.Request;

namespace BookSwap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;


        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ApiResult<BookResponse>> CreateBook([FromForm] CreateBookRequest request)
        {
            var result = await _bookService.CreateBookAsync(request);
            return this.ToApiResult(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ApiResult<BookResponse>> UpdateBook([FromForm] UpdateBookRequest request)
        {
            var result = await _bookService.UpdateBookAsync(request);
            return this.ToApiResult(result);
        }

        [HttpDelete("{bookId}")]
        [Authorize]
        public async Task<ApiResult> DeleteBook(int bookId)
        {
            var result = await _bookService.DeleteBookAsync(bookId);
            return this.ToApiResult(result);
        }

        [HttpGet("{bookId}")]
        public async Task<ApiResult<BookResponse>> GetBookById(int bookId)
        {
            var result = await _bookService.GetBookByIdAsync(bookId);
            return this.ToApiResult(result);
        }

        [HttpGet]
        public async Task<ApiResult<IEnumerable<BookResponse>>> GetAllApprovedBooks()
        {
            var result = await _bookService.GetAllApprovedBooksAsync();
            return this.ToApiResult(result);
        }

        [HttpGet("Pending")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<IEnumerable<BookResponse>>> GetPendingApprovalBooks()
        {
            var result = await _bookService.GetPendingApprovalBooksAsync();
            return this.ToApiResult(result);
        }

        [HttpGet("Owner/{ownerId}")]
        [Authorize]
        public async Task<ApiResult<IEnumerable<BookResponse>>> GetBooksByOwner(int ownerId)
        {
            var result = await _bookService.GetBooksByOwnerAsync(ownerId);
            return this.ToApiResult(result);
        }

        [HttpGet("Search")]
        public async Task<ApiResult<IEnumerable<BookResponse>>> SearchBooks([FromQuery] string? searchTerm)
        {
            var result = await _bookService.SearchBooksAsync(searchTerm);
            return this.ToApiResult(result);
        }

        [HttpPost("Approve")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult> ApproveBook([FromBody] ApproveBookRequest request)
        {
            var result = await _bookService.ApproveBookAsync(request);
            return this.ToApiResult(result);
        }
        [HttpGet("RejectedBooks")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<IEnumerable<BookResponse>>> RejectedBooks()
        {
            var result = await _bookService.GetRejectedBooksAsync();
            return this.ToApiResult(result);
        }
        [HttpGet("RejectedBooks/{ownerId}")]
        public async Task<ApiResult<IEnumerable<BookResponse>>> RejectedBooksByOwnerId(int ownerId)
        {
            var result = await _bookService.GetRejectedBooksByOwnerAsync(ownerId);
            return this.ToApiResult(result);
        }
    }
}
