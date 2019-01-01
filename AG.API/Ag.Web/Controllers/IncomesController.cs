using Ag.BusinessLogic.Interfaces;
using Ag.Common.Dtos.Request;
using Ag.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ag.Web.Controllers
{
  //  [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ServiceFilter(typeof(ExceptionHandlerFilterAttribute))]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomesController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        public IActionResult GetIncomes(int userId)
        {
            var incomeEntries = _incomeService.GetIncomeEntries(userId);
            return Ok(incomeEntries);
        }

        [HttpPost]
        public IActionResult AddIncome(int userId, IncomeEntryAddDto incomeEntryDto)
        {
            var addedIncomeEntry = _incomeService.AddIncomEntry(userId, incomeEntryDto);
            return Ok(addedIncomeEntry);
        }
    }
}
