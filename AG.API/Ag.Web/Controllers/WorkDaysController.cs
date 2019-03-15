using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos;
using Ag.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ag.Web.Controllers
{
    /// <summary>
    /// OBSOLETE
    /// </summary>
  //  [Authorize]
    [Route("api")]
    [ServiceFilter(typeof(ActionLogFilterAttribute))]
    [ApiController]
    public class WorkDaysController : ControllerBase
    {
     //   private readonly IWorkDayService _workDayService;

     //   public WorkDaysController(IWorkDayService workDayService)
     //   {
     //       _workDayService = workDayService;
     //   }

     //   [HttpGet("workdays/{userId}")]
     //   public IActionResult GetCurrentWorkDaysOfUser(int userId)
     //   {
     //       return Ok(_workDayService.GetCurrentWorkDaysOfUser(userId));
     //   }

     //   [HttpGet("workdays/available")]
     //   public IActionResult GetModifiableWorkDaysInPeriod()
     //   {
     //       return Ok(_workDayService.GetModifiableWorkDays());
     //   }

     ////   [Authorize("Operator")]
     //   [HttpPost("users/{userId}/workdays/{date}")]
     //   public IActionResult AddWorkDay(int userId, DateTime date)
     //   {
     //       //if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
     //       //    return Unauthorized();

     //       //TODO add validation if its a valid workday for this period

     //       _workDayService.AddWorkDay(date, userId);

     //       return StatusCode(201);
     //   }

     // //  [Authorize("Operator")]
     //   [HttpDelete("users/{userId}/workdays/{date}")]
     //   public IActionResult RemoveWorkDay(int userId, DateTime date)
     //   {
     //       //if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
     //       //    return Unauthorized();

     //       //TODO add validation if its a valid workday for this period

     //       _workDayService.RemoveWorkDay(date, userId);

     //       return NoContent();
     //   }
    }
}