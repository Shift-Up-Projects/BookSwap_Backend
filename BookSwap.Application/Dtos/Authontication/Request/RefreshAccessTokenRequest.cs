namespace BookSwap.Application.Dtos.Authontication.Request
{
    public class RefreshAccessTokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
