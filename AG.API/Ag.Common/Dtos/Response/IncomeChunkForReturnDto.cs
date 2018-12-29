using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos.Response
{
    public class IncomeChunkForReturnDto
    {
        public long Id { get; set; }

        public Site Site { get; set; }

        public double IncomeForOwner { get; set; }

        public double IncomeForOperator { get; set; }

        public double IncomeForPerformer { get; set; }
    }
}
