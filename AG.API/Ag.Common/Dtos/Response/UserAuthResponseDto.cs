namespace Ag.Common.Dtos.Response
{
    public class UserAuthResponseDto
    {
        public string Token { get; set; }

        public UserDetailDto User { get; set; }
    }
}
