using Ag.Common.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserForListDto> GetUsers();
        UserDetailDto GetUser(int userId);
        void AddPerformer(int operatorId, int performerId);
    }
}
