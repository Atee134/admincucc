using Ag.Common.Dtos.Response;
using Ag.Domain.Models;

namespace Ag.BusinessLogic.Interfaces.Converters
{
    public interface IUserConverter
    {
        UserForListDto ConvertToUserToListDto(User user);
        UserDetailDto ConvertToUserDetailDto(User user);
    }
}
