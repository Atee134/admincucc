using System;
using System.Collections.Generic;

namespace Ag.Common.Dtos.Response
{
    public class IncomeEntryForReturnDto
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public string OperatorName { get; set; }

        public string PerformerName { get; set; }

        public double TotalSum { get; set; }

        public double TotalIncomeForOwner { get; set; }

        public double TotalIncomeForOperator { get; set; }

        public double TotalIncomeForPerformer { get; set; }

        // TODO for admin earning listing it may need an OP id and perf ID

        public List<IncomeChunkForReturnDto> IncomeChunks { get; set; }
    }
}
