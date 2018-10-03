using Ag.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        UserForListDto Login(UserForLoginDto userDto);
        UserForListDto Register(UserForRegisterDto userDto);
        bool UserExists(string userName);
    }
}
