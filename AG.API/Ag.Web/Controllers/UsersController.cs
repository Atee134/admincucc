using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Interfaces.Converters;
using Ag.Common.Dtos.Request;
using Ag.Common.Enums;
using Ag.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ag.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(ActionLogFilterAttribute))]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJoinTableHelperService _joinTableHelperService;
        private readonly IUserConverter _userConverter;

        public UsersController(IUserService userService, IJoinTableHelperService joinTableHelperService, IUserConverter userConverter)
        {
           _userService = userService;
           _joinTableHelperService = joinTableHelperService;
            _userConverter = userConverter;
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult GetUsers([FromQuery] Role? role = null)
        {
            var users = _userService.GetUsers(role);

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize("Admin")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUser(id);

            return Ok(user);
        }

        [HttpPut]
        [Authorize("Admin")]
        public IActionResult UpdateUser(UserForEditDto userDto)
        {
            // TODO validate if the userDto role is admin, only the logged in user id == userdto.id can edit that

            _userService.UpdateUser(userDto);

            return NoContent();
        }

        [Authorize(Roles = "Operator, Admin")]
        [HttpGet("{userId}/colleagues")]
        public IActionResult GetColleagues(int userId)
        {
            var colleagues = _joinTableHelperService.GetColleagues(userId).Select(c => _userConverter.ConvertToUserToListDto(c)).ToList();

            return Ok(colleagues);
        }

        [Authorize("Admin")]
        [HttpPut("{operatorId}/performer/{performerId}")]
        public IActionResult AddPerformer(int operatorId, int performerId)
        {
            _userService.AddPerformer(operatorId, performerId);
            //TODO handle exception? or let the middleware handle it and return the relevant info

            return NoContent();
        }

        [Authorize("Admin")]
        [HttpDelete("{operatorId}/performer/{performerId}")]
        public IActionResult RemovePerformer(int operatorId, int performerId)
        {
            _userService.RemovePerformer(operatorId, performerId);
            //TODO handle exception? or let the middleware handle it and return the relevant info

            return NoContent();
        }
    }
}