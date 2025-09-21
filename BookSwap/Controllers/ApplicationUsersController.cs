using BookSwap.Api.Bases;
using BookSwap.Api.Extention;
using BookSwap.Application.Abstracts;
using BookSwap.Application.Dtos.User.Request;
using BookSwap.Application.Dtos.User.Responce;
using BookSwap.Core.Helping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSwap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        public IApplicationUserService _applicationUserService { get; }
        public ICurrentUserService _currentUserService { get; }

        public ApplicationUsersController(IApplicationUserService applicationUser, ICurrentUserService currentUserService)
        {
            _applicationUserService = applicationUser;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ApiResult> AddNewUser(AddUserDto userDto)
        {
            var AddNewUserResult = await _applicationUserService.RegisterAsync(userDto);
            if (!AddNewUserResult.IsSuccess)
            {
                return this.ToApiResult(AddNewUserResult);
            }
            return ApiResult.Created(AddNewUserResult.Message!);
        }
        [HttpPut()]
        [Authorize(Roles = "User,Admin")]
        public async Task<ApiResult> UpdateUser([FromForm] UpdateUserDto userDto)
        {
            var UpdateUserResult = await _applicationUserService.UpdateUserAsync(userDto);
            if (!UpdateUserResult.IsSuccess)
            {
                return this.ToApiResult(UpdateUserResult);
            }
            return ApiResult.Ok(UpdateUserResult.Message!);

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<ApiResult> DeleteUser(int Id)
        {
            var DeleteUserResult = await _applicationUserService.DeleteUserAsync(Id);
            if (!DeleteUserResult.IsSuccess)
            {
                return this.ToApiResult(DeleteUserResult);
            }
            return ApiResult.Ok(DeleteUserResult.Message!);

        }
        [HttpGet("UserInformation")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ApiResult<GetUserByIdDto>> GetUserInformation()
        {
            var user = await _currentUserService.GetUserAsync();
            var UserResult = await _applicationUserService.GetUserByIdAsync(user.Id);
            if (!UserResult.IsSuccess)
            {
                return this.ToApiResult<GetUserByIdDto>(UserResult);
            }
            return ApiResult<GetUserByIdDto>.Ok(UserResult.Value!);

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<IEnumerable<GetUsersDto>>> GetUsers()
        {
            var UsersResult = await _applicationUserService.GetUsersAsync();
            if (!UsersResult.IsSuccess)
            {
                return this.ToApiResult<IEnumerable<GetUsersDto>>(UsersResult);
            }
            return ApiResult<IEnumerable<GetUsersDto>>.Ok(UsersResult.Value!);

        }
        [HttpGet("Pagination")]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<PaginatedResult<GetUsersDto>>> GetUsers(string? search, int pageNumber = 1, int pageSize = 10)
        {
            var UsersResult = await _applicationUserService.GetUsersPaginationAsync(search, pageNumber, pageSize);
            if (!UsersResult.IsSuccess)
            {
                return this.ToApiResult<PaginatedResult<GetUsersDto>>(UsersResult);
            }
            return ApiResult<PaginatedResult<GetUsersDto>>.Ok(UsersResult.Value!);

        }
    }
}
