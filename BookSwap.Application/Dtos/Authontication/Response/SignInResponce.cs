namespace BookSwap.Application.Dtos.Authontication.Response
{
    public class SignInResponse
    {
        public IEnumerable<GetRolesDto> GetRolesDto { get; set; }
        public JwtAuthResult JwtAuthResult { get; set; }
    }

}
