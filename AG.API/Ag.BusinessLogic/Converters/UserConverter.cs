using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Interfaces.Converters;
using Ag.Common.Dtos.Response;
using Ag.Common.Enums;
using Ag.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ag.BusinessLogic.Converters
{
    public class UserConverter : IUserConverter
    {
        private readonly IJoinTableHelperService _joinTableHelperService;

        public UserConverter(IJoinTableHelperService joinTableHelperService)
        {
            _joinTableHelperService = joinTableHelperService;
        }

        public UserForListDto ConvertToUserToListDto(User user)
        {
            return new UserForListDto
            {
                Id = user.Id,
                UserName = user.UserName,
                LastPercent = user.LastPercent,
                Role = user.Role,
                Shift = user.Shift,
            };
        }

        public UserDetailDto ConvertToUserDetailDto(User user)
        {
            var sites = user.Sites.Length == 0 ? new List<Site>() : user.Sites.Split(';').Select(s => Enum.Parse<Site>(s)).ToList();
            var colleagues = _joinTableHelperService.GetColleagues(user.Id).Select(u => ConvertToUserToListDto(u)).ToList();

            return new UserDetailDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Shift = user.Shift,
                Role = user.Role,
                Sites = sites,
                Colleagues = colleagues,
                MinPercent = user.MinPercent,
                MaxPercent = user.MaxPercent,
            };
        }
    }
}
