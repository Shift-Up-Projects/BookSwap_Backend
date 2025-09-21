namespace BookSwap.Application.Dtos.Authontication.Response
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public RefreshTokenResult RefreshToken { get; set; }
    }

}
