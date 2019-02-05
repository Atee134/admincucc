using System;
using System.Collections.Generic;

namespace Ag.Common.Dtos.Response
{
    public class IncomeListDataReturnDto
    {
        public IncomeStatisticsDto OperatorStatistics { get; set; }

        public IncomeStatisticsDto PerformerStatistics { get; set; }

        public IncomeStatisticsDto StudioStatistics { get; set; }

        public IncomeStatisticsDto TotalStatistics { get; set; }

        public List<IncomeStatisticsSiteSumDto> SiteStatistics { get; set; }

        public List<IncomeEntryForReturnDto> IncomeEntries { get; set; }
    }
}
