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
    public class WorkDaysController : ControllerBase
    {
        private readonly IWorkDayService _workDayService;

        public WorkDaysController(IWorkDayService workDayService)
        {
            _workDayService = workDayService;
        }

        [HttpGet("available")]
        public IActionResult GetAvailableWorkdaysInPeriod()
        {
            return new JsonResult(_workDayService.GetDatesOfCurrentPeriod());
        }
    }
}