using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        UserDetailDto Login(UserForLoginDto userDto);
        UserForListDto Register(UserForRegisterDto userDto);
        bool UserExists(string userName);
    }
}
