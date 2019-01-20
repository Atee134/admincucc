using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ag.BusinessLogic.Interfaces;
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

        public UsersController(IUserService userService)
        {
           _userService = userService;
        }

        [HttpGet]
        [Authorize("Admin")]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize("Admin")]
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUser(id);

            return Ok(user);
        }

        [Authorize("Admin")]
        [HttpPut("{operatorId}/performer/{performerId}")]
        public IActionResult AddPerformer(int operatorId, int performerId)
        {
            _userService.AddPerformer(operatorId, performerId);
            //TODO handle exception? or let the middleware handle it and return the relevant info

            return NoContent();
        }
    }
}