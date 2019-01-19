using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ag.Common.Dtos.Request
{
    public class IncomeEntryAddDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public List<IncomeChunkAddDto> IncomeChunks { get; set; }

        public int? PerformerId { get; set; }
    }
}
