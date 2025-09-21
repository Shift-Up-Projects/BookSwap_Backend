namespace BookSwap.Application.Dtos.Authontication.Response
{
    public class RefreshTokenResult
    {
        public string UserName { get; set; }
        public string RefreshTokenString { get; set; }
        public DateTime ExpireIn { get; set; }
    }
}
