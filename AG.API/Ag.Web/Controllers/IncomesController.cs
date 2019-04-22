using Ag.BusinessLogic.Interfaces;
using Ag.BusinessLogic.Models;
using Ag.Common.Dtos.Request;
using Ag.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ag.Web.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ServiceFilter(typeof(ActionLogFilterAttribute))]
    [ApiController]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomeService _incomeService;

        public IncomesController(IIncomeService incomeService)
        {
            _incomeService = incomeService;
        }

        [HttpGet]
        public IActionResult GetIncomes([FromQuery] IncomeListFilterParams filterParams)
        {
            if (!filterParams.UserId.HasValue || filterParams.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var incomeEntries = _incomeService.GetIncomeEntries(filterParams);
            return Ok(incomeEntries);
        }

        [HttpGet("~/api/incomes")]
        [Authorize("Admin")]
        public IActionResult GetAllIncomes([FromQuery] IncomeListFilterParams filterParams)
        {
            var incomeEntries = _incomeService.GetIncomeEntries(filterParams);
            return Ok(incomeEntries);
        }

        [HttpGet("{incomeId}")]
        [Authorize(Roles = "Operator, Admin")]
        public IActionResult GetIncome(int userId, long incomeId)
        {
            if (!User.IsInRole("Admin") && userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var incomeEntry = _incomeService.GetIncomeEntry(incomeId);

            return Ok(incomeEntry);
        }

        [HttpPost]
        [Authorize(Roles = "Operator, Admin")]
        public IActionResult AddIncome(int userId, IncomeEntryAddDto incomeEntryDto)
        {
            if (!User.IsInRole("Admin") && userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var addedIncomeEntry = _incomeService.AddIncomEntry(userId, incomeEntryDto);
            return Ok(addedIncomeEntry);
        }

        [HttpPut("{incomeId}")]
        [Authorize(Roles = "Operator, Admin")]
        public IActionResult UpdateIncome(int userId, long incomeId, IncomeEntryUpdateDto incomeEntryDto)
        {
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin && userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            if (!isAdmin)
            {
                _incomeService.ValidateAuthorityToUpdateIncome(userId, incomeId);
            }

            var incomeEntry = _incomeService.UpdateIncomeEntry(incomeId, incomeEntryDto);

            return Ok(incomeEntry);
        }

        [HttpDelete("~/api/incomes/{incomeId}")]
        [Authorize("Admin")]
        public IActionResult DeleteIncome(long incomeId)
        {
            _incomeService.DeleteIncomeEntry(incomeId);

            return NoContent();
        }

        [HttpPut("{incomeId}/lock")]
        [Authorize("Admin")]
        public IActionResult LockIncome(int userId, long incomeId)
        {
            bool result = _incomeService.UpdateIncomeEntryLockedState(incomeId, true);

            return Ok(result);
        }

        [HttpPut("{incomeId}/unlock")]
        [Authorize("Admin")]
        public IActionResult UnlockIncome(int userId, long incomeId)
        {
            bool result = _incomeService.UpdateIncomeEntryLockedState(incomeId, false);

            return Ok(result);
        }
    }
}