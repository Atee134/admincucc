using System;
using System.Collections.Generic;

namespace Ag.Common.Dtos.Request
{
    public class IncomeEntryUpdateDto
    {
        public DateTime? Date { get; set; }

        public List<IncomeChunkUpdateDto> IncomeChunks { get; set; } = new List<IncomeChunkUpdateDto>();

        public int? PerformerId { get; set; }
    }
}
