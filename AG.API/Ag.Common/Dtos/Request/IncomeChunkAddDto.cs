using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ag.Common.Dtos.Request
{
    public class IncomeChunkAddDto
    {
        [Required]
        public Site Site { get; set; }

        [Required]
        public double Income { get; set; }
    }
}
