using Ag.Common.Enums;

namespace Ag.Common.Dtos.Response
{
    public class IncomeStatisticsSiteSumDto
    {
        public Site Site { get; set; }

        public IncomeStatisticsDto Statistics { get; set; }
    }
}
