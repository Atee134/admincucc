using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using Ag.Domain.Models;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        UserDetailDto Login(UserForLoginDto userDto);
        UserForListDto Register(UserForRegisterDto userDto);
        void ChangeUserPassword(User user, string newPassword);
        bool UserExists(string userName);
    }
}
