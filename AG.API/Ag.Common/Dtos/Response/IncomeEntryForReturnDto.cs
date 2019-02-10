using System;
using System.Collections.Generic;

namespace Ag.Common.Dtos.Response
{
    public class IncomeEntryForReturnDto
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public bool Locked { get; set; }

        public string Color { get; set; }

        public int OperatorId { get; set; }

        public string OperatorName { get; set; }

        public double CurrentOperatorPercent { get; set; }

        public int PerformerId { get; set; }

        public string PerformerName { get; set; }

        public double CurrentPerformerPercent { get; set; }

        public double TotalSum { get; set; }

        public double TotalIncomeForStudio { get; set; }

        public double TotalIncomeForOperator { get; set; }

        public double TotalIncomeForPerformer { get; set; }

        public List<IncomeChunkForReturnDto> IncomeChunks { get; set; }
    }
}
