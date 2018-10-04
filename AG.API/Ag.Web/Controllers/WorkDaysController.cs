using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ag.Web.Controllers
{
    [Route("api")]
    [ApiController]
    public class WorkDaysController : ControllerBase
    {
        private readonly IWorkDayService _workDayService;

        public WorkDaysController(IWorkDayService workDayService)
        {
            _workDayService = workDayService;
        }

        [HttpGet("workdays/available")]
        public IActionResult GetAvailableWorkdaysInPeriod()
        {
            return Ok(_workDayService.GetDatesOfCurrentPeriod());
        }

        [HttpPost("users/{userId}/workdays")]
        public IActionResult AddWorkDay(int userId, [FromBody] WorkDayForAddDto workDayDto)
        {
            //if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            //    return Unauthorized();

            //TODO add validation if its a valid workday for this period

            _workDayService.AddWorkDay(workDayDto, userId);

            return StatusCode(201);
        }
    }
}