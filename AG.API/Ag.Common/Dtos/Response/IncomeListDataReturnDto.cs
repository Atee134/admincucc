using System.Collections.Generic;

namespace Ag.Common.Dtos.Response
{
    public class IncomeListDataReturnDto
    {
        public IncomeStatisticsDto OperatorStatistics { get; set; } = new IncomeStatisticsDto();

        public IncomeStatisticsDto PerformerStatistics { get; set; } = new IncomeStatisticsDto();

        public IncomeStatisticsDto StudioStatistics { get; set; } = new IncomeStatisticsDto();

        public IncomeStatisticsDto TotalStatistics { get; set; } = new IncomeStatisticsDto();

        public List<IncomeStatisticsSiteSumDto> SiteStatistics { get; set; } = new List<IncomeStatisticsSiteSumDto>();

        public List<IncomeEntryForReturnDto> IncomeEntries { get; set; } = new List<IncomeEntryForReturnDto>();
    }
}
