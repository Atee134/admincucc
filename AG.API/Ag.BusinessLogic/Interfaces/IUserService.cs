using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserForListDto> GetUsers(Role? role = null);
        UserDetailDto GetUser(int userId);
        void UpdateUser(UserForEditDto userDto);
        void AddPerformer(int operatorId, int performerId);
        void RemovePerformer(int operatorId, int performerId);
    }
}
