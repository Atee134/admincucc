using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Ag.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StaticDataController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StaticDataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("colors")]
        [Authorize("Admin")]
        public IActionResult GetColors()
        {
            IConfigurationSection colors = _configuration.GetSection("UserColors");
            var colorsArray = colors.AsEnumerable().Where(c => c.Value != null).OrderBy(c => c.Key).Select(c => c.Value).ToList();

            return Ok(colorsArray);
        }
    }
}