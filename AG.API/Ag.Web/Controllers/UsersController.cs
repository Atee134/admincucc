using System.Linq;
using System.Security.Claims;
using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Interfaces.Converters;
using Ag.Common.Dtos.Request;
using Ag.Common.Enums;
using Ag.Web.Filters;
using Microsoft.AspNetCore.Authorization;
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
            if (_userService.GetUser(userDto.Id.Value).Role == Role.Admin && userDto.Id.Value != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            _userService.UpdateUser(userDto);

            return NoContent();
        }

        [Authorize(Roles = "Operator, Admin")]
        [HttpGet("{userId}/colleagues")]
        public IActionResult GetColleagues(int userId)
        {
            if (!User.IsInRole("Admin") && userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var colleagues = _joinTableHelperService.GetColleagues(userId).Select(c => _userConverter.ConvertToUserToListDto(c)).ToList();

            return Ok(colleagues);
        }

        [Authorize("Admin")]
        [HttpPut("{operatorId}/performer/{performerId}")]
        public IActionResult AddPerformer(int operatorId, int performerId)
        {
            _userService.AddPerformer(operatorId, performerId);

            return NoContent();
        }

        [Authorize("Admin")]
        [HttpDelete("{operatorId}/performer/{performerId}")]
        public IActionResult RemovePerformer(int operatorId, int performerId)
        {
            _userService.RemovePerformer(operatorId, performerId);

            return NoContent();
        }
    }
}