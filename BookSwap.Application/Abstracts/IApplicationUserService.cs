using BookSwap.Application.Dtos.User.Request;
using BookSwap.Application.Dtos.User.Responce;
using BookSwap.Core.Helping;
using BookSwap.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace BookSwap.Application.Abstracts
{
    public interface IApplicationUserService
    {
        public Task<Result> RegisterAsync(AddUserDto userDtos);
        public Task<Result> UpdateUserAsync(UpdateUserDto userDtos);
        public Task<Result> DeleteUserAsync(int Id);
        public Task<Result<string>> ChangePasswordAsync(ChangeUserPassword changeUserPassword);
        public Task<Result<GetUserByIdDto>> GetUserByIdAsync(int Id);
        public Task<Result<IEnumerable<GetUsersDto>>> GetUsersAsync();
        public Task<Result<PaginatedResult<GetUsersDto>>> GetUsersPaginationAsync(string? search, int pageNumber , int pageSize );

    }
}
