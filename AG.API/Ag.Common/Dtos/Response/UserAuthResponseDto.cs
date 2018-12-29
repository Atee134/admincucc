namespace Ag.Common.Dtos.Response
{
    public class UserAuthResponseDto
    {
        public string Token { get; set; }

        public UserForListDto User { get; set; }
    }
}
