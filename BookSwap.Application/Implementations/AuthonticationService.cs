using BookSwap.Application.Abstracts;
using BookSwap.Application.Dtos.Authontication.Request;
using BookSwap.Application.Dtos.Authontication.Response;
using BookSwap.Core.Entities.Identity;
using BookSwap.Core.Helping;
using BookSwap.Core.Results;
using BookSwap.Infrastructure.Abstracts;
using BookSwap.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookSwap.Application.Implemetations
{
    public class AuthonticationService : IAuthonticationService
    {
        private readonly BookSwapDbContext _dBContext;
        public IUrlHelper _urlHelper { get; }
        public JwtSettings _jwtSettings { get; }
        public IRefreshTokenRepositoryAsync _refreshTokenRepository { get; }
        private UserManager<User> _userManager { get; }
        private SignInManager<User> _signInManager;
        public IHttpContextAccessor _httpContextAccessor { get; }
        public IEmailService _emailService { get; }
        public AuthonticationService(UserManager<User> userManager,
                                      BookSwapDbContext dBContext,
                                      IHttpContextAccessor httpContextAccessor,
                                      IEmailService emailService,
                                      IUrlHelper urlHelper,
                                      JwtSettings jwtSettings,
                                      IRefreshTokenRepositoryAsync refreshTokenRepositoryAsync,
                                      SignInManager<User> signInManager
            )
        {
            _dBContext = dBContext;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _urlHelper = urlHelper;
            _jwtSettings = jwtSettings;
            _refreshTokenRepository = refreshTokenRepositoryAsync;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        #region Handle Functions
        public async Task<Result<SignInResponse>> SignInAsync(SignInRequest signInDto)
        {
            var user = await _userManager.FindByEmailAsync(signInDto.Email);

            if (user is null)
                return Result<SignInResponse>.Failure($"Not Found User By Email Or Password Not Correct : {signInDto.Email} ", failureType: ResultFailureType.Unauthorized);

            if (!user.EmailConfirmed)
                return Result<SignInResponse>.BadRequest("Email Not Confirm");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, signInDto.Password, false);

            if (!signInResult.Succeeded)
                return Result<SignInResponse>.Failure("Not Found User By Email Or Password Not Correct", failureType: ResultFailureType.Unauthorized);

            // حذف أي refresh tokens قديمة
            await RemoveOldRefreshTokens(user.Id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = new SignInResponse()
            {
                GetRolesDto = userRoles.Select(x => new GetRolesDto { Name = x }),
                JwtAuthResult = await GetJWTAndRerfreshToken(user)
            };

            return Result<SignInResponse>.Success(result);
        }

        public async Task<Result<string>> SendConfirmEmailCode(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<string>.NotFound($"Not Found User With Email : {email}");

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);


            if (await _emailService.SendEmailConfirmationAsync(user.Email!, user.Id.ToString(), code))
                return Result<string>.Success("Success");
            return Result<string>.BadRequest("Failed sent email");
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest reques)
        {
            var user = await _userManager.FindByIdAsync(reques.UserId.ToString());

            if (user is null)
                return Result.NotFound($"User Not Found");

            var resultConfirmEmail = await _userManager.ConfirmEmailAsync(user, reques.Code);

            if (!resultConfirmEmail.Succeeded)
                return Result.BadRequest("Invalid confirmation code");

            return Result.Success();
        }
        public async Task<Result<JwtAuthResult>> RefreshAccessTokenAsync(RefreshAccessTokenRequest request)
        {


            try
            {
                var principal = GetPrincipalFromExpiredToken(request.AccessToken);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(request.AccessToken);
                var userId = principal.FindFirstValue(nameof(UserClaimModel.Id));
                var user = await _userManager.FindByIdAsync(userId!);
                if (user == null)
                {
                    return Result<JwtAuthResult>.NotFound("User Not Found");
                }

                var validationResult = await ValidateRefreshTokenAsync(request.AccessToken, request.RefreshToken, user.Id);
                if (!validationResult.IsSuccess)
                    return Result<JwtAuthResult>.Failure(validationResult.Message, validationResult.Errors);

                var oldRefreshToken = await _refreshTokenRepository.GetTableNoTracking()
                                                                   .FirstOrDefaultAsync(r => r.RefreshToken == request.RefreshToken);
                // حذف  Refresh Token في قاعدة البيانات
                await _refreshTokenRepository.DeleteAsync(oldRefreshToken!);



                var result = await GetJWTAndRerfreshToken(user);
                return Result<JwtAuthResult>.Success(result);

            }
            catch (Exception)
            {
                return Result<JwtAuthResult>.BadRequest("Invalid AccessToken");
                throw new SecurityTokenException();
            }


        }
        public async Task<Result> LogoutAsync(LogoutRequest logOutDto)
        {
            var token = await _refreshTokenRepository.GetTableNoTracking()
                                                     .FirstOrDefaultAsync(t => t.Token == logOutDto.RefreshToken);

            if (token != null)
            {
                token.IsRevoked = true;
                await _refreshTokenRepository.UpdateAsync(token);
            }
            return Result.Success("Logout Seuccess");
        }
        public async Task<Result> ForgotPasswordAsync(Dtos.Authontication.Request.ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result.NotFound("User not found");

            //Generate Random Number
            var chars = "0123456789";
            var random = new Random();
            var randomNumber = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());

            //update User In Database Code
            user.ResetPasswordCode = randomNumber;
            user.ResetPasswordCodeExpiry = DateTime.Now.AddMinutes(15);
            var updateResult = await _userManager.UpdateAsync(user);
            await _emailService.SendPasswordResetEmailAsync(user.Email!, user.ResetPasswordCode);

            return Result.Success();
        }
        public async Task<Result> ResetPasswordAsync(Dtos.Authontication.Request.ResetPasswordRequest request)
        {
            using var trans = await _dBContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Result.NotFound("User not found");
                if (user.ResetPasswordCode != request.Code ||
                    user.ResetPasswordCodeExpiry < DateTime.Now)
                {
                    await trans.RollbackAsync();
                    return Result.Failure("Invalid or expired reset code");
                }
                var RemovePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!RemovePasswordResult.Succeeded)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest(string.Join(", ", RemovePasswordResult.Errors.Select(e => e.Description)));
                }
                if (await _userManager.HasPasswordAsync(user))
                {
                    await  trans.RollbackAsync();
                    return Result.BadRequest("Failed Remove Password");
                }
                var AddNewPasswordresult = await _userManager.AddPasswordAsync(user, request.NewPassword);
                if (!AddNewPasswordresult.Succeeded)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest(string.Join(", ", AddNewPasswordresult.Errors.Select(e => e.Description)));
                }
                await trans.CommitAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result.Failure("internal error", failureType: ResultFailureType.InternalError);
            }
        }
        public async Task<JwtAuthResult> GetJWTAndRerfreshToken(User user)
        {
            var accessToken = await GenerateJWTToken(user);
            var refreshToken = GetRefreshToken(user.UserName!);

            var userRefreshToken = new UserRefreshToken
            {
                AddedTime = DateTime.Now,
                ExpiryDate = refreshToken.ExpireIn,
                IsUsed = true,
                IsRevoked = false,
                RefreshToken = refreshToken.RefreshTokenString,
                Token = accessToken,
                UserId = user.Id
            };
            await _refreshTokenRepository.AddAsync(userRefreshToken);

            var response = new JwtAuthResult()
            {
                RefreshToken = refreshToken,
                AccessToken = accessToken
            };

            return response;
        }

        // Private helper methods
        private async Task<string> GenerateJWTToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = await GetClaims(user, userRoles);

            var jwtToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(_jwtSettings.AccessTokenExpireDate),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256Signature)
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return accessToken;
        }
        private RefreshTokenResult GetRefreshToken(string username)
        {
            var refreshToken = new RefreshTokenResult
            {
                ExpireIn = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                UserName = username,
                RefreshTokenString = GenerateRefreshToken()
            };
            return refreshToken;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task<List<Claim>> GetClaims(User user, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(nameof(UserClaimModel.Id), user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            return claims;
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = false // تجاهل انتهاء الصلاحية
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        private async Task<Result> ValidateRefreshTokenAsync(string accessToken, string refreshToken, int userId)
        {
            var storedToken = await _refreshTokenRepository.GetTableNoTracking()
                                                           .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            if (storedToken == null)
                return Result.Failure("Invalid refresh token");

            if (storedToken.Token != accessToken)
                return Result.Failure("Token mismatch");

            if (storedToken.UserId != userId)
                return Result.Failure("User mismatch");

            if (storedToken.ExpiryDate < DateTime.UtcNow)
                return Result.Failure("Refresh token expired");

            if (storedToken.IsRevoked)
                return Result.Failure("Refresh token revoked");

            return Result.Success();
        }

        public async Task<JwtAuthResult> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime expiryDate, string refreshToken)
        {
            var newToken = await GenerateJWTToken(user);
            var response = new JwtAuthResult();
            response.AccessToken = newToken;
            var refreshTokenResult = new RefreshTokenResult();
            refreshTokenResult.UserName = user.UserName!;
            refreshTokenResult.RefreshTokenString = refreshToken;
            refreshTokenResult.ExpireIn = expiryDate;
            response.RefreshToken = refreshTokenResult;
            return response;

        }
        private JwtSecurityToken ReadJWTToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }
        private string ValidateToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { _jwtSettings.Issuer },
                ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                ValidAudience = _jwtSettings.Audience,
                ValidateAudience = _jwtSettings.ValidateAudience,
                ValidateLifetime = false,
            };
            try
            {
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);

                if (validator == null)
                {
                    return "InvalidToken";
                }

                return "NotExpired";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task SaveRefreshTokenAsync(int userId, string accessToken, string refreshToken)
        {
            var refreshTokenEntity = new UserRefreshToken
            {
                UserId = userId,
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDate),
                AddedTime = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        }
        private async Task RemoveOldRefreshTokens(int userId)
        {
            var oldTokens = await _refreshTokenRepository.GetByUserIdAsync(userId);
            foreach (var token in oldTokens)
            {
                await _refreshTokenRepository.DeleteAsync(token);
            }
        }
        #endregion
    }
}