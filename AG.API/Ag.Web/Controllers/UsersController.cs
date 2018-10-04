using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ag.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ag.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
           _userService = userService;
        }

        [HttpPut("{operatorId}/performer/{performerId}")]
        public IActionResult AddPerformer(int operatorId, int performerId)
        {
            _userService.AddPerformer(operatorId, performerId);
            //TODO handle exception? or let the middleware handle it and return the relevant info

            return NoContent();
        }
    }
}