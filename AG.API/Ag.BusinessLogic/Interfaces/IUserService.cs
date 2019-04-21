using Ag.Common.Dtos.Request;
using Ag.Common.Dtos.Response;
using Ag.Common.Enums;
using System.Collections.Generic;

namespace Ag.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserForListDto> GetUsers(Role? role = null);
        UserDetailDto GetUser(int userId);
        void UpdateUser(UserForEditDto userDto);
        void AddPerformer(int operatorId, int performerId);
        void RemovePerformer(int operatorId, int performerId);
        void ChangeColor(int operatorId, int performerId, string color);
        string GetColor(int operatorId, int performerId);
    }
}
