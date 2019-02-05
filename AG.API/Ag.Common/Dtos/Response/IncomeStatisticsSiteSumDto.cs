using Ag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ag.Common.Dtos.Response
{
    public class IncomeStatisticsSiteSumDto
    {
        public Site Site { get; set; }

        public IncomeStatisticsDto Statistics { get; set; }
    }
}
